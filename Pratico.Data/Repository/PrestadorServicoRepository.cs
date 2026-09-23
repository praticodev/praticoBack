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
    public class PrestadorServicoRepository : Repository<TipoPrestadorServico>, IPrestadorServicoRepository
    {
        public PrestadorServicoRepository(PraticoContext context) : base(context)
        {
        }

        public async Task<PrestadorServico> AdicionarPrestador(PrestadorServico prestador)
        {
            Db.PrestadorServico.Add(prestador);
            await SaveChanges();
            return prestador;
        }

        public async Task AtualizarPrestador(PrestadorServico prestador)
        {
            Db.PrestadorServico.Update(prestador);
            await SaveChanges();
        }

        public async Task RemoverPrestador(Guid id)
        {
            Db.PrestadorServico.Remove(new PrestadorServico { Id = id });
            await SaveChanges();
        }

        public async Task<IEnumerable<PrestadorServico>> ObterPorUnidade(Guid imovelId, Guid condominioId)
        {
            return await Db.PrestadorServico
                .AsNoTracking()
                .Where(x => x.ImovelId == imovelId && x.CondominioId == condominioId)
                .ToListAsync();
        }

        public async Task<PrestadorServico> ObterPrestadorPorId(Guid id, Guid condominioId)
        {
            return await Db.PrestadorServico
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id && x.CondominioId == condominioId);
        }

        public async Task<IEnumerable<PrestadorServico>> ObterAlteradosPorSyncLog(Guid condominioId, Guid imovelId, long token)
        {
            return await Db.PrestadorServico
                .AsNoTracking()
                .Where(prestador =>
                    prestador.CondominioId == condominioId &&
                    prestador.ImovelId == imovelId &&
                    Db.SyncLog
                        .AsNoTracking()
                        .Any(log =>
                            log.CondominioId == condominioId &&
                            log.ImovelId == imovelId &&
                            log.RegistroId == prestador.Id &&
                            log.Entidade == nameof(PrestadorServico) &&
                            log.Operacao != "DELETE" &&
                            log.Token > token))
                .ToListAsync();
        }
    }
}
