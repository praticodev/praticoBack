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
    public class VisitanteImovelRepository : Repository<VisitanteImovel>, IVisitanteImovelRepository
    {
        public VisitanteImovelRepository(PraticoContext context) : base(context)
        {
        }

        public async Task<IEnumerable<VisitanteImovel>> ObterPorUnidade(Guid imovelId, Guid condominioId)
        {
            return await DbSet
                .AsNoTracking()
                .Where(x => x.ImovelId == imovelId && x.CondominioId == condominioId)
                .ToListAsync();
        }

        public async Task<VisitanteImovel> ObterVisitantePorId(Guid id, Guid condominioId)
        {
            return await DbSet
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id && x.CondominioId == condominioId);
        }

        public async Task<IEnumerable<VisitanteImovel>> ObterAlteradosPorSyncLog(Guid condominioId, Guid imovelId, long token)
        {
            return await DbSet
                .AsNoTracking()
                .Where(visitante =>
                    visitante.CondominioId == condominioId &&
                    visitante.ImovelId == imovelId &&
                    Db.SyncLog
                        .AsNoTracking()
                        .Any(log =>
                            log.CondominioId == condominioId &&
                            log.ImovelId == imovelId &&
                            log.RegistroId == visitante.Id &&
                            log.Entidade == nameof(VisitanteImovel) &&
                            log.Operacao != "DELETE" &&
                            log.Token > token))
                .ToListAsync();
        }
    }
}
