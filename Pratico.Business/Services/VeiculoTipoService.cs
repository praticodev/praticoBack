using Pratico.Dominio.Intefaces.Repository;
using Pratico.Dominio.Intefaces.Service;
using Pratico.Dominio.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pratico.Business.Services
{
    public class VeiculoTipoService : IVeiculoTipoService
    {
        private readonly IVeiculoTipoRepository _veiculoTipoRepository;

        public VeiculoTipoService(IVeiculoTipoRepository veiculoTipoRepository)
        {
            _veiculoTipoRepository = veiculoTipoRepository;
        }

        public async Task<IEnumerable<VeiculoTipo>> ListarTiposCadastrados()
        {
            return await _veiculoTipoRepository.ObterTodos();
        }

        public void Dispose()
        {
            _veiculoTipoRepository.Dispose();
        }
    }
}
