using Microsoft.EntityFrameworkCore;
using Pratico.Data.Context;
using Pratico.Dominio.Intefaces.Repository;
using Pratico.Dominio.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pratico.Data.Repository
{
    public class RelatorioCorrespondenciaRepository : Repository<RelatorioCorrespondencia>, IRelatorioCorrespondenciaRepository
    {
        public RelatorioCorrespondenciaRepository(PraticoContext context) : base(context) { }

        public async Task<IEnumerable<RemessaExterna>> ObterRemessasExternasAbertasRelatorio(Guid condominio, DateTime? dataInicio, DateTime? dataFim, Guid? operador)
        {
            if (dataInicio != null && dataFim != null && operador != null)
            {
                return await Db.RemessaExterna.AsNoTracking()
                            .Include(c => c.Operador)
                            .Where(x => x.CondominioId == condominio && x.DataCadastro >= dataInicio.Value.AddHours(0).AddMinutes(0).AddSeconds(0) && x.DataCadastro <= dataFim.Value.AddHours(23).AddMinutes(59).AddSeconds(59) && x.OperadorId == operador.Value && x.QuantidadeItens > 0)
                            .ToListAsync();
            }
            else if (dataInicio != null && dataFim != null)
            {
                return await Db.RemessaExterna.AsNoTracking()
                            .Include(c => c.Operador)
                            .Where(x => x.CondominioId == condominio && x.DataCadastro >= dataInicio.Value.AddHours(0).AddMinutes(0).AddSeconds(0) && x.DataCadastro <= dataFim.Value.AddHours(23).AddMinutes(59).AddSeconds(59) && x.QuantidadeItens > 0)
                            .ToListAsync();
            }
            else
            {
                return await Db.RemessaExterna.AsNoTracking()
                            .Include(c => c.Operador)
                            .Where(x => x.CondominioId == condominio && x.DataCadastro >= dataInicio.Value.AddHours(0).AddMinutes(0).AddSeconds(0) && x.QuantidadeItens > 0)
                            .ToListAsync();
            }
        }

        public async Task<IEnumerable<RemessaExterna>> ObterRemessasExternasFechadasRelatorio(Guid condominio, DateTime? dataInicio, DateTime? dataFim, Guid? operador)
        {
            if (dataInicio != null && dataFim != null && operador != null)
            {
                return await Db.RemessaExterna.AsNoTracking()
                            .Include(c => c.Operador)
                            .Where(x => x.CondominioId == condominio && x.DataCadastro >= dataInicio.Value.AddHours(0).AddMinutes(0).AddSeconds(0) && x.DataCadastro <= dataFim.Value.AddHours(23).AddMinutes(59).AddSeconds(59) && x.OperadorId == operador.Value && x.QuantidadeItens == 0)
                            .ToListAsync();
            }
            else if (dataInicio != null && dataFim != null)
            {
                return await Db.RemessaExterna.AsNoTracking()
                            .Include(c => c.Operador)
                            .Where(x => x.CondominioId == condominio && x.DataCadastro >= dataInicio.Value.AddHours(0).AddMinutes(0).AddSeconds(0) && x.DataCadastro <= dataFim.Value.AddHours(23).AddMinutes(59).AddSeconds(59) && x.QuantidadeItens == 0)
                            .ToListAsync();
            }
            else
            {
                return await Db.RemessaExterna.AsNoTracking()
                            .Include(c => c.Operador)
                            .Where(x => x.CondominioId == condominio && x.DataCadastro >= dataInicio.Value.AddHours(0).AddMinutes(0).AddSeconds(0) && x.QuantidadeItens == 0)
                            .ToListAsync();
            }
        }
    }
}
