using Pratico.Dominio.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pratico.Dominio.Intefaces.Repository
{
    public interface IVeiculoRepository : IRepository<Veiculo>
    {
        Task<IEnumerable<Veiculo>> ObterPorUnidade(Guid imovelId, Guid condominioId);
        Task<IEnumerable<Veiculo>> ObterAlteradosPorSyncLog(Guid condominioId, Guid imovelId, long token);
    }
}
