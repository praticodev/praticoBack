using Pratico.Dominio.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pratico.Dominio.Intefaces.Repository
{
    public interface IVisitanteImovelRepository : IRepository<VisitanteImovel>
    {
        Task<IEnumerable<VisitanteImovel>> ObterPorUnidade(Guid imovelId, Guid condominioId);
        Task<VisitanteImovel> ObterVisitantePorId(Guid id, Guid condominioId);
        Task<IEnumerable<VisitanteImovel>> ObterAlteradosPorSyncLog(Guid condominioId, Guid imovelId, long token);
    }
}
