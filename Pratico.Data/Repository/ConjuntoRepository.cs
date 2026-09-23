using Microsoft.EntityFrameworkCore;
using Pratico.Data.Context;
using Pratico.Dominio.Intefaces.Repository;
using Pratico.Dominio.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace Pratico.Data.Repository
{
    public class ConjuntoRepository : Repository<Conjunto>, IConjuntoRepository
    {
        public ConjuntoRepository(PraticoContext context) : base(context) { }

        public async Task<int> ContaTotalConjuntos(Guid id)
        {
            return await Db.Conjunto.AsNoTracking().CountAsync(p => p.CondominioId == id);
        }

        public async Task<Conjunto> ObterConjuntoPorId(Guid id)
        {
            return await Db.Conjunto.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Conjunto>> ObterTodosPorId(Guid id)
        {
            //return await Buscar(p => p.CondominioId == id);
            return await Db.Conjunto.AsNoTracking()
                .Include(a => a.Andares)
                .ThenInclude(i => i.Imoveis)
                .Where(p => p.CondominioId == id)
                .ToListAsync();
        }
    }
}
