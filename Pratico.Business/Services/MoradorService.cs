using Amazon.S3.Model;
using Microsoft.AspNetCore.Http;
using Pratico.Business.Utils;
using Pratico.Dominio.Enums;
using Pratico.Dominio.Intefaces;
using Pratico.Dominio.Intefaces.Repository;
using Pratico.Dominio.Intefaces.Service;
using Pratico.Dominio.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Pratico.Business.Services
{
    public class MoradorService : BaseService, IMoradorService
    {
        private readonly IMoradorImovelRepository _moradorImovelRepository;
        private readonly IPessoaRepository _pessoaRepository;
        private readonly IContatoRepository _contatoRepository;
        private readonly ICondominioRepository _condominioRepository;
        private readonly IPessoaUsuarioRepository _pessoaUsuarioRepository;
        private readonly IUsuarioCondominioRepository _usuarioCondominioRepository;
        private readonly ISyncLogRepository _syncLogRepository;

        public MoradorService(IMoradorImovelRepository moradorImovelRepository,
                              IPessoaRepository pessoaRepository,
                              IContatoRepository contatoRepository,
                              ICondominioRepository condominioRepository,
                              IPessoaUsuarioRepository pessoaUsuarioRepository,
                              IUsuarioCondominioRepository usuarioCondominioRepository,
                              ISyncLogRepository syncLogRepository,
                              INotificador notificador) : base(notificador)
        {
            _moradorImovelRepository = moradorImovelRepository;
            _pessoaRepository = pessoaRepository;
            _contatoRepository = contatoRepository;
            _condominioRepository = condominioRepository;
            _pessoaUsuarioRepository = pessoaUsuarioRepository;
            _usuarioCondominioRepository = usuarioCondominioRepository;
            _syncLogRepository = syncLogRepository;
        }

        public async Task<bool> Adicionar(Morador morador)
        {
            try
            {
                await _pessoaRepository.Adicionar(morador);
                MoradorImovel novo = new MoradorImovel
                {
                    ImovelId = morador.ImovelId,
                    PessoaId = morador.Id
                };
                var result = await _moradorImovelRepository.Adicionar(novo);
                await _syncLogRepository.Adicionar(new SyncLog()
                {
                    CondominioId = morador.CondominioId,
                    ImovelId = morador.ImovelId,
                    Entidade = "Morador",
                    RegistroId = morador.Id,
                    Operacao = "INSERT"
                });
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<Pessoa> AdicionarApp(Morador morador, IFormFile? imagem)
        {
            try
            {
                if (imagem != null)
                {
                    var imagemNuvem = await InsereBlobImagem(imagem, morador.Id);
                    morador.Imagem = imagemNuvem;
                }
                var pessoa = await _pessoaRepository.Adicionar(morador);
                MoradorImovel novo = new MoradorImovel
                {
                    ImovelId = morador.ImovelId,
                    PessoaId = morador.Id,
                    CondominioId = morador.CondominioId
                };
                var result = await _moradorImovelRepository.Adicionar(novo);
                await _syncLogRepository.Adicionar(new SyncLog()
                {
                    CondominioId = morador.CondominioId,
                    ImovelId = morador.ImovelId,
                    Entidade = "Morador",
                    RegistroId = morador.Id,
                    Operacao = "INSERT"
                });
                return pessoa;
            }
            catch (Exception e)
            {
                await RemoveBlob(morador.Id);
                return null;
            }
        }

        public async Task<bool> Atualizar(Morador morador, IFormFile? imagem)
        {
            try
            {                
                var moradorBase = await _pessoaRepository.Buscar(x => x.Id == morador.Id && x.CondominioId == morador.CondominioId);
                if (moradorBase.Any())
                {
                    if(!String.IsNullOrEmpty(moradorBase.FirstOrDefault().Imagem) && imagem != null)
                        await S3Service.DeleteIfExistsAsync(moradorBase.FirstOrDefault().Imagem, "moradores");
                    if (imagem != null)
                    {
                        await S3Service.DeleteIfExistsAsync(imagem.FileName, "moradores");
                        await InsereBlobImagem(imagem, morador.Id);
                    }
                    var moradorAtualizar = moradorBase.FirstOrDefault();
                    moradorAtualizar.Cnh = morador.Cnh;
                    moradorAtualizar.Cpf = morador.Cpf;
                    moradorAtualizar.Cnpj = morador.Cnpj;
                    moradorAtualizar.DataNascimento = morador.DataNascimento;
                    moradorAtualizar.EstadoCivil = morador.EstadoCivil;
                    moradorAtualizar.Imagem = imagem != null ? imagem.FileName : moradorBase.FirstOrDefault().Imagem;
                    moradorAtualizar.InscEstadual = morador.InscEstadual;
                    moradorAtualizar.InscMunicipal = morador.InscMunicipal;
                    moradorAtualizar.Nome = morador.Nome;
                    moradorAtualizar.NomeFantasia = morador.NomeFantasia;
                    moradorAtualizar.Rg = morador.Rg;
                    moradorAtualizar.Sexo = morador.Sexo;

                    await _pessoaRepository.Atualizar(moradorAtualizar);
                    var moradorImovel = (await _moradorImovelRepository.Buscar(x =>
                        x.PessoaId == moradorAtualizar.Id &&
                        x.CondominioId == morador.CondominioId)).FirstOrDefault();

                    await _syncLogRepository.Adicionar(new SyncLog()
                    {
                        CondominioId = morador.CondominioId,
                        ImovelId = morador.ImovelId != Guid.Empty ? morador.ImovelId : moradorImovel?.ImovelId ?? Guid.Empty,
                        Entidade = "Morador",
                        RegistroId = moradorAtualizar.Id,
                        Operacao = "UPDATE"
                    });
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                await S3Service.DeleteIfExistsAsync(imagem.FileName, "moradores");
                return false;
            }
        }

        public async Task<string> Remover(Guid id, int codCondominio)
        {
            try
            {
                var condominio = await _condominioRepository.ObterCondominioPorCodigo(codCondominio);

                if (condominio != null)
                {
                    var morador = await _pessoaRepository.Buscar(x => x.Id == id && x.CondominioId == condominio.Id);
                    if (morador.Any())
                    {
                        string result = String.Empty;

                        if (!String.IsNullOrEmpty(morador.FirstOrDefault().Imagem))
                            await S3Service.DeleteIfExistsAsync(morador.FirstOrDefault().Imagem, "moradores");

                        var pessoaUsuario = await _pessoaUsuarioRepository.Buscar(x => x.PessoaId == id && x.CondominioId == condominio.Id);                   

                        if (pessoaUsuario.Any())
                        {
                            result = pessoaUsuario.FirstOrDefault().UsuarioId.ToString();
                            var usuarioCondominio = await _usuarioCondominioRepository.Buscar(x => x.UsuarioId == pessoaUsuario.FirstOrDefault().UsuarioId && x.CondominioId == condominio.Id);
                            if (usuarioCondominio.Any())
                                await _usuarioCondominioRepository.RemoverTodos(usuarioCondominio);
                            await _pessoaUsuarioRepository.RemoverTodos(pessoaUsuario);

                        }

                        var moradorImovel = (await _moradorImovelRepository.Buscar(x =>
                            x.PessoaId == id &&
                            x.CondominioId == condominio.Id)).FirstOrDefault();

                        await _pessoaRepository.Remover(morador.FirstOrDefault().Id);
                        await _syncLogRepository.Adicionar(new SyncLog()
                        {
                            CondominioId = condominio.Id,
                            ImovelId = moradorImovel?.ImovelId ?? Guid.Empty,
                            Entidade = "Morador",
                            RegistroId = morador.FirstOrDefault().Id,
                            Operacao = "DELETE"
                        });
                        return result;
                    }
                }
                return String.Empty;
            }
            catch (Exception e)
            {
                return String.Empty;
            }
        }

        public async Task<Morador> GeraMorador(Guid imovelId, string nome, string cpf, string numDoc, DateTime dataNascimento, TipoEstadoCivil estadoCivil, TipoSexo sexo, TipoPessoa tipoPessoa, TipoDocumento tipoDocumento, int condominioId)
        {
            var condominio = await _condominioRepository.ObterCondominioPorCodigo(condominioId);
            Morador morador = new Morador
            {
                Ativo = true,
                Cpf = cpf,
                DataNascimento = DateTime.SpecifyKind(dataNascimento.Date, DateTimeKind.Unspecified),
                TipoDocumento = tipoDocumento,
                TipoPessoa = tipoPessoa,
                EstadoCivil = estadoCivil,
                ImovelId = imovelId,
                Nome = nome,
                Sexo = sexo,
                CondominioId = condominio.Id,
                Rg = numDoc
            };

            switch (tipoDocumento)
            {
                case TipoDocumento.Rg:
                case TipoDocumento.Rne:
                    morador.Rg = numDoc;
                    break;
                case TipoDocumento.Cnh:
                    morador.Cnh = numDoc;
                    break;
                default:
                    break;
            }

            return morador;

        }

        public async Task<IEnumerable<Morador>> ObterMoradoresPorImovel(Guid imovel, int codCondominio)
        {
            var condominio = await _condominioRepository.ObterCondominioPorCodigo(codCondominio);
            if (condominio != null)
            {
                List<Morador> moradoresLista = new List<Morador>();
                IEnumerable<MoradorImovel> moradoresId = await _moradorImovelRepository.Buscar(x => x.ImovelId == imovel && x.CondominioId == condominio.Id);
                List<Guid> listaIds = new List<Guid>();
                moradoresId.ToList().ForEach(x =>
                    listaIds.Add(x.PessoaId)
                );
                var moradores = await _pessoaRepository.Buscar(x => listaIds.Contains(x.Id));
                var contatos = await _contatoRepository.Buscar(x => listaIds.Contains(x.Entidade) && x.CondominioId == condominio.Id);
                var contatoPorPessoa = contatos
                    .GroupBy(x => x.Entidade)
                    .ToDictionary(x => x.Key, x => x.OrderByDescending(c => c.Principal).FirstOrDefault());

                moradores.ToList().ForEach(x =>
                {
                    var contato = contatoPorPessoa.ContainsKey(x.Id) ? contatoPorPessoa[x.Id] : null;
                        moradoresLista.Add(new Morador
                        {
                            Ativo = true,
                            Nome = x.Nome,
                            NomeFantasia = x.NomeFantasia,
                            Id = x.Id,
                            ImovelId = imovel,
                            Email = contato?.Email,
                            Telefone = contato != null ? $"({contato.Ddd.ToString()}){contato?.Telefone}" : null,
                            Sexo = x.Sexo,
                            Cpf = x.Cpf,
                            Rg = x.Rg,
                            EstadoCivil = x.EstadoCivil,
                            Imagem = x.Imagem,
                        }
                    );
                });
                return moradoresLista.OrderBy(x=> x.Nome);
            }
            return null;
        }

        private async Task<string> InsereBlobImagem(IFormFile imagem, Guid idMorador)
        {
            if (imagem == null || imagem.Length == 0 || idMorador == Guid.Empty)
                return null;

            if (!imagem.ContentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase))
                return null;

            try
            {
                using var stream = imagem.OpenReadStream();
                var t = await S3Service.UploadAsync(stream, imagem.FileName, imagem.ContentType, true, "moradores");
                return t;
            }
            catch (Exception e)
            {
                return  null;
            }
        }

        private async Task<bool> RemoveBlob(Guid idMorador)
        {
            if (idMorador == Guid.Empty)
                return false;

            try
            {
                var objectKey = await ResolveMoradorImageObjectKeyAsync(idMorador);

                if (objectKey == null)
                    return false;

                return await S3Service.DeleteIfExistsAsync(objectKey, "moradores");
            }
            catch (Exception e)
            {
                return false;
            }
        }

        private static async Task<string> ResolveMoradorImageObjectKeyAsync(Guid idMorador)
        {
            foreach (var objectKey in GetMoradorImageObjectKeyCandidates(idMorador))
            {
                if (await S3Service.ExistsAsync(objectKey, "moradores"))
                    return objectKey;
            }

            return null;
        }

        private static IEnumerable<string> GetMoradorImageObjectKeyCandidates(Guid idMorador)
        {
            var baseKey = idMorador.ToString();

            yield return baseKey;
            yield return $"{baseKey}.jpg";
            yield return $"{baseKey}.jpeg";
            yield return $"{baseKey}.png";
            yield return $"{baseKey}.gif";
            yield return $"{baseKey}.webp";
            yield return $"{baseKey}.bmp";
        }

        public void Dispose()
        {
            _moradorImovelRepository.Dispose();
            _pessoaRepository.Dispose();
        }

        public async Task<int> ObterQuantidadeMoradoresPorImovel(Guid imovelId, int codCOndominio)
        {
            var condominio = await _condominioRepository.ObterCondominioPorCodigo(codCOndominio);
            if (condominio == null)
                return 0;

            var quantidade = await _moradorImovelRepository.Buscar(x => x.ImovelId == imovelId && x.CondominioId == condominio.Id);
            return quantidade.ToList().Count();
        }
    }
}
