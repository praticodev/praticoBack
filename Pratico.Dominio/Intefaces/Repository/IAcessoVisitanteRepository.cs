using Pratico.Dominio.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Pratico.Dominio.Intefaces.Repository
{
    public interface IAcessoVisitanteRepository : IRepository<AcessoVisitante>
    {
        Task<IEnumerable<AcessoVisitante>> ObterVisitantesPorImovel(Guid imovel, DateTime data);
    }
}
