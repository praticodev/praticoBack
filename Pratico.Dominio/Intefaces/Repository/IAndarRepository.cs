using Pratico.Dominio.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pratico.Dominio.Intefaces.Repository
{
    public interface IAndarRepository : IRepository<Andar>
    {
        Task<Andar> ObterAndarPorId(Guid id);
        Task<IEnumerable<Andar>> ObterTodosPorId(Guid id);
    }
}
