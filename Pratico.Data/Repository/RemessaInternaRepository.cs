using Microsoft.EntityFrameworkCore;
using Pratico.Data.Context;
using Pratico.Dominio.Intefaces.Repository;
using Pratico.Dominio.Model;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Pratico.Data.Repository
{
    public class RemessaInternaRepository : Repository<RemessaInterna>, IRemessaInternaRepository
    {
        public RemessaInternaRepository(PraticoContext context) : base(context) { }

        public async Task<IEnumerable<RemessaInterna>> ItensAbertosPorImovel(Guid id)
        {
            try
            {
                return await Db.RemessaInterna.AsNoTracking().Where(x => x.ImovelId == id).ToListAsync();
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        public async Task<IEnumerable<RemessaInterna>> ItensPorMorador(Guid id)
        {
            return await Db.RemessaInterna.AsNoTracking()
                            .Include(c => c.PessoaId == id && c.DataEntrega == null).ToListAsync();
        }

        public async Task<RemessaInterna> ObterPorNumero(string numero)
        {
            return await Db.RemessaInterna.AsNoTracking()
                            .Include(C => C.Pessoa)
                            .Include(c => c.Conjunto)
                            .Include(c => c.Condominio)
                            .Include(c => c.Andar)
                            .Include(c => c.Imovel)
                            .Where(x => x.Numero == numero).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<RemessaInterna>> ObterPorMorador(Guid id)
        {
            return await Db.RemessaInterna.AsNoTracking()
                            .Include(C => C.Pessoa)
                            .Include(c => c.Conjunto)
                            .Include(c => c.Condominio)
                            .Include(c => c.Andar)
                            .Include(c => c.Imovel)
                            .Where(x => x.PessoaId == id).ToListAsync();
        }

        public async Task<IEnumerable<RemessaInterna>> ObterInternasPorImovelComPessoaRetirada(Guid condominio, Guid imovel)
        {
            var remessas = await (
                from remessa in Db.RemessaInterna.AsNoTracking()
                join pessoaRetirada in Db.Pessoa.AsNoTracking()
                    on remessa.PessoaRetiradaId equals pessoaRetirada.Id into pessoasRetirada
                from pessoaRetirada in pessoasRetirada.DefaultIfEmpty()
                where remessa.CondominioId == condominio && remessa.ImovelId == imovel
                select new
                {
                    Remessa = remessa,
                    NomeMoradorRetirada = pessoaRetirada != null ? pessoaRetirada.Nome : null
                }).ToListAsync();

            foreach (var remessa in remessas)
                remessa.Remessa.NomeMoradorRetirada = remessa.NomeMoradorRetirada;

            return remessas.Select(x => x.Remessa).ToList();
        }

        public async Task<IEnumerable<RemessaInterna>> ObterAlteradasPorSyncLog(Guid condominioId, Guid imovelId, long token)
        {
            return await DbSet
                .AsNoTracking()
                .Where(remessa =>
                    remessa.CondominioId == condominioId &&
                    remessa.ImovelId == imovelId &&
                    Db.SyncLog
                        .AsNoTracking()
                        .Any(log =>
                            log.CondominioId == condominioId &&
                            log.ImovelId == imovelId &&
                            log.RegistroId == remessa.Id &&
                            log.Entidade == "Remessa" &&
                            log.Operacao != "DELETE" &&
                            log.Token > token))
                .ToListAsync();
        }

        public async Task<IEnumerable<RemessaInterna>> RemessasPorMorador(Guid morador)
        {
            return await Db.RemessaInterna.AsNoTracking().Where(k => k.PessoaId == morador).ToListAsync();
        }
    }
}
