using Pratico.Dominio.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Pratico.Dominio.Intefaces.Service
{
    public interface IProprietarioImovelService
    {
        Task<bool> ObterProprietarioValidado(Guid id);
        Task<Guid> ObterImovelValidado(Guid id);
        Task<Guid> ObterImovelNaoValidado(Guid id);
    }
}
