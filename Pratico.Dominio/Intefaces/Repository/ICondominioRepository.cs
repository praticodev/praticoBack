using Pratico.Dominio.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pratico.Dominio.Intefaces.Repository
{
    public interface ICondominioRepository : IRepository<Condominio>
    {
        Task<IEnumerable<Condominio>> ObterCondominiosPorAdministrador();
        Task<IEnumerable<Condominio>> ObterCondominios();
        Task<Condominio> ObterCondominioPorId(Guid id);
        Task<Condominio> ObterCondominioPorCnpj(string cnpj);
        Task<Condominio> ObterCondominioCompleto(int codigo);
        Task<Condominio> ObterCondominioPorCodigo(int codigo);
    }
}
