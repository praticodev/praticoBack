using Microsoft.EntityFrameworkCore;
using Pratico.Data.Context;
using Pratico.Dominio.Intefaces.Repository;
using Pratico.Dominio.Model;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Pratico.Data.Repository
{
    public class RemessaRepository : Repository<RemessaExterna>, IRemessaRepository
    {
        public RemessaRepository(PraticoContext context) : base(context) { }

        public async Task<IEnumerable<RemessaExterna>> ObterPorConjunto(Guid morador)
        {
            //verificar como ficará a busca por critérios
            return null;
        }

        public async Task<RemessaExterna> ObterRemessa(Guid remessa)
        {
            return await Db.RemessaExterna.AsNoTracking().Where(x => x.Id == remessa).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<RemessaExterna>> ObterRemessasComEntregadora(Guid condominio, DateTime dataInicio, DateTime dataFim)
        {
            return await Db.RemessaExterna.AsNoTracking()
                .Include(x => x.EmpresaSimplificada)
                .Where(x => x.CondominioId == condominio && x.DataCadastro >= dataInicio && x.DataCadastro <= dataFim.AddHours(23).AddMinutes(59))
                .ToListAsync();
        }
    }
}
