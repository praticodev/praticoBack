using Pratico.Dominio.Model;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pratico.Dominio.Intefaces.Service
{
    public interface IPrestadorServicoService : IDisposable
    {
        Task<IEnumerable<TipoPrestadorServico>> ListarPrestadores();
        Task<PrestadorServico> Adicionar(PrestadorServico prestador, IFormFile imagem, int codCondominio);
        Task<PrestadorServico> Atualizar(PrestadorServico prestador, IFormFile imagem, int codCondominio);
        Task<bool> Remover(Guid id, int codCondominio);
        Task<IEnumerable<PrestadorServico>> ObterPorUnidade(Guid imovelId, int codCondominio);
    }
}
