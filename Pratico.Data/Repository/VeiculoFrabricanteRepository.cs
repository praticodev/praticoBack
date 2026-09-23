using Microsoft.EntityFrameworkCore;
using Pratico.Data.Context;
using Pratico.Dominio.Intefaces.Repository;
using Pratico.Dominio.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pratico.Data.Repository
{
    public class VeiculoFrabricanteRepository : Repository<VeiculoFabricante>, IVeiculoFabricanteRepository
    {
        public VeiculoFrabricanteRepository(PraticoContext context) : base(context) { }

        public async Task<IEnumerable<VeiculoFabricante>> ObterTopConhecidas()
        {
            var fabricantesDescricao = new List<string>
            {
                "FIAT",
                "VW - VOLKSWAGEN",
                "GM - CHEVROLET",
                "HYUNDAI",
                "TOYOTA",
                "JEEP",
                "RENAULT",
                "BYD",
                "HONDA",
                "NISSAN",
                "FORD"
            };

            List<VeiculoFabricante> fabricantes = await Db.VeiculoFabricante
                .AsNoTracking()
                .Where(x => fabricantesDescricao.Contains(x.Descricao))
                .ToListAsync();

            fabricantes = fabricantes
                .OrderBy(x => fabricantesDescricao)
                .ToList();

            List<VeiculoFabricante> outros = await Db.VeiculoFabricante
                .AsNoTracking()
                .Where(x => !fabricantesDescricao.Contains(x.Descricao))
                .ToListAsync();

            var result = outros.OrderBy(x => x.Descricao).ToList();
            fabricantes.AddRange(result);
            return fabricantes.ToList();
        }
    }
}
