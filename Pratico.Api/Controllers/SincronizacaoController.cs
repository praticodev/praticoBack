using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Pratico.Api.Data;
using Pratico.Api.ViewModels;
using Pratico.Business.Services;
using Pratico.Dominio.Intefaces;
using Pratico.Dominio.Intefaces.Repository;
using Pratico.Dominio.Intefaces.Service;
using Pratico.Dominio.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pratico.Api.Controllers
{
    [Route("api/Sincronizacao")]
    public class SincronizacaoController : MainController
    {
        private readonly IMoradorService _moradorService;
        private readonly IMapper _mapper;
        private readonly IVeiculoService _veiculoService;
        private readonly ISyncLogService _syncLogService;
        public SincronizacaoController(INotificador notificador, 
                                       IUser appUser, 
                                       IMoradorService moradorService, 
                                       IMapper mapper,
                                       ISyncLogService syncLogService,
                                       IVeiculoService veiculoService) : base(notificador, appUser)
        {
            _moradorService = moradorService;
            _mapper = mapper;
            _veiculoService = veiculoService;
            _syncLogService = syncLogService;
        }

        [HttpGet("ObterTodosPorImovel/{imovel:guid}/{codCondominio:int}")]
        public async Task<IEnumerable<MoradorViewModel>> ObterTodosPorImovel(Guid imovel, int codCondominio)
        {
            return _mapper.Map<IEnumerable<MoradorViewModel>>(await _moradorService.ObterMoradoresPorImovel(imovel, codCondominio));
        }

        [HttpGet("Listar-app")]
        public async Task<ActionResult> ObterPorUnidade([FromQuery] Guid imovelId, [FromQuery] int codCondominio)
        {
            if (imovelId == Guid.Empty || codCondominio == 0)
            {
                NotificarErro("Os parâmetros informados são inválidos");
                return CustomResponse();
            }

            var result = await _veiculoService.ObterPorUnidade(imovelId, codCondominio);
            return CustomResponse(_mapper.Map<IEnumerable<VeiculoViewModel>>(result));
        }

        [HttpGet("Log-app/{codigo:int}/{token:int}")]
        public async Task<ActionResult> ObterSicronizacao(int codigo, int token, [FromQuery] Guid? imovelId)
        {
            if (imovelId == Guid.Empty)
            {
                NotificarErro("O parâmetro imovelId é obrigatório");
                return CustomResponse();
            }

            var dadosSincronizacao = await _syncLogService.ObterSincronizacao(codigo, imovelId, token);
            var result = new SicronizacaoViewModel()
            {
                Codigo = dadosSincronizacao.Codigo.ToString(),
                Moradores = _mapper.Map<IEnumerable<MoradorViewModel>>(dadosSincronizacao.Moradores),
                Veiculos = _mapper.Map<IEnumerable<VeiculoViewModel>>(dadosSincronizacao.Veiculos),
                PrestadoresServico = _mapper.Map<IEnumerable<PrestadorServicoViewModel>>(dadosSincronizacao.PrestadoresServico),
                VisitantesImovel = _mapper.Map<IEnumerable<VisitanteImovelViewModel>>(dadosSincronizacao.VisitantesImovel),
                Transportadoras = _mapper.Map<IEnumerable<EmpresaSimplificadaViewModel>>(dadosSincronizacao.Transportadoras),
                Remessas = _mapper.Map<IEnumerable<RemessaInternaViewModel>>(dadosSincronizacao.Remessas),
                Excluidos = dadosSincronizacao.Excluidos
                    .Select(x => new DeleteViewModel { Entidade = x.Entidade, Id = x.RegistroId })
                    .ToList()
            };

            return CustomResponse(result);
        }
    }
}
