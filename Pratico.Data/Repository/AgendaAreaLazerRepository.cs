using Microsoft.EntityFrameworkCore;
using Pratico.Data.Context;
using Pratico.Dominio.Intefaces.Repository;
using Pratico.Dominio.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace Pratico.Data.Repository
{
    public class AgendaAreaLazerRepository : Repository<AgendaAreaLazer>, IAgendaAreaLazerRepository
    {
        public AgendaAreaLazerRepository(PraticoContext context) : base(context) { }

        public async Task<IEnumerable<AgendaAreaLazer>> ListaCompletaPorMoradorPendente(Guid condominio)
        {
            //return await Db.AgendaAreaLazer.AsNoTracking()
            //    .Include(r => r.AreaLazer)
            //    .Include(c => c.Pessoa)
            //    .ThenInclude(i => i.MoradoresImovel)
            //    .ThenInclude(j => j.Imovel)
            //    .ThenInclude(k => k.Andar)
            //    .ThenInclude(m => m.Conjunto)
            //    .ThenInclude(h => h.Condominio).Where(x => x.Autorizado == false).ToListAsync();

            return await Db.AgendaAreaLazer.AsNoTracking()
                .Include(r => r.AreaLazer)
                .Include(c => c.Pessoa).Where(x => x.Autorizado == 0 && x.CondominioId == condominio).ToListAsync();
        }

        public async Task<IEnumerable<AgendaAreaLazer>> ObterReservas(Guid condominioId, Guid areaId, DateTime data)
        {
            return await Db.AgendaAreaLazer.AsNoTracking()
                .Include(x => x.AreaLazer)
                .Where(x => x.CondominioId == condominioId
                        && x.AreaLazerId == areaId
                        && x.DataInicio >= data 
                        && x.DataFim <= data.AddHours(23).AddMinutes(59))
                .ToListAsync();
        }

        public async Task<IEnumerable<AgendaAreaLazer>> ObterTodasMorador(Guid morador)
        {
            return await Db.AgendaAreaLazer.AsNoTracking()
                .Include(x => x.AreaLazer)
                .Where(x => x.PessoaId == morador)
                .ToListAsync();
        }

        public async Task<IEnumerable<AgendaAreaLazer>> ObterReservasAutorizadas(Guid morador)
        {
            return await Db.AgendaAreaLazer.AsNoTracking()
               .Include(x => x.AreaLazer)
               .Where(x => x.PessoaId == morador && x.Autorizado == 1 && x.DataInicio > DateTime.Now)
               .ToListAsync();
        }

        public async Task<IEnumerable<AgendaAreaLazer>> ListarReservasPorMes(Guid condominioId, DateTime mes)
        {
            return await Db.AgendaAreaLazer.AsNoTracking()
                .Include(x => x.AreaLazer)
                .Where(x => x.CondominioId == condominioId
                        && x.DataInicio.Month == mes.Month
                        && x.DataInicio.Year == mes.Year)
                .ToListAsync();
        }

        public async Task<IEnumerable<AgendaAreaLazer>> ListaCompleta(Guid condominio, int status, int ano)
        {
            return await Db.AgendaAreaLazer.AsNoTracking()
                .Include(r => r.AreaLazer).Where(x => x.Autorizado == status && x.CondominioId == condominio && x.DataCadastro.Year == ano).ToListAsync();
        }

        public async Task<IEnumerable<AgendaAreaLazer>> ListaEventosBase(Guid condominio, int status, int ano)
        {
            return await Db.AgendaAreaLazer.AsNoTracking().Where(x => x.Autorizado == status && x.CondominioId == condominio && x.DataCadastro.Year == ano).ToListAsync();
        }
    }
}
