using Pratico.Dominio.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Pratico.Dominio.Intefaces.Service
{
    public interface IInquilinoService : IDisposable
    {
        Task<Inquilino> Adicionar(Inquilino inquilino, Endereco endereco, Guid imovelId);
        Task<ProprietarioEndereco> ObterInquilinoEndereco(Guid id);
    }
}
