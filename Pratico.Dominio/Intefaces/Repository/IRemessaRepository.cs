using Pratico.Dominio.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Pratico.Dominio.Intefaces.Repository
{
    public interface IRemessaRepository : IRepository<RemessaExterna>
    {
        Task<IEnumerable<RemessaExterna>> ObterPorConjunto(Guid morador);
        Task<RemessaExterna> ObterRemessa(Guid remessa);
        Task<IEnumerable<RemessaExterna>> ObterRemessasComEntregadora(Guid condominio, DateTime dataInicio, DateTime dataFim);
    }
}
