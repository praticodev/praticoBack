using Microsoft.EntityFrameworkCore;
using Pratico.Data.Context;
using Pratico.Dominio.Intefaces.Repository;
using Pratico.Dominio.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pratico.Data.Repository
{
    public class AcessoVisitanteRepository : Repository<AcessoVisitante>, IAcessoVisitanteRepository
    {
        public AcessoVisitanteRepository(PraticoContext context) : base(context) { }

        public async Task<IEnumerable<AcessoVisitante>> ObterVisitantesPorImovel(Guid imovel, DateTime data)
        {
            return await Db.AcessoVisitante.AsNoTracking()
                            .Include(c => c.Pessoa)
                            .Where(x => x.ImovelId == imovel && x.DataCadastro <= data.AddHours(23).AddMinutes(59))
                            .ToListAsync();
        }
    }
}
