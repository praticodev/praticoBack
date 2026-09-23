using Pratico.Dominio.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Pratico.Dominio.Intefaces.Service
{
    public interface IRelatorioCorrespondenciaService
    {
        Task<List<RemessaExternaRelatorio>> ObterRemessasExternasAbertasRelatorio(int condigo, DateTime? dataInicio, DateTime? dataFim, Guid? operador);
        Task<List<Operador>> ObterOperadoresRelatorio(int condigo);
    }
}
