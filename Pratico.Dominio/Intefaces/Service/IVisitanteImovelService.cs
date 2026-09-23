using Microsoft.AspNetCore.Http;
using Pratico.Dominio.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pratico.Dominio.Intefaces.Service
{
    public interface IVisitanteImovelService : IDisposable
    {
        Task<VisitanteImovel> Adicionar(VisitanteImovel visitante, IFormFile imagem, int codCondominio);
        Task<VisitanteImovel> Atualizar(VisitanteImovel visitante, IFormFile imagem, int codCondominio);
        Task<bool> Remover(Guid id, int codCondominio);
        Task<IEnumerable<VisitanteImovel>> ObterPorUnidade(Guid imovelId, int codCondominio);
    }
}
