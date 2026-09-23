using Pratico.Dominio.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pratico.Dominio.Intefaces.Repository
{
    public interface IPessoaRepository : IRepository<Pessoa>
    {
        Task<IEnumerable<Morador>> ObterMoradoresAlteradosPorSyncLog(Guid condominioId, Guid imovelId, long token);
    }
}
