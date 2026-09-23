using Pratico.Dominio.Model;
using System;
using System.Threading.Tasks;

namespace Pratico.Dominio.Intefaces.Repository
{
    public interface IContatoRepository : IRepository<Contato>
    {
        Task<Contato> ObterContatoPorDados(Contato contato);
        Task<Contato> ObterContatoPorEntidade(Guid contato);
    }
}
