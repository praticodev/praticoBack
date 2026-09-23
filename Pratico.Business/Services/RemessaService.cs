using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Pratico.Business.Utils;
using Pratico.Dominio.Intefaces;
using Pratico.Dominio.Intefaces.Repository;
using Pratico.Dominio.Intefaces.Service;
using Pratico.Dominio.Model;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Pratico.Business.Services
{
    public class RemessaService : BaseService, IRemessaService
    {
        private readonly IRemessaRepository _remessaRepository;
        private readonly IConjuntoRepository _conjuntoRepository;
        private readonly ICondominioRepository _condominioRepository;
        private readonly IRemessaInternaRepository _remessaInternaRepository;
        private readonly IPessoaRepository _pessoaRepository;
        private readonly IContatoRepository _contatoRepository;
        private readonly IEmailSenGridService _emailSenGridService;
        private readonly ISmsService _smsService;
        private readonly ISyncLogRepository _syncLogRepository;

        public RemessaService(IRemessaRepository remessaRepository,
                              ICondominioRepository condominioRepository,
                              INotificador notificador,
                              IRemessaInternaRepository remessaInternaRepository,
                              IConjuntoRepository conjuntoRepository,
                              IPessoaRepository pessoaRepository,
                              IContatoRepository contatoRepository,
                              IEmailSenGridService emailSenGridService,
                              ISyncLogRepository syncLogRepository,
                              ISmsService smsService) : base(notificador)
        {
            _remessaRepository = remessaRepository;
            _condominioRepository = condominioRepository;
            _remessaInternaRepository = remessaInternaRepository;
            _conjuntoRepository = conjuntoRepository;
            _pessoaRepository = pessoaRepository;
            _contatoRepository = contatoRepository;
            _emailSenGridService = emailSenGridService;
            _smsService = smsService;
            _syncLogRepository = syncLogRepository;
        }

        public async Task<RemessaExterna> Adicionar(RemessaExterna remessa, int codCondominio, IFormFile? imagem)
        {
            try
            {
                if (imagem != null)
                    remessa.Imagem = await InsereBlobImagem(imagem);

                var condominio = await _condominioRepository.ObterCondominioPorCodigo(codCondominio);
                var numero = await ObterProximoNumeroRemessa(condominio.Id);
                remessa.CondominioId = condominio.Id;
                remessa.Numero = numero + DateTime.Now.ToString("ddMMyyyy");
                return await _remessaRepository.Adicionar(remessa);                
            }
            catch (Exception e)
            {

                throw e;
            }
            
        }

        public async Task<RemessaInterna> AdicionarInterna(RemessaInterna remessa, int codCondominio, IFormFile? imagem)
        {
            
            try
            {
                if (imagem != null)
                    remessa.Imagem = await InsereBlobImagem(imagem);

                _condominioRepository.AbreTransacao();
                var condominio = await _condominioRepository.ObterCondominioPorCodigo(codCondominio);
                int proximoNumero = await ObterProximoNumeroRemessaInterna(condominio.Id);
                remessa.CondominioId = condominio.Id;
                remessa.Numero = proximoNumero + DateTime.Now.ToString("ddMMyyyy");
                remessa.CodigoRetirada = remessa.Id.ToString().Substring(0, 8);
                var result = await _remessaInternaRepository.Adicionar(remessa);
                var externa = await _remessaRepository.ObterPorId(remessa.RemessaExternaId);
                externa.QuantidadeLancados += 1;
                if (externa.QuantidadeItens == externa.QuantidadeLancados)
                    externa.Interna = true;
                await _remessaRepository.Atualizar(externa);
                await _syncLogRepository.Adicionar(new SyncLog()
                {
                    CondominioId = remessa.CondominioId,
                    ImovelId = remessa.ImovelId,
                    Entidade = "Remessa",
                    RegistroId = remessa.Id,
                    Operacao = "INSERT"
                });
                _condominioRepository.FechaTransacao();
                return result;
            }
            catch (Exception e)
            {
                _condominioRepository.CancelaTransacao();
                throw e;
            }
        }

        public async Task EnviaEmail(RemessaInterna remessa)
        {
            var pessoa = await _pessoaRepository.ObterPorId(remessa.PessoaId);
            string nome = pessoa.NomeFantasia != null ? pessoa.NomeFantasia : pessoa.Nome;
            var contatoRemessa = await _contatoRepository.ObterContatoPorEntidade(remessa.PessoaId);
            if (_emailSenGridService.ValidaEnderecoEmail(contatoRemessa.Email))
                await _emailSenGridService.EnviaEmail(contatoRemessa.Email, nome, remessa.CodigoRetirada, remessa.Numero);
        }

        public async Task EnviaSMS(RemessaInterna remessa)
        {
            var contatoRemessa = await _contatoRepository.ObterContatoPorEntidade(remessa.PessoaId);
            string numero = contatoRemessa.Ddd.ToString() + contatoRemessa.Telefone;
            string conteudo = "Você recebeu uma nova encomenda n. " + remessa.Numero + " e para retirar use o código: " + remessa.CodigoRetirada;
            var tmp = await _smsService.SendAsync(conteudo, numero);
        }

        private async Task EnviaSMSConfirmacaoRetirada(RemessaInterna remessa, string item, string terceiro, string rg)
        {
            var contatoRemessa = await _contatoRepository.ObterContatoPorEntidade(remessa.PessoaId);
            string numero = contatoRemessa.Ddd.ToString() + contatoRemessa.Telefone;
            StringBuilder sb = new StringBuilder();
            sb.Append("O item: " + item + " da remessa número: " + remessa.Numero + ".");
            if (!terceiro.Equals(string.Empty))
            {
                sb.Append("Retirado por: " + terceiro + " RG.: " + rg + ".");
            }
            sb.Append("Foi retirado.");        
            sb.Append("Data da retirada:" + DateTime.Now.ToString("dd/MM/yyyy HH:mm") + " H.");
            var tmp = await _smsService.SendAsync(sb.ToString(), numero);
        }

        public async Task<RemessaExterna> ObterRemessa(Guid remessa)
        {
            var result = await _remessaRepository.ObterRemessa(remessa);
            return result;
        }

        public async Task<RemessaCompleta> ObterPorNumero(string numero)
        {
            var result = await _remessaInternaRepository.ObterPorNumero(numero);
            if (result != null)
            {
                var completa = new RemessaCompleta
                {
                    CodigoRetirada = result.CodigoRetirada,
                    NomeMorador = result.Pessoa.Nome,
                    NomeConjunto = result.Conjunto.Nome,
                    NumeroRemessa = numero,
                    NomeCondominio = result.Condominio.NomeFantasia,
                    NumeroAndar = result.Andar.NumAndarInterno.ToString(),
                    NumeroImovel = result.Imovel.NumImovel,
                };
                return completa;
            }
            return null;
        }

        private async Task<int> ObterProximoNumeroRemessa(Guid condominio)
        {
            var remessas = await _remessaRepository.Buscar(x => x.CondominioId == condominio && x.DataCadastro.Year == DateTime.Now.Year && DateTime.Now.Month >= x.DataCadastro.Month);
            if (remessas.ToList().Count == 0)
                return 1;

            var ultima = remessas.OrderBy(x=> x.DataCadastro).Last();
            string numero = ultima.Numero.Substring(0, ultima.Numero.Length - 8);
            int incremento = Convert.ToInt32(numero);
            return ++incremento;
        }

        private async Task<int> ObterProximoNumeroRemessaInterna(Guid condominio)
        {
            try
            {
                var remessas = await _remessaInternaRepository.Buscar(x => x.CondominioId == condominio && x.DataCadastro.Year == DateTime.Now.Year && DateTime.Now.Month >= x.DataCadastro.Month);
                if (remessas.ToList().Count == 0)
                    return 1;

                var ultima = remessas.OrderBy(x => x.DataCadastro).Last();
                string numero = ultima.Numero.Substring(0, ultima.Numero.Length - 8);
                int incremento = Convert.ToInt32(numero);
                return ++incremento;
            }
            catch (Exception e)
            {

                throw e;
            }
            
        }

        public async Task<IEnumerable<RemessaComEmpresa>> ObterRemessaPorCodigoCondominio(Guid conjunto, DateTime dataInicio, DateTime dataFim)
        {
            var condominio = await _conjuntoRepository.ObterConjuntoPorId(conjunto);
            var result = await _remessaRepository.ObterRemessasComEntregadora(condominio.CondominioId, dataInicio, dataFim);
            List<RemessaComEmpresa> listaRemessas = new List<RemessaComEmpresa>();
            foreach ( var item in result )
            {
                RemessaComEmpresa remessaComEmpresa = new RemessaComEmpresa()
                {
                    Id = item.Id,
                    CondominioId = item.CondominioId,
                    DataCadastro = item.DataCadastro,
                    EmpresaSimplificadaId = item.EmpresaSimplificadaId,
                    NomeEmpresa = item.EmpresaSimplificada.NomeFantasia,
                    Numero = item.Numero,
                    QuantidadeItens = item.QuantidadeItens,
                };
                listaRemessas.Add(remessaComEmpresa);
            }
            return listaRemessas.OrderByDescending(x => x.DataCadastro).ToList();
        }

        public async Task<IEnumerable<RemessaExterna>> ObterRemessasExternasAbertas(int codigo)//lalala
        {
            List<RemessaExterna> remessas = new List<RemessaExterna>();
            var condominio = await _condominioRepository.ObterCondominioPorCodigo(codigo);
            if (condominio != null)
            {
                var result = await _remessaRepository.Buscar(x =>
                    x.Interna == false &&
                    x.CondominioId == condominio.Id &&
                    x.QuantidadeLancados < x.QuantidadeItens);
                foreach (var item in result)
                    item.QuantidadeItens = item.QuantidadeItens - item.QuantidadeLancados;
                return result.OrderByDescending(x => x.DataCadastro);
            }
            return remessas;
        }

        public async Task<IEnumerable<RemessaInterna>> ObterPorRemessaInternaParametros(Guid conjunto, Guid andar, Guid imovel)
        {

            if (conjunto != null && imovel == Guid.Empty)
            {
                return await _remessaInternaRepository.Buscar(x => x.ConjuntoId == conjunto && x.AndarId == andar);
            }
            if (conjunto != null && imovel != null)
            {
                return await _remessaInternaRepository.Buscar(x => x.ConjuntoId == conjunto && x.AndarId == andar && x.ImovelId == imovel);
            }
            else
            {
                return new List<RemessaInterna>();
            }
        }

        public async Task Testa(Guid remessa)
        {
            var result = await _remessaInternaRepository.ObterPorId(remessa);
            //var saida = await AlterarItemRemessaInterna(remessa, "Genilson", "25.907.002-6");
            //await EnviaSMS(result);
        }

        public async Task<List<MoradorCorrespondencia>> ObterPessoasNome(string nome)
        {
            string inicioBusca = nome.Substring(0,1).ToUpper();
            string complementoBusca = nome.Substring(1, nome.Length - 1);

            List<MoradorCorrespondencia> pessoas = new List<MoradorCorrespondencia>();
            var result = await _pessoaRepository.Buscar(x => x.Nome.Contains(inicioBusca + complementoBusca));
            foreach ( var pessoa in result.ToList())
            {
                pessoas.Add(new MoradorCorrespondencia
                {
                    Id = pessoa.Id,
                    Nome = pessoa.Nome
                });
            }
            return pessoas;
        }

        public async Task<IEnumerable<RemessaInterna>> ObterRemessasPorMorador(Guid morador)
        {
            var result = await _remessaInternaRepository.RemessasPorMorador(morador);
            return result.ToList();
        }

        public async Task<IEnumerable<RemessaCompleta>> ObterRemessasPorMorador2(Guid morador)
        {
            var result = await _remessaInternaRepository.ObterPorMorador(morador);
            List<RemessaCompleta> completas = new List<RemessaCompleta>();
            
            foreach (var item in result.ToList().OrderByDescending(x => x.DataCadastro))
            {
                RemessaCompleta completa = new RemessaCompleta();
                completa.CodigoRetirada = item.CodigoRetirada;
                completa.NumeroRemessa = item.Numero;
                completa.CodigoRetirada = item.CodigoRetirada;
                completa.NomeMorador = item.Pessoa.Nome;
                completa.NomeConjunto = item.Conjunto.Nome;
                completa.NomeCondominio = item.Condominio.NomeFantasia;
                completa.NumeroAndar = item.Andar.NumAndarInterno.ToString();
                completa.NumeroImovel = item.Imovel.NumImovel;
                completas.Add(completa);
            }
            return completas;
        }

        private async Task<string> InsereBlobImagem(IFormFile imagem)
        {
            if (imagem == null || imagem.Length == 0)
                return null;

            if (!imagem.ContentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase))
                return null;

            try
            {
                using var stream = imagem.OpenReadStream();
                var t = await S3Service.UploadAsync(stream, imagem.FileName, imagem.ContentType, true, "remessas");
                return t;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<bool> BaixaRemessa(Guid remessaId, Guid pessoa, Guid imovel, Guid operador, int codCondominio)
        {
            var condominio = await _condominioRepository.ObterCondominioPorCodigo(codCondominio);
            if (condominio != null)
            {
                var result = await _remessaInternaRepository.Buscar(x => x.Id == remessaId && x.ImovelId == imovel && x.CondominioId == condominio.Id);
                if (result != null)
                {
                    var remessa = result.FirstOrDefault();
                    remessa.DataEntrega = DateTime.Now;
                    remessa.OperadorRetiradaId = operador;
                    remessa.PessoaRetiradaId = pessoa;
                    await _remessaInternaRepository.Atualizar(remessa);
                    await _syncLogRepository.Adicionar(new SyncLog()
                    {
                        CondominioId = remessa.CondominioId,
                        ImovelId = remessa.ImovelId,
                        Entidade = "Remessa",
                        RegistroId = remessa.Id,
                        Operacao = "UPDATE"
                    });
                    return true;
                }
            }
            return false;
        }

        public async Task<IEnumerable<RemessaInterna>> ObterInternasPorImovel(int codigo, Guid imovel)
        {
            var condominio = await _condominioRepository.ObterCondominioPorCodigo(codigo);
            if (condominio != null)
            {
                var result = await _remessaInternaRepository.ObterInternasPorImovelComPessoaRetirada(condominio.Id, imovel);
                return result;
            }
            return null;
        }
    }
}
