using Pratico.Dominio.Enums;
using Pratico.Dominio.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Pratico.Dominio.Intefaces.Service
{
    public interface IProprietarioService : IDisposable
    {
        Task<Proprietario> Adicionar(Proprietario proprietario, Endereco endereco, Guid imovelId);
        Task<bool> UpLoad(Stream arquivo, string nome);
        Task<DocumentoImovel> Adicionar(DocumentoImovel documento);
        Task<ProprietarioEndereco> ObterProprietarioEndereco(Guid id);
    }
}
