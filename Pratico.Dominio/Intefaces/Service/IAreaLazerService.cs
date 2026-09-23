using Pratico.Dominio.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Pratico.Dominio.Intefaces.Service
{
    public interface IAreaLazerService : IDisposable
    {
        Task<AreaLazer> Adicionar(AreaLazer areaLazer);
        Task<IEnumerable<TipoAreaLazer>> ObterTiposAreas(Guid condiminio);
        Task<IEnumerable<AreaLazer>> ObterAreasPorCondominio(int condiminio);
        Task<IEnumerable<AgendaAreaLazerCompleta>> ObterResevasPorPeriodo(Guid area, DateTime inicio, DateTime fim);
    }
}
