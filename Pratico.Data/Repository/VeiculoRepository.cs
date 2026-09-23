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
    public class VeiculoRepository : Repository<Veiculo>, IVeiculoRepository
    {
        public VeiculoRepository(PraticoContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Veiculo>> ObterPorUnidade(Guid imovelId, Guid condominioId)
        {
            return await DbSet
                .AsNoTracking()
                .Include(x => x.Fabricante)
                .Where(x => x.ImovelId == imovelId && x.CondominioId == condominioId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Veiculo>> ObterAlteradosPorSyncLog(Guid condominioId, Guid imovelId, long token)
        {
            return await DbSet
                .AsNoTracking()
                .Where(veiculo =>
                    veiculo.CondominioId == condominioId &&
                    veiculo.ImovelId == imovelId &&
                    Db.SyncLog
                        .AsNoTracking()
                        .Any(log =>
                            log.CondominioId == condominioId &&
                            log.ImovelId == imovelId &&
                            log.RegistroId == veiculo.Id &&
                            log.Entidade == nameof(Veiculo) &&
                            log.Operacao != "DELETE" &&
                            log.Token > token))
                .ToListAsync();
        }
    }
}
