using Pratico.Dominio.Intefaces.Repository;
using Pratico.Dominio.Intefaces.Service;
using Pratico.Dominio.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pratico.Business.Services
{
    public class VeiculoFabricanteService : IVeiculoFabricanteService
    {
        private readonly IVeiculoFabricanteRepository _veiculoFabricanteRepository;

        public VeiculoFabricanteService(IVeiculoFabricanteRepository veiculoFabricanteRepository)
        {
            _veiculoFabricanteRepository = veiculoFabricanteRepository;
        }

        public async Task<IEnumerable<VeiculoFabricante>> ListarFabricantesCadastrados()
        {
            return await _veiculoFabricanteRepository.ObterTopConhecidas();
        }

        public void Dispose()
        {
            _veiculoFabricanteRepository.Dispose();
        }
    }
}
