using Pratico.Dominio.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Pratico.Dominio.Intefaces.Repository
{
    public interface IRelatorioCorrespondenciaRepository : IRepository<RelatorioCorrespondencia>
    {
        Task<IEnumerable<RemessaExterna>> ObterRemessasExternasAbertasRelatorio(Guid condominio, DateTime? dataInicio, DateTime? dataFim, Guid? operador);
        Task<IEnumerable<RemessaExterna>> ObterRemessasExternasFechadasRelatorio(Guid condominio, DateTime? dataInicio, DateTime? dataFim, Guid? operador);
    }
}
