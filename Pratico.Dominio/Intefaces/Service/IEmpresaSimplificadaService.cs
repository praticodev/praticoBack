using Pratico.Dominio.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Pratico.Dominio.Intefaces.Service
{
    public interface IEmpresaSimplificadaService : IDisposable
    {
        Task<EmpresaSimplificada> Adicionar(EmpresaSimplificada empresa, int codigo);
        Task<EmpresaSimplificada> Atualizar(EmpresaSimplificada empresa, int codigo);
        Task<bool> VerificaCpnj(string cnpj);
        Task<IEnumerable<EmpresaSimplificada>> ListarEntregadoras(int codigo);
        Task<IEnumerable<TipoDocumentoEntrega>> ObterTipoDocumentos();
        Task<bool> Remover(Guid id, int codCondominio);
    }
}
