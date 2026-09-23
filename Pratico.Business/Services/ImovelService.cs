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
using Pratico.Dominio.Validations;
using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Amazon.S3.Model;
using Amazon.Runtime.Internal;
using Azure.Storage.Blobs;

namespace Pratico.Business.Services
{
    public class ImovelService : BaseService, IImovelService
    {
        private readonly IDocumentoImovelRepository _docImovelRepository;
        private readonly IImovelRepository _imovelRepository;
        private readonly IConfiguration _configuration;
        private readonly IDocumentoImovelRepository _documentoImovelRepository;
        private readonly IAmazonS3 _amazonS3;
        private readonly IPessoaRepository _pessoaRepository;
        private readonly IProprietarioImovelRepository _proprietarioImovelRepository;
        private readonly IMoradorImovelRepository _moradorImovelRepository;

        private ImoveisProprietario imoveis = null;
        public ImovelService(IDocumentoImovelRepository docImovelRepository,
                             IImovelRepository imovelRepository,
                             INotificador notificador,
                             IConfiguration configuration,
                             IDocumentoImovelRepository documentoImovelRepository,
                             IAmazonS3 amazonS3,
                             IProprietarioImovelRepository proprietarioImovelRepository,
                             IMoradorImovelRepository moradorImovelRepository,
                             IPessoaRepository pessoaRepository) : base(notificador)
        {
            _docImovelRepository = docImovelRepository;
            _imovelRepository = imovelRepository;
            _configuration = configuration;
            _documentoImovelRepository = documentoImovelRepository;
            _amazonS3 = amazonS3;
            _pessoaRepository = pessoaRepository;
            _proprietarioImovelRepository = proprietarioImovelRepository;
            _moradorImovelRepository = moradorImovelRepository;
        }

        public async Task<bool> AdicionarDocumento(DocumentoImovel documento)
        {
            if (!ExecutarValidacao(new DocumentoImovelValidation(), documento))
                return false;

            await _docImovelRepository.Adicionar(documento);
            return true;
        }

        public async Task<ImoveisProprietario> ObterImoveisProprietario(Guid id, Guid condId)
        {
            var result = await _imovelRepository.ImoveisProprietario(id, condId);
            if (result != null)
            {
                imoveis = new ImoveisProprietario();
                foreach (var item in result)
                {
                    imoveis.Imoveis.Add(item);
                }
                imoveis.NomeCondominio = result[0].Andar.Conjunto.Condominio.NomeFantasia;
            }
            return imoveis;
        }

        public async Task<ImovelCondominio> ObterImovelProprietario(Guid id)
        {
            var result = await _imovelRepository.ImovelCondominio(id);
            if (result != null)
            {
                var imovel = new ImovelCondominio
                {
                    Andar = result.NumAndar,
                    Conjunto = result.Andar.Conjunto.Nome,
                    Imovel = result,
                    Id = result.Id,
                    NomeCondominio = result.Andar.Conjunto.Condominio.NomeFantasia
                };
                if (result.Proprietario.HasValue)
                {
                    var propietario = await _pessoaRepository.ObterPorId(result.Proprietario.Value);
                    if (propietario != null)
                        imovel.NomeProprietario = propietario.Nome;
                }

                return imovel;
            }
            return null;
        }

        public async Task<bool> UpLoad(Stream arquivo, string nome, Guid imovelId, TipoDocumento tipoDoc)
        {
            var result = false;
            try
            {
                DocumentoImovel novoDoc = new DocumentoImovel
                {
                    ImovelId = imovelId,
                    NomeDoc = nome,
                    TipoDoc = tipoDoc,
                    Validado = false
                };
                string extensao = nome.Substring(nome.IndexOf('.'), nome.Length - nome.IndexOf('.'));
                var appSettingsSection = _configuration.GetSection("ContainerAzure");
                var azSettings = appSettingsSection.Get<AzureStorageAccount>();
                var blob = new BlobClient(azSettings.ConectionString, azSettings.NomeContainer, novoDoc.Id.ToString() + extensao);
                using (var ms = new MemoryStream(GeraArrayBytes(arquivo)))
                {
                    blob.Upload(ms);
                }

                novoDoc.Url = blob.Uri.AbsoluteUri;
                var doc = await _documentoImovelRepository.Adicionar(novoDoc);

                if (doc != null)
                    result = true;

            }
            catch (Exception e)
            {

                throw e;
            }
            finally
            {
                arquivo.Close();
            }

            return result;
        }

        public async Task<bool> UpLoadValidado(Stream arquivo, string nome, Guid imovelId, TipoDocumento tipoDoc)
        {
            var result = false;
            try
            {
                DocumentoImovel novoDoc = new DocumentoImovel
                {
                    ImovelId = imovelId,
                    NomeDoc = nome,
                    TipoDoc = tipoDoc,
                    Validado = true
                };

                string extensao = nome.Substring(nome.IndexOf('.'), nome.Length - nome.IndexOf('.'));
                var appSettingsSection = _configuration.GetSection("ContainerAzure");
                var azSettings = appSettingsSection.Get<AzureStorageAccount>();
                var blob = new BlobClient(azSettings.ConectionString, azSettings.NomeContainer, novoDoc.Id.ToString() + extensao);
                using (var ms = new MemoryStream(GeraArrayBytes(arquivo)))
                {
                    blob.Upload(ms);             
                }                

                novoDoc.Url = blob.Uri.AbsoluteUri;
                var doc = await _documentoImovelRepository.Adicionar(novoDoc);

                if (doc != null)
                    result = true;

            }
            catch (Exception e)
            {

                throw e;
            }
            finally
            {
                arquivo.Close();
            }

            return result;
        }

        public async Task<List<DocumentoImovel>> ObterDocumentosPorImovel(Guid imovelId)
        {
            var list = await _documentoImovelRepository.Buscar(x => x.ImovelId == imovelId);
            return list.ToList();
        }

        public async Task<GetObjectResponse> BaixaArquivoAzure(string nome)
        {
            var appSettingsSection = _configuration.GetSection("S3");
            var s3Settings = appSettingsSection.Get<S3Configuration>();
            RegionEndpoint endPoint = RegionEndpoint.SAEast1;

            var request = new GetObjectRequest
            {
                Key = nome,
                BucketName = s3Settings.Balde
            };

            GetObjectResponse response = await _amazonS3.GetObjectAsync(request);
            return response;
        }

        public async Task<bool> AtualizarValidacao(Guid id)
        {
            var existente = await _documentoImovelRepository.ObterPorId(id);
            if (existente != null)
                existente.Validado = true;
            try
            {
                await _documentoImovelRepository.Atualizar(existente);
                int totalDocsValidados = await ContaDocsValidados(existente.ImovelId);
                if (totalDocsValidados > 5)
                {
                    var result = await _proprietarioImovelRepository.Buscar(x => x.ImovelId == existente.ImovelId);
                    foreach (var doc in result)
                    {
                        doc.Validado = true;
                        await _proprietarioImovelRepository.Atualizar(doc);
                    }
                }
                    
                return true;
            }
            catch (Exception e)
            {
                throw new Exception("Erro: " + e.Message);
            }
        }

        public async Task<bool> RemoveArquivoAzure(Guid id, string nome)
        {
            try
            {
                string extensao = nome.Substring(nome.IndexOf('.'), nome.Length - nome.IndexOf('.'));
                var appSettingsSection = _configuration.GetSection("ContainerAzure");
                var azSettings = appSettingsSection.Get<AzureStorageAccount>();
                var blob = new BlobClient(azSettings.ConectionString, azSettings.NomeContainer, id.ToString() + extensao);
                blob.Delete();
                await _documentoImovelRepository.Remover(id);
                return true;
            }
            catch (Exception e)
            {

                return false;
            }
        }

        public async Task<int> ContaDocsValidados(Guid idImovel) 
        {
            var total = await _documentoImovelRepository.Buscar(x => x.Validado == true && x.ImovelId == idImovel);
            return total.Count();
        }

        public byte[] GeraArrayBytes(Stream stream)
        {
            byte[] result = null;
            using (MemoryStream ms = new MemoryStream())
            {
                stream.CopyTo(ms);
                result = ms.ToArray();
            }
            return result;
        }

        public async Task<bool> AdicionarProprietarioImovelValidacao(Guid imovelId, Guid proprietarioId)
        {
            try
            {
                ProprietarioImovel propImovel = new ProprietarioImovel
                {
                    ImovelId = imovelId,
                    PessoaId = proprietarioId,
                    Validado = false
                };
                var result = await _proprietarioImovelRepository.Adicionar(propImovel);
                if (result != null)
                    return true;
                else
                    return false;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<ImovelCondominio> ObterDadosImovel(Guid id)
        {
            var imovel = await ObterImovelProprietario(id);
            if (imovel != null)
            {
                var moradores = await _moradorImovelRepository.Buscar(x => x.ImovelId == id);
                foreach (var item in moradores)
                {
                    var pessoa = await _pessoaRepository.ObterPorId(item.PessoaId);
                    if (pessoa != null)
                        imovel.Moradores.Add(pessoa.Nome);
                }
            }
            return imovel;
        }

        public async Task<Guid> ObterImovelPorMorador(Guid morador)
        {
            var result = await _moradorImovelRepository.Buscar(x => x.PessoaId == morador);
            foreach (var item in result)
                return item.ImovelId;
            return Guid.Empty;
        }
    }
}
