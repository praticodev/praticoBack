using Microsoft.AspNetCore.Http;
using Pratico.Dominio.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Pratico.Dominio.Intefaces.Service
{
    public interface IRemessaService
    {
        Task<RemessaExterna> Adicionar(RemessaExterna remessa, int codCondominio, IFormFile? imagem);
        Task<RemessaInterna> AdicionarInterna(RemessaInterna remessa, int codCondominio, IFormFile? imagem);
        Task<RemessaExterna> ObterRemessa(Guid remessa);
        Task<RemessaCompleta> ObterPorNumero(string numero);
        Task<IEnumerable<RemessaComEmpresa>> ObterRemessaPorCodigoCondominio(Guid conjunto, DateTime dataInicio, DateTime dataFim);
        Task<IEnumerable<RemessaExterna>> ObterRemessasExternasAbertas(int codigo);
        Task<IEnumerable<RemessaInterna>> ObterPorRemessaInternaParametros(Guid conjunto, Guid andar, Guid imovel);
        Task<IEnumerable<RemessaInterna>> ObterInternasPorImovel(int codigo, Guid imovel);
        Task EnviaEmail(RemessaInterna remessa);
        Task EnviaSMS(RemessaInterna remessa);
        Task Testa(Guid remessa);
        Task<List<MoradorCorrespondencia>> ObterPessoasNome(string nome);
        Task<IEnumerable<RemessaInterna>> ObterRemessasPorMorador(Guid morador);
        Task<IEnumerable<RemessaCompleta>> ObterRemessasPorMorador2(Guid morador);
        Task<bool> BaixaRemessa(Guid remessaId, Guid pessoa, Guid imovel, Guid operador, int codCondominio);
    }
}
