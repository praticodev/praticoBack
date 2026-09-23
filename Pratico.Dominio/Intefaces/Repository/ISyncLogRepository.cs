using Pratico.Dominio.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pratico.Dominio.Intefaces.Repository
{
    public interface ISyncLogRepository : IRepositoryN<SyncLog>
    {
        Task<IEnumerable<SyncLog>> ObterPorCondominio(Guid condominioId);
        Task<IEnumerable<SyncLog>> ObterPorCondominioEToken(Guid condominioId, long token);
        Task<long?> ObterUltimoToken(Guid condominioId);
    }
}
