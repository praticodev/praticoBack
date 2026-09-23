using Amazon.S3.Model;
using Pratico.Dominio.Enums;
using Pratico.Dominio.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Pratico.Dominio.Intefaces.Service
{
    public interface IImovelService
    {
        Task<bool> AdicionarDocumento(DocumentoImovel documento);
        Task<ImovelCondominio> ObterImovelProprietario(Guid id);
        Task<ImovelCondominio> ObterDadosImovel(Guid id);
        Task<ImoveisProprietario> ObterImoveisProprietario(Guid id, Guid condId);
        Task<bool> UpLoad(Stream arquivo, string nome, Guid imovelId, TipoDocumento tipoDoc);
        Task<bool> UpLoadValidado(Stream arquivo, string nome, Guid imovelId, TipoDocumento tipoDoc);
        Task<List<DocumentoImovel>> ObterDocumentosPorImovel(Guid imovelId);
        Task<GetObjectResponse> BaixaArquivoAzure(string nome);
        Task<bool> RemoveArquivoAzure(Guid id, string nome);
        Task<bool> AtualizarValidacao(Guid id);
        Task<bool> AdicionarProprietarioImovelValidacao(Guid imovelId, Guid proprietarioId);
        Task<int> ContaDocsValidados(Guid idImovel);
        Task<Guid> ObterImovelPorMorador(Guid morador);
    }
}
