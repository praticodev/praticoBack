using Microsoft.EntityFrameworkCore;
using Pratico.Data.Context;
using Pratico.Dominio.Intefaces.Repository;
using Pratico.Dominio.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pratico.Data.Repository
{
    public class VeiculoCorRepository : Repository<VeiculoCor>, IVeiculoCorRepository
    {
        public VeiculoCorRepository(PraticoContext context) : base(context) { }
    }
}
