using Pratico.Dominio.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pratico.Dominio.Intefaces.Service
{
    public interface IVeiculoFabricanteService : IDisposable
    {
        Task<IEnumerable<VeiculoFabricante>> ListarFabricantesCadastrados();
    }
}
