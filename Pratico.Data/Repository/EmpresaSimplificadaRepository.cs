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
    public class EmpresaSimplificadaRepository : Repository<EmpresaSimplificada>, IEmpresaSimplificadaRepository
    {
        public EmpresaSimplificadaRepository(PraticoContext context) : base(context) { }

        public async Task<IEnumerable<EmpresaSimplificada>> ObterAlteradasPorSyncLog(Guid condominioId, long token)
        {
            return await DbSet
                .AsNoTracking()
                .Where(empresa =>
                    empresa.CondominioId == condominioId &&
                    Db.SyncLog
                        .AsNoTracking()
                        .Any(log =>
                            log.CondominioId == condominioId &&
                            log.RegistroId == empresa.Id &&
                            log.Entidade == "Transportadora" &&
                            log.Operacao != "DELETE" &&
                            log.Token > token))
                .ToListAsync();
        }
    }
}
