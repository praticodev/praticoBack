using Pratico.Dominio.Intefaces;
using Pratico.Dominio.Intefaces.Repository;
using Pratico.Dominio.Intefaces.Service;
using Pratico.Dominio.Model;
using Pratico.Dominio.Validations;
using System;
using System.Threading.Tasks;
using System.Linq;
using System.Globalization;

namespace Pratico.Business.Services
{
    public class CondominioService : BaseService, ICondominioService
    {
        private readonly ICondominioRepository _condominioRepository;
        private readonly IEnderecoRepository _enderecoRepository;
        private readonly IContatoRepository _contatoRepository;
        private readonly IRepresentanteLegalRepository _representanteRepository;
        CondominioCompleto condominioCompleto = null;

        public CondominioService(ICondominioRepository condominioRepository,
                                IEnderecoRepository enderecoRepository,
                                IContatoRepository contatoRepository,
                                IRepresentanteLegalRepository representanteRepository,
                                INotificador notificador) : base(notificador)
        {
            _condominioRepository = condominioRepository;
            _enderecoRepository = enderecoRepository;
            _contatoRepository = contatoRepository;
            _representanteRepository = representanteRepository;
        }

        public async Task<Condominio> Adicionar(Condominio condominio)
        {
            if (!ExecutarValidacao(new CondominioValidation(), condominio)) 
                return null;

            condominio.Cnpj = condominio.Cnpj.Replace(".", "").Replace("-", "").Replace("/", "");
            var existe = await ObterCondominioPorCnpj(condominio.Cnpj);
            if (existe != null)
            {
                Notificar("Já existe um condomínio cadastrado com este cnpj infomado.");
                return null;
            }

            condominio.CodCondominio = new Random().Next(0, 100000);
            return await _condominioRepository.Adicionar(condominio);
        }
        public async Task<CondominioCompleto> ObterCondominioCompleto(int codigo)
        {
            var result = await _condominioRepository.ObterCondominioCompleto(codigo);

            if (result != null)
            {
                condominioCompleto = new CondominioCompleto()
                {
                    Area = result.Area,
                    Ativo = result.Ativo,
                    Cnpj = result.Cnpj,
                    DataCadastro = result.DataCadastro,
                    Finalidade = result.Finalidade,
                    DiaVencimento = result.DiaVencimento,
                    Fracao = result.Fracao,
                    Id = result.Id,
                    InscEstadual = result.InscEstadual,
                    InscMunicipal = result.InscMunicipal,
                    NomeFantasia = result.NomeFantasia,
                    RazaoSocial = result.RazaoSocial,
                    TipoCondominio = result.TipoCondominio,
                    UsuarioId = result.UsuarioId
                };

                condominioCompleto.Contato = await ObterContatoPorEntidade(result.Id);
                condominioCompleto.Endereco = await ObterEnderecoPorCondominio(result.Id);
                condominioCompleto.RepresentanteLegal = await ObterRepresentantePorCondominio(result.Id);

                result.Conjuntos = result.Conjuntos.OrderBy(x => x.DataCadastro).ToList();
                foreach (var item in result.Conjuntos)
                {
                    item.Andares = item.Andares.OrderBy(x => x.NumAndarInterno).ToList();
                    foreach (var item2 in item.Andares)
                        item2.Imoveis = item2.Imoveis.OrderBy(x => x.NumImovelinterno);
                }

                condominioCompleto.Conjuntos = result.Conjuntos;
            }
            return condominioCompleto;
        }

        public async Task<Condominio> ObterCondominioPorCnpj(string cnpj)
        {
            return await _condominioRepository.ObterCondominioPorCnpj(cnpj);
        }

        public async Task<bool> Atualizar(Condominio condominio)
        {
            if (!ExecutarValidacao(new CondominioValidation(), condominio)) return false;
            await _condominioRepository.Atualizar(condominio);
            return true;
        }

        private async Task<Contato> ObterContatoPorEntidade(Guid id)
        {
            return await _contatoRepository.ObterContatoPorEntidade(id);
        }

        private async Task<Endereco> ObterEnderecoPorCondominio(Guid id)
        {
            return await _enderecoRepository.ObterEnderecoPorCondominio(id);
        }

        private async Task<RepresentanteLegal> ObterRepresentantePorCondominio(Guid id)
        {
            return await _representanteRepository.ObterRepresentantePorCondominio(id);
        }

        public void Dispose()
        {
            _condominioRepository?.Dispose();
        }
    }
}