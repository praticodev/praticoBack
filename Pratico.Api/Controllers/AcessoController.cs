using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Pratico.Business.Services.AcessoVisitante.Commands.Create;
using Pratico.Business.Services.ListaEvento.Commands.Create;
using Pratico.Dominio.Intefaces;
using Pratico.Dominio.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pratico.Api.Controllers
{
    [Route("api/Acesso")]
    public class AcessoController : MainController
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        public AcessoController(IMediator mediator, 
                                  INotificador notificador,
                                  IUser appUser,
                                  IMapper mapper) : base(notificador, appUser)
        {
            _mapper = mapper;
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult> InsereLista(CreateAcessoVisitanteCommand command)
        {
            var response = await _mediator.Send(command);
            return CustomResponse(response);
        }

        [HttpGet]
        public async Task<ActionResult> ListaVisitantes(CreateAcessoVisitanteCommand command)
        {
            var response = await _mediator.Send(command);
            return CustomResponse(response);
        }
    }
}
