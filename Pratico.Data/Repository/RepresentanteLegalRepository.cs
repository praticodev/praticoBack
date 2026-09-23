using Microsoft.EntityFrameworkCore;
using Pratico.Data.Context;
using Pratico.Dominio.Intefaces.Repository;
using Pratico.Dominio.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Pratico.Data.Repository
{
    public class RepresentanteLegalRepository : Repository<RepresentanteLegal>, IRepresentanteLegalRepository
    {
        public RepresentanteLegalRepository(PraticoContext context) : base(context) { }

        public async Task<RepresentanteLegal> ObterRepresentantePorCnpj(string cnpj)
        {
            return await Db.RepresentanteLegal.AsNoTracking().FirstOrDefaultAsync(x => x.CnpjRep == cnpj);
        }

        public async Task<RepresentanteLegal> ObterRepresentantePorCondominio(Guid id)
        {
            return await Db.RepresentanteLegal.AsNoTracking().FirstOrDefaultAsync(x => x.CondominioId == id);
        }

        public async Task<RepresentanteLegal> ObterRepresentantePorCpf(string cpf)
        {
            return await Db.RepresentanteLegal.AsNoTracking().FirstOrDefaultAsync(x => x.Cpf == cpf);
        }
    }
}
