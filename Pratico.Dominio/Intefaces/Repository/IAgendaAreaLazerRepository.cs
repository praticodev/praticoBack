using Pratico.Dominio.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Pratico.Dominio.Intefaces.Repository
{
    public interface IAgendaAreaLazerRepository : IRepository<AgendaAreaLazer>
    {
        Task<IEnumerable<AgendaAreaLazer>> ListaCompletaPorMoradorPendente(Guid condominio);
        Task<IEnumerable<AgendaAreaLazer>> ListaCompleta(Guid condominio, int status, int ano);
        Task<IEnumerable<AgendaAreaLazer>> ObterReservas(Guid condominioId, Guid areaId, DateTime data);
        Task<IEnumerable<AgendaAreaLazer>> ObterTodasMorador(Guid morador);
        Task<IEnumerable<AgendaAreaLazer>> ObterReservasAutorizadas(Guid morador);
        Task<IEnumerable<AgendaAreaLazer>> ListarReservasPorMes(Guid condominioId, DateTime mes);
        Task<IEnumerable<AgendaAreaLazer>> ListaEventosBase(Guid condominio, int status, int ano);
    }
}
