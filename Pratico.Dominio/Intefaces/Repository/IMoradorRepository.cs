using Pratico.Dominio.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Pratico.Dominio.Intefaces.Repository
{
    public interface IMoradorRepository : IRepository<Morador>
    {
        Task<IEnumerable<MoradorImovel>> ObterMoradoresPorImovel(Guid imovel);
        Task<Morador> ObterMoradorPorId(Guid id);
    }
}
