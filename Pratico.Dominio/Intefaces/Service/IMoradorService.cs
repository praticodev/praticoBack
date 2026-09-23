using Microsoft.AspNetCore.Http;
using Pratico.Dominio.Enums;
using Pratico.Dominio.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Pratico.Dominio.Intefaces.Service
{
    public interface IMoradorService : IDisposable
    {
        Task<bool> Adicionar(Morador morador);
        Task<Pessoa> AdicionarApp(Morador morador, IFormFile? imagem);
        Task<bool> Atualizar(Morador morador, IFormFile? imagem);
        Task<string> Remover(Guid id, int codCondominio);
        Task<Morador> GeraMorador(Guid imovelId, string nome, string cpf, string numDoc, DateTime dataNascimento, TipoEstadoCivil estadoCivil, TipoSexo sexo, TipoPessoa tipoPessoa, TipoDocumento tipoDocumento, int condominioId);
        Task<IEnumerable<Morador>> ObterMoradoresPorImovel(Guid imovel, int codCondominio);
        Task<int> ObterQuantidadeMoradoresPorImovel(Guid imovelId, int codCOndominio);
    }
}
