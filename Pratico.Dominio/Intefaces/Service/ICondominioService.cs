using Pratico.Dominio.Model;
using System;
using System.Threading.Tasks;

namespace Pratico.Dominio.Intefaces.Service
{
    public interface ICondominioService : IDisposable
    {
        Task<Condominio> Adicionar(Condominio condominio);
        Task<Condominio> ObterCondominioPorCnpj(string cnpj);
        Task<CondominioCompleto> ObterCondominioCompleto(int codigo);
        Task<bool> Atualizar(Condominio condominio);
        //Task Remover(Guid id);
    }
}
