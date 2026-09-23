using Pratico.Dominio.Intefaces;
using Pratico.Dominio.Intefaces.Repository;
using Pratico.Dominio.Intefaces.Service;
using Pratico.Dominio.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pratico.Business.Services
{
    public class SyncLogService : BaseService, ISyncLogService
    {
        private readonly ISyncLogRepository _syncLogRepository;
        private readonly IPessoaRepository _pessoaRepository;
        private readonly ICondominioRepository _condominioRepository;
        private readonly IContatoRepository _contatoRepository;
        private readonly IMoradorImovelRepository _moradorImovelRepository;
        private readonly IVeiculoRepository _veiculoRepository;
        private readonly IPrestadorServicoRepository _prestadorServicoRepository;
        private readonly IVisitanteImovelRepository _visitanteImovelRepository;
        private readonly IEmpresaSimplificadaRepository _empresaSimplificadaRepository;
        private readonly IRemessaInternaRepository _remessaInternaRepository;

        public SyncLogService(ISyncLogRepository syncLogRepository, 
                              IPessoaRepository pessoaRepository, 
                              INotificador notificador, 
                              ICondominioRepository condominioRepository,
                              IMoradorImovelRepository moradorImovelRepository,
                              IVeiculoRepository veiculoRepository,
                              IPrestadorServicoRepository prestadorServicoRepository,
                              IVisitanteImovelRepository visitanteImovelRepository,
                              IEmpresaSimplificadaRepository empresaSimplificadaRepository,
                              IRemessaInternaRepository remessaInternaRepository,
                              IContatoRepository contatoRepository) : base(notificador)
        {
            _syncLogRepository = syncLogRepository;
            _pessoaRepository = pessoaRepository;
            _condominioRepository = condominioRepository;
            _contatoRepository = contatoRepository;
            _moradorImovelRepository = moradorImovelRepository;
            _veiculoRepository = veiculoRepository;
            _prestadorServicoRepository = prestadorServicoRepository;
            _visitanteImovelRepository = visitanteImovelRepository;
            _empresaSimplificadaRepository = empresaSimplificadaRepository;
            _remessaInternaRepository = remessaInternaRepository;
        }

        public async Task<SyncLog> Adicionar(SyncLog syncLog)
        {
            return await _syncLogRepository.Adicionar(syncLog);
        }

        public async Task<bool> Atualizar(SyncLog syncLog)
        {
            await _syncLogRepository.Atualizar(syncLog);
            return true;
        }

        public async Task<IEnumerable<SyncLog>> ObterPorCondominio(Guid condominioId)
        {
            return await _syncLogRepository.ObterPorCondominio(condominioId);
        }

        public async Task<IEnumerable<SyncLog>> ObterPorCondominioEToken(Guid condominioId, long token)
        {
            return await _syncLogRepository.ObterPorCondominioEToken(condominioId, token);
        }

        public void Dispose()
        {
            _syncLogRepository?.Dispose();
        }

        public async Task<SincronizacaoDados> ObterSincronizacao(int codCondominio, Guid? imovelId, long token)
        {
            var condominio = await _condominioRepository.ObterCondominioPorCodigo(codCondominio);
            if (condominio == null)
                return new SincronizacaoDados { Codigo = 0 };

            var moradores = imovelId.HasValue
                ? await _pessoaRepository.ObterMoradoresAlteradosPorSyncLog(condominio.Id, imovelId.Value, token)
                : Enumerable.Empty<Morador>();

            var veiculos = imovelId.HasValue
                ? await _veiculoRepository.ObterAlteradosPorSyncLog(condominio.Id, imovelId.Value, token)
                : Enumerable.Empty<Veiculo>();

            var prestadoresServico = imovelId.HasValue
                ? await _prestadorServicoRepository.ObterAlteradosPorSyncLog(condominio.Id, imovelId.Value, token)
                : Enumerable.Empty<PrestadorServico>();

            var visitantesImovel = imovelId.HasValue
                ? await _visitanteImovelRepository.ObterAlteradosPorSyncLog(condominio.Id, imovelId.Value, token)
                : Enumerable.Empty<VisitanteImovel>();

            var transportadoras = await _empresaSimplificadaRepository.ObterAlteradasPorSyncLog(condominio.Id, token);

            var remessas = imovelId.HasValue
                ? await _remessaInternaRepository.ObterAlteradasPorSyncLog(condominio.Id, imovelId.Value, token)
                : Enumerable.Empty<RemessaInterna>();

            var excluidos = await ObterExcluidosPorCondominio(condominio.Id, imovelId, token);
            var ultimoToken = await _syncLogRepository.ObterUltimoToken(condominio.Id);

            return new SincronizacaoDados
            {
                Codigo = ultimoToken,
                Moradores = moradores,
                Veiculos = veiculos,
                PrestadoresServico = prestadoresServico,
                VisitantesImovel = visitantesImovel,
                Transportadoras = transportadoras,
                Remessas = remessas,
                Excluidos = excluidos
            };
        }

        public async Task<IEnumerable<Morador>> ObterMoradores(int codCondominio, Guid? imovelId, long token)
        {
            if (imovelId == null)
                return Enumerable.Empty<Morador>();

            var condominio = await _condominioRepository.ObterCondominioPorCodigo(codCondominio);
            if (condominio == null)
                return Enumerable.Empty<Morador>();

            return await _pessoaRepository.ObterMoradoresAlteradosPorSyncLog(condominio.Id, imovelId.Value, token);
        }

        public async Task<IEnumerable<Veiculo>> ObterVeiculos(int codCondominio, Guid? imovelId, long token)
        {
            if (imovelId == null)
                return Enumerable.Empty<Veiculo>();

            var condominio = await _condominioRepository.ObterCondominioPorCodigo(codCondominio);
            if (condominio == null)
                return Enumerable.Empty<Veiculo>();

            return await _veiculoRepository.ObterAlteradosPorSyncLog(condominio.Id, imovelId.Value, token);
        }

        public async Task<IEnumerable<PrestadorServico>> ObterPrestadoresServico(int codCondominio, Guid? imovelId, long token)
        {
            if (imovelId == null)
                return Enumerable.Empty<PrestadorServico>();

            var condominio = await _condominioRepository.ObterCondominioPorCodigo(codCondominio);
            if (condominio == null)
                return Enumerable.Empty<PrestadorServico>();

            return await _prestadorServicoRepository.ObterAlteradosPorSyncLog(condominio.Id, imovelId.Value, token);
        }

        public async Task<IEnumerable<VisitanteImovel>> ObterVisitantesImovel(int codCondominio, Guid? imovelId, long token)
        {
            if (imovelId == null)
                return Enumerable.Empty<VisitanteImovel>();

            var condominio = await _condominioRepository.ObterCondominioPorCodigo(codCondominio);
            if (condominio == null)
                return Enumerable.Empty<VisitanteImovel>();

            return await _visitanteImovelRepository.ObterAlteradosPorSyncLog(condominio.Id, imovelId.Value, token);
        }

        public async Task<IEnumerable<EmpresaSimplificada>> ObterTransportadoras(int codCondominio, long token)
        {
            var condominio = await _condominioRepository.ObterCondominioPorCodigo(codCondominio);
            if (condominio == null)
                return Enumerable.Empty<EmpresaSimplificada>();

            return await _empresaSimplificadaRepository.ObterAlteradasPorSyncLog(condominio.Id, token);
        }

        public async Task<IEnumerable<RemessaInterna>> ObterRemessas(int codCondominio, Guid? imovelId, long token)
        {
            if (imovelId == null)
                return Enumerable.Empty<RemessaInterna>();

            var condominio = await _condominioRepository.ObterCondominioPorCodigo(codCondominio);
            if (condominio == null)
                return Enumerable.Empty<RemessaInterna>();

            return await _remessaInternaRepository.ObterAlteradasPorSyncLog(condominio.Id, imovelId.Value, token);
        }

        public async Task<IEnumerable<SyncLog>> ObterExcluidos(int codCondominio, Guid? imovelId, long token)
        {
            var condominio = await _condominioRepository.ObterCondominioPorCodigo(codCondominio);
            if (condominio == null)
                return Enumerable.Empty<SyncLog>();

            return await ObterExcluidosPorCondominio(condominio.Id, imovelId, token);
        }

        private async Task<IEnumerable<SyncLog>> ObterExcluidosPorCondominio(Guid condominioId, Guid? imovelId, long token)
        {
            var logs = Enumerable.Empty<SyncLog>();

            if (imovelId != null)
            {
                logs = await _syncLogRepository.Buscar(x =>
                x.CondominioId == condominioId &&
                x.Operacao == "DELETE" &&
                ((x.Entidade != nameof(Veiculo) && x.Entidade != nameof(Morador) && x.Entidade != nameof(PrestadorServico) && x.Entidade != nameof(VisitanteImovel)) || x.ImovelId == imovelId.Value) &&
                x.Token > token);
                return logs;
            }

            logs = await _syncLogRepository.Buscar(x =>
                x.CondominioId == condominioId &&
                x.Operacao == "DELETE" &&
                ((x.Entidade != nameof(Veiculo) && x.Entidade != nameof(Morador) && x.Entidade != nameof(PrestadorServico) && x.Entidade != nameof(VisitanteImovel) && x.ImovelId == Guid.Empty)) &&
                x.Token > token);
            return logs;
        }

        public async Task<long?> ObterUltimoToken(int codCondominio)
        {
            var condominio = await _condominioRepository.ObterCondominioPorCodigo(codCondominio);
            return condominio != null ? await _syncLogRepository.ObterUltimoToken(condominio.Id) : 0;
        }
    }
}
