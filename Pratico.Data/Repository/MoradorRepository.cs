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
    public class MoradorRepository : Repository<Morador>, IMoradorRepository
    {
        public MoradorRepository(PraticoContext context) : base(context) { }

        public async Task<IEnumerable<MoradorImovel>> ObterMoradoresPorImovel(Guid imovel)
        {
            return await Db.MoradorImovel.AsNoTracking().Where(x => x.ImovelId == imovel).ToListAsync();
        }

        public async Task<Morador> ObterMoradorPorId(Guid id)
        {
            //return await Db.Morador.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            return null;
        }
    }
}
