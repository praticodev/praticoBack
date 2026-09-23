using Microsoft.AspNetCore.Mvc;
using Pratico.Business.Services;
using Pratico.Dominio.Intefaces;
using Pratico.Dominio.Intefaces.Service;
using System.Threading.Tasks;
using System;
using Pratico.Dominio.Intefaces.Repository;
using Pratico.Api.ViewModels;
using Pratico.Dominio.Model;
using MediatR;
using Pratico.Business.Services.ListaEvento.Commands.Create;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Pratico.Api.Controllers
{
    [Route("api/Reserva")]
    public class ReservaController : MainController
    {
        private readonly IAreaLazerService _areaLazerService;
        private readonly IAreaLazerRepository _areaLazerRepository;
        private readonly IReversaService _reversaService;
        private readonly IMediator _mediator;
        public ReservaController(IAreaLazerService areaLazerService,
                                IAreaLazerRepository areaLazerRepository,
                                IReversaService reversaService,
                                INotificador notificador,
                                IMediator mediator,
                                IUser appUser) : base(notificador, appUser)
        {
            _areaLazerService = areaLazerService;
            _areaLazerRepository = areaLazerRepository;
            _reversaService = reversaService;
            _mediator = mediator;
        }

        [HttpGet("ObterAreas/{condominio:int}")]
        public async Task<ActionResult> ObterAreas(int condominio)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var result = await _areaLazerService.ObterAreasPorCondominio(condominio);
            return CustomResponse(result);
        }

        [HttpPost("InserirReserva")]
        public async Task<ActionResult> InserirReserva(ReservaViewModel model)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            AgendaAreaLazer reserva = new AgendaAreaLazer();

            if (!model.HoraInicio.Equals(string.Empty))
            {
                var valores = model.HoraInicio.Split(":");
                DateTime inicio = new DateTime(DateTime.Now.Year, model.Mes, model.Dia, Convert.ToInt32(valores[0]), Convert.ToInt32(valores[1]), 0);
                reserva.DataInicio = inicio;
            }

            if (!model.HoraFim.Equals(string.Empty))
            {
                var valoresSec = model.HoraFim.Split(":");
                DateTime fim = new DateTime(DateTime.Now.Year, model.Mes, model.Dia, Convert.ToInt32(valoresSec[0]), Convert.ToInt32(valoresSec[1]), 0);
                reserva.DataFim = fim;
            }

            reserva.AreaLazerId = model.AreaLazerId;

            var result = await _reversaService.Adicionar(reserva, model.PessoaId, model.CodCond);

            //var result = await _areaLazerService.ObterAreasPorCondominio(condominio);
            return CustomResponse();
        }

        [HttpGet("ListarPendentes/{condominio:int}")]
        public async Task<ActionResult> ListarPendentes(int condominio)
        {
            var result = await _reversaService.ListarReservasPendetes(condominio);
            return CustomResponse(result);
        }

        [HttpGet("Listar/{condominio:int}/{status:int}/{ano:int}")]
        public async Task<ActionResult> Listar(int condominio, int status, int ano)
        {
            var result = await _reversaService.ListarReservasParametros(condominio,status,ano);
            return CustomResponse(result);
        }

        [HttpGet("Calendario/{condominio:int}/{status:int}")]
        public async Task<ActionResult> Calendario(int condominio, int status)
        {
            var result = await _reversaService.ListarReservasCalendarioParametros(condominio, status);
            return CustomResponse(result);
        }

        [HttpPost("ObterReserva")]
        public async Task<ActionResult> ObterReserva(ConsultaReservaViewModel model)
        {
            var result = await _reversaService.ObterReservas(model.CodCondominio, model.Area, model.Data);
            return CustomResponse(result);
        }

        [HttpPut("AtualizaReserva/{reserva:guid}")]
        public async Task<ActionResult> ListarPendentes(Guid reserva)
        {
            var result = await _reversaService.Atualizar(reserva, true);
            return CustomResponse(result);
        }

        [HttpPut("NegaReserva/{reserva:guid}")]
        public async Task<ActionResult> NegaReserva(Guid reserva)
        {
            var result = await _reversaService.Atualizar(reserva, false);
            return CustomResponse(result);
        }

        [HttpPost("InsereEspera")]
        public async Task<ActionResult> InsereEspera(FilaEsperaViewModel model)
        {
            var result = await _reversaService.AdicionaFila(model.pessoaId, model.reservaId);
            return CustomResponse(result);
        }

        [HttpGet("ListarReservasMorador/{morador:guid}")]
        public async Task<ActionResult> ListarReservasMorador(Guid morador)
        {
            var result = await _reversaService.ObterReservasPorMorador(morador);
            return CustomResponse(result);
        }

        [HttpGet("ListarReservasMes/{condominio:int}/{mes:int}")]
        public async Task<ActionResult> ListarReservasMes(int condominio, int mes)
        {
            var result = await _reversaService.ListarReservasPorMes(condominio, mes);
            return CustomResponse(result);
        }

        [HttpPost("InsereLista")]
        public async Task<ActionResult> InsereLista(CreateListaEventoCommand command)
        {
            //var result = await _reversaService.AdicionaFila(model.pessoaId, model.reservaId);
            //return CustomResponse(result);
            var response = await _mediator.Send(command);
            return null;
        }

    }
}
