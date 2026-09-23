using Pratico.Dominio.Intefaces.Repository;
using Pratico.Dominio.Intefaces.Service;
using Pratico.Dominio.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pratico.Business.Services
{
    public class VeiculoCorService : IVeiculoCorService
    {
        private readonly IVeiculoCorRepository _veiculoCorRepository;

        public VeiculoCorService(IVeiculoCorRepository veiculoCorRepository)
        {
            _veiculoCorRepository = veiculoCorRepository;
        }

        public async Task<IEnumerable<VeiculoCor>> ListarCoresCadastradas()
        {
            return await _veiculoCorRepository.ObterTodos();
        }

        public void Dispose()
        {
            _veiculoCorRepository.Dispose();
        }
    }
}
