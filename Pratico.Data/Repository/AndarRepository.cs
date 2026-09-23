using Microsoft.EntityFrameworkCore;
using Pratico.Data.Context;
using Pratico.Dominio.Intefaces.Repository;
using Pratico.Dominio.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pratico.Data.Repository
{
    public class AndarRepository : Repository<Andar>, IAndarRepository
    {
        public AndarRepository(PraticoContext context) : base(context) { }

        public async Task<Andar> ObterAndarPorId(Guid id)
        {
            return await Db.Andar.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Andar>> ObterTodosPorId(Guid id)
        {
            var result = await Buscar(a => a.ConjuntoId == id);
            if (result.ToList().Count > 0)
            {
                result.OrderBy(p => p.NumAndarInterno).ToList();
            }
            //return await Db.Andar.AsNoTracking().Where(a => a.Conjuntoid == id).OrderBy(x => x.NumAndarInterno).ToListAsync();
            return result;

        }
    }
}
