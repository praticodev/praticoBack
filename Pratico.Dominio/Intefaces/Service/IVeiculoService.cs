using Microsoft.AspNetCore.Http;
using Pratico.Dominio.Model;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;

namespace Pratico.Dominio.Intefaces.Service
{
    public interface IVeiculoService : IDisposable
    {
        Task<Veiculo> Adicionar(Veiculo veiculo, IFormFile imagem, int codCondominio);
        Task<Veiculo> Atualizar(Veiculo veiculo, IFormFile imagem, int codCondominio);
        Task<bool> Remover(Guid id, int codCondominio);
        Task<IEnumerable<Veiculo>> ObterPorUnidade(Guid imovelId, int codCondominio);
    }
}
