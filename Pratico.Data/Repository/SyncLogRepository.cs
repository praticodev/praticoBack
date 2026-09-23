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
    public class SyncLogRepository : RepositoryN<SyncLog>, ISyncLogRepository
    {
        public SyncLogRepository(PraticoContext context) : base(context)
        {
        }

        public async Task<IEnumerable<SyncLog>> ObterPorCondominio(Guid condominioId)
        {
            return await DbSet
                .AsNoTracking()
                .Where(x => x.CondominioId == condominioId)
                .OrderBy(x => x.Token)
                .ToListAsync();
        }

        public async Task<IEnumerable<SyncLog>> ObterPorCondominioEToken(Guid condominioId, long token)
        {
            return await DbSet
                .AsNoTracking()
                .Where(x => x.CondominioId == condominioId && x.Token > token)
                .OrderBy(x => x.Token)
                .ToListAsync();
        }

        public async Task<long?> ObterUltimoToken(Guid condominioId)
        {
            return await DbSet
                .AsNoTracking()
                .Where(x => x.CondominioId == condominioId)
                .MaxAsync(x => (long?)x.Token) ?? 0;
        }
    }
}
