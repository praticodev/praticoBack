using Pratico.Dominio.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Pratico.Dominio.Intefaces.Service
{
    public interface IReversaService : IDisposable
    {
        Task<AgendaAreaLazer> Adicionar(AgendaAreaLazer agenda, Guid usuario, int codCond);
        Task<bool> Atualizar(Guid agenda, bool acao);
        Task<IEnumerable<AgendaAreaLazer>> ListarReservasPendetes(int codCond);
        Task<IEnumerable<DateTime>> ListarReservasPorMes(int codCond, int mes);
        Task<IEnumerable<AgendaAreaLazer>> ObterReservas(int condCondominio, Guid areaId, DateTime data);
        Task<int> AdicionaFila(Guid pessoaId, Guid agendaId);
        Task<IEnumerable<AgendaAreaLazer>> ObterReservasPorMorador(Guid morador);
        Task<IEnumerable<AgendaAreaLazer>> ObterReservasAprovadasMorador(Guid morador);
        Task<IEnumerable<AgendaAreaLazer>> ListarReservasParametros(int codCond, int status, int ano);
        Task<IEnumerable<EventoCalendario>> ListarReservasCalendarioParametros(int codCond, int status);
    }
}
