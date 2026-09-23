using Pratico.Dominio.Intefaces.Repository;
using Pratico.Dominio.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pratico.Dominio.Intefaces.Service
{
    public interface ISyncLogService : IDisposable
    {
        Task<SyncLog> Adicionar(SyncLog syncLog);
        Task<bool> Atualizar(SyncLog syncLog);
        Task<IEnumerable<SyncLog>> ObterPorCondominio(Guid condominioId);
        Task<IEnumerable<SyncLog>> ObterPorCondominioEToken(Guid condominioId, long token);
        Task<SincronizacaoDados> ObterSincronizacao(int codCondominio, Guid? imovelId, long token);
        Task<IEnumerable<Morador>> ObterMoradores(int codCondominio, Guid? imovelId, long token);
        Task<IEnumerable<Veiculo>> ObterVeiculos(int codCondominio, Guid? imovelId, long token);
        Task<IEnumerable<PrestadorServico>> ObterPrestadoresServico(int codCondominio, Guid? imovelId, long token);
        Task<IEnumerable<VisitanteImovel>> ObterVisitantesImovel(int codCondominio, Guid? imovelId, long token);
        Task<long?> ObterUltimoToken(int codCondominio);
        Task<IEnumerable<SyncLog>> ObterExcluidos(int codCondominio, Guid? imovelId, long token);
        Task<IEnumerable<EmpresaSimplificada>> ObterTransportadoras(int codCondominio, long token);
        Task<IEnumerable<RemessaInterna>> ObterRemessas(int codCondominio, Guid? imovelId, long token);
    }
}
