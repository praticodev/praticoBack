using Pratico.Dominio.Model;
using System;
using System.Threading.Tasks;

namespace Pratico.Dominio.Intefaces.Service
{
    public interface IAndarService : IDisposable
    {
        Task<Andar> Adicionar(Andar andar);
        Task<bool> Atualizar(Andar andar);
        Task Remover(Guid id);
    }
}
