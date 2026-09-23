using Pratico.Dominio.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pratico.Dominio.Intefaces.Service
{
    public interface IConjuntoService : IDisposable
    {
        Task<Conjunto> Adicionar(Conjunto conjunto, int numTorre);
        Task<Conjunto> AdicionarIguais(Conjunto conjunto, int andares, int imoveis, decimal area, decimal Fracao, int numTorre, string NumPrimeiroImovel);
        Task<IEnumerable<Conjunto>> ObterTodosPorCodigo(int codigo);
        Task Remover(Guid id);
    }
}
