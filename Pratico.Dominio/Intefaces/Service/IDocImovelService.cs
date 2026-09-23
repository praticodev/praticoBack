using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Pratico.Dominio.Intefaces.Service
{
    public interface IDocImovelService
    {
        Task<bool> Atualizar(Guid id, int etapa);
    }
}
