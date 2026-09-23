using Pratico.Dominio.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pratico.Dominio.Intefaces.Repository
{
    public interface IPrestadorServicoRepository : IRepository<TipoPrestadorServico>
    {
        Task<PrestadorServico> AdicionarPrestador(PrestadorServico prestador);
        Task AtualizarPrestador(PrestadorServico prestador);
        Task RemoverPrestador(Guid id);
        Task<IEnumerable<PrestadorServico>> ObterPorUnidade(Guid imovelId, Guid condominioId);
        Task<PrestadorServico> ObterPrestadorPorId(Guid id, Guid condominioId);
        Task<IEnumerable<PrestadorServico>> ObterAlteradosPorSyncLog(Guid condominioId, Guid imovelId, long token);
    }
}
