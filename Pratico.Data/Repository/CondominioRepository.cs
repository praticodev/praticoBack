using Microsoft.EntityFrameworkCore;
using Pratico.Data.Context;
using Pratico.Dominio.Intefaces.Repository;
using Pratico.Dominio.Model;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pratico.Data.Repository
{
    public class CondominioRepository : Repository<Condominio>, ICondominioRepository
    {
        public CondominioRepository(PraticoContext context) : base(context) { }

        public async Task<Condominio> ObterCondominioCompleto(int codigo)
        {
            return await Db.Condominio.AsNoTracking()
                            .Include(c => c.Conjuntos)
                            .ThenInclude(k => k.Andares)
                            .ThenInclude(j => j.Imoveis)
                            .ThenInclude(i => i.DocImovel)
                            .FirstOrDefaultAsync(c => c.CodCondominio == codigo);
        }

        public async Task<Condominio> ObterCondominioPorCnpj(string cnpj)
        {
            return await Db.Condominio.AsNoTracking().FirstOrDefaultAsync(x => x.Cnpj == cnpj);
        }

        public async Task<Condominio> ObterCondominioPorCodigo(int codigo)
        {
            return await Db.Condominio.AsNoTracking().FirstOrDefaultAsync(x => x.CodCondominio == codigo);
        }

        public async Task<Condominio> ObterCondominioPorId(Guid id)
        {
            return await Db.Condominio.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<Condominio>> ObterCondominios()
        {
            return await Db.Condominio.AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<Condominio>> ObterCondominiosPorAdministrador()
        {
            return await Buscar(x => x.Ativo == true);
        }
    }
}
