using Pratico.Dominio.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pratico.Dominio.Intefaces.Repository
{
    public interface IConjuntoRepository : IRepository<Conjunto>
    {
        Task<Conjunto> ObterConjuntoPorId(Guid id);
        Task<IEnumerable<Conjunto>> ObterTodosPorId(Guid id);
        Task<int> ContaTotalConjuntos(Guid id);
    }
}
