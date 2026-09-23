using Pratico.Dominio.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Pratico.Dominio.Intefaces.Repository
{
    public interface IRemessaInternaRepository : IRepository<RemessaInterna>
    {
        Task<IEnumerable<RemessaInterna>> ItensAbertosPorImovel(Guid imovel);
        Task<IEnumerable<RemessaInterna>> ItensPorMorador(Guid morador);
        Task<IEnumerable<RemessaInterna>> RemessasPorMorador(Guid morador);
        Task<RemessaInterna> ObterPorNumero(string numero);
        Task<IEnumerable<RemessaInterna>> ObterPorMorador(Guid id);
        Task<IEnumerable<RemessaInterna>> ObterInternasPorImovelComPessoaRetirada(Guid condominio, Guid imovel);
        Task<IEnumerable<RemessaInterna>> ObterAlteradasPorSyncLog(Guid condominioId, Guid imovelId, long token);
    }
}
