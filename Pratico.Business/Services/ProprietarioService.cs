using Amazon;
using Amazon.S3;
using Amazon.S3.Transfer;
using Microsoft.Extensions.Configuration;
using Pratico.Business.Configuration;
using Pratico.Dominio.Enums;
using Pratico.Dominio.Intefaces;
using Pratico.Dominio.Intefaces.Repository;
using Pratico.Dominio.Intefaces.Service;
using Pratico.Dominio.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Pratico.Business.Services
{
    public class ProprietarioService : BaseService, IProprietarioService
    {
        private readonly IProprietarioRepository _proprietarioRepository;
        private readonly IEnderecoRepository _enderecoRepository;
        private readonly IImovelRepository _imovelRepository;
        private readonly IProprietarioImovelRepository _proprietarioImovelRepository;
        private readonly IConfiguration _configuration;
        private readonly IDocumentoImovelRepository _documentoImovelRepository;
        private readonly IPessoaRepository _pessoaRepository;
        public ProprietarioService(IProprietarioRepository proprietarioRepository,
                                    IEnderecoRepository enderecoRepository,
                                    IImovelRepository imovelRepository,
                                    IProprietarioImovelRepository proprietarioImovelRepository,
                                    INotificador notificador,
                                    IConfiguration configuration,
                                    IPessoaRepository pessoaRepository,
                                    IDocumentoImovelRepository documentoImovelRepository) : base(notificador)
        {
            _proprietarioRepository = proprietarioRepository;
            _enderecoRepository = enderecoRepository;
            _imovelRepository = imovelRepository;
            _proprietarioImovelRepository = proprietarioImovelRepository;
            _configuration = configuration;
            _documentoImovelRepository = documentoImovelRepository;
            _pessoaRepository = pessoaRepository;
        }

        public async Task<Proprietario> Adicionar(Proprietario proprietario, Endereco endereco, Guid imovelId)
        {
            if (proprietario.Cnpj != null && proprietario.Cnpj.Length > 10)
            {
                proprietario.Cnpj.Replace(".", "").Replace("-", "").Replace("/", "");
                proprietario.TipoPessoa = TipoPessoa.Juridica;
            }
            else
            {
                proprietario.Cpf = proprietario.Cpf.Replace(".", "").Replace("-", "");
                proprietario.TipoPessoa = TipoPessoa.Fisica;
            }
            proprietario = await _proprietarioRepository.Adicionar(proprietario);
            var imovel = await _imovelRepository.ObterPorId(imovelId);
            if (imovel != null)
            {
                imovel.Proprietario = proprietario.Id;
                await _imovelRepository.Atualizar(imovel);
            }
            endereco.Entidade = proprietario.Id;
            await _enderecoRepository.Adicionar(endereco);
            ProprietarioImovel propImovel = new ProprietarioImovel
            {
                ImovelId = imovelId,
                PessoaId = proprietario.Id,
                Validado = true
            };
            await _proprietarioImovelRepository.Adicionar(propImovel);
            return proprietario;
        }

        public async Task<bool> UpLoad(Stream arquivo, string nome)
        {
            var result = false;
            var appSettingsSection = _configuration.GetSection("S3");
            var s3Settings = appSettingsSection.Get<S3Configuration>();
            RegionEndpoint endPoint = RegionEndpoint.SAEast1;

            using (var client = new AmazonS3Client(s3Settings.Usuario, s3Settings.Senha, endPoint))
            {
                var transfer = new TransferUtility(client);
                MemoryStream ms = new MemoryStream();
                await arquivo.CopyToAsync(ms);
                var nomeArquivo = nome.Replace(" ","");
                await transfer.UploadAsync(ms, s3Settings.Balde, nomeArquivo);
                result = true;
            }

            return result;
        }

        public async Task<DocumentoImovel> Adicionar(DocumentoImovel documento)
        {
            return await _documentoImovelRepository.Adicionar(documento);
        }



        private async Task<Endereco> ObterEnderecoProprietario(Guid id)
        {
            return await _enderecoRepository.ObterPorId(id);
        }

        private async Task<Proprietario> ObterProprietario(Guid id)
        {
            return await _proprietarioRepository.ObterPorId(id);
        }

        public async Task<ProprietarioEndereco> ObterProprietarioEndereco(Guid id)
        {
            ProprietarioEndereco propEndereco = null;
            try
            {
                var imovel = await _imovelRepository.ObterPorId(id);
                if (imovel != null)
                {
                    var pessoa = imovel.Proprietario.HasValue ? await _pessoaRepository.ObterPorId(imovel.Proprietario.Value) : null;
                    var endereco = pessoa != null ? await _enderecoRepository.ObterEnderecoPorCondominio(pessoa.Id) : null;
                    if (pessoa != null && endereco != null)
                    {
                        propEndereco = new ProprietarioEndereco
                        {
                            //AssinaDoc = pessoa
                            //Cargo =
                            Cnpj = pessoa.Cnpj,
                            Cpf = pessoa.Cpf,
                            DataNascimento = pessoa.DataNascimento.ToString("s"),//"1979-07-20T00:00:00"
                            EstadoCivil = (int)pessoa.EstadoCivil,
                            //FimPeriodo =
                            Id = pessoa.Id,
                            ImovelId = imovel.Id,
                            InscEstadual = pessoa.InscEstadual,
                            InscMunicipal = pessoa.InscMunicipal,
                            Nome = pessoa.Nome,
                            NomeFantasia = pessoa.NomeFantasia,
                            //NumDoc =
                            RazaoSocial = pessoa.Nome,
                            Sexo = (int)pessoa.Sexo,
                            //TipoDocumento = 
                            //FimPeriodo =
                            //IniPeriodo =
                            Bairro = endereco.Bairro,
                            Cep = endereco.Cep,
                            Cidade = endereco.Cidade,
                            Cobranca = endereco.Cobranca,
                            Complemento = endereco.Complemento,
                            Logradouro = endereco.Logradouro,
                            Entidade = endereco.Entidade,
                            Numero = endereco.Numero,
                            Referencia = endereco.Referencia,
                            UF = endereco.UF
                        };
                    }
                }
            }
            catch (Exception e)
            {

                throw e;
            }
            
            return propEndereco;
        }

        public void Dispose()
        {
            _proprietarioRepository?.Dispose();
            _enderecoRepository?.Dispose();
            _imovelRepository?.Dispose();
        }
    }
}
