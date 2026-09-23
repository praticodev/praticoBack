using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Pratico.Dominio.Intefaces.Repository;
using Pratico.Dominio.Intefaces.Service;
using Pratico.Dominio.Intefaces;
using Pratico.Data.Repository;
using Pratico.Business.Services;
using System.Threading.Tasks;
using System;
using Pratico.Api.ViewModels;

namespace Pratico.Api.Controllers
{
    [Route("api/RelatorioCorrespondencia")]
    public class RelatorioCorrespondenciaController : MainController
    {
        private readonly IMapper _mapper;
        private readonly IOperadorRepository _operadorRepository;
        private readonly IRelatorioCorrespondenciaService _relatorioCorrespondeciaService;
        public RelatorioCorrespondenciaController(INotificador notificador,
                                    IOperadorRepository operadorRepository,
                                    IRelatorioCorrespondenciaService relatorioCorrespondeciaService,
                                    IUser user,
                                    IMapper mapper) : base(notificador, user)
        {
            _mapper = mapper;
            _operadorRepository = operadorRepository;
            _relatorioCorrespondeciaService = relatorioCorrespondeciaService;
        }

        //[HttpGet("ObterExternas/{codigo}/{inicio}/{fim}/{operador}")]
        //public async Task<ActionResult> ObterRemessasExternas(int codigo, DateTime? inicio, DateTime? fim, Guid? operador)
        //{
        //    if (!ModelState.IsValid) return CustomResponse(ModelState);

        //    var result = await _relatorioCorrespondeciaService.ObterRemessasExternasAbertasRelatorio(codigo, inicio.Value, fim.Value, operador.Value);
        //    return CustomResponse(result);
        //}

        [HttpPost("ObterExternas")]
        public async Task<ActionResult> ObterRemessasExternas(RelatorioRemessaExternaViewModel model)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var result = await _relatorioCorrespondeciaService.ObterRemessasExternasAbertasRelatorio(model.Codigo, model.Inicio, model.Fim, model.Operador);
            return CustomResponse(result);
        }

        [HttpGet("ObterOperadores/{codigo}")]
        public async Task<ActionResult> ObterOperadores(int codigo)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var result = await _relatorioCorrespondeciaService.ObterOperadoresRelatorio(codigo);
            return CustomResponse(result);
        }
    }
}
