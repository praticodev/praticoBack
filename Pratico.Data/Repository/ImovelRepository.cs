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
    public class ImovelRepository : Repository<Imovel>, IImovelRepository
    {
        public ImovelRepository(PraticoContext context) : base(context) { }

        public async Task<IEnumerable<Imovel>> ObterImoveisPorAndar(Guid andar)
        {
            var result = await Buscar(x => x.AndarId == andar);
            return result.OrderBy(x => x.NumImovelinterno);
        }

        public async Task<bool> RemoverImoveisPorAndar(IEnumerable<Imovel> imoveis)
        {
            await RemoverTodos(imoveis);
            return true;
        }

        public async Task<Imovel> ImovelCondominio(Guid id)
        {
            return await Db.Imovel.AsNoTracking()
                            .Include(c => c.Andar)
                            .ThenInclude(k => k.Conjunto)
                            .ThenInclude(j => j.Condominio)
                            .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<List<Imovel>> ImoveisProprietario(Guid id, Guid condId)
        {
            return await Db.Imovel.AsNoTracking()
                .Include(c => c.Andar)
                .ThenInclude(k => k.Conjunto)
                .ThenInclude(j => j.Condominio)
                .Where(c => c.Proprietario == id && c.Andar.Conjunto.CondominioId == condId)
                .ToListAsync();
        }
    }
}
