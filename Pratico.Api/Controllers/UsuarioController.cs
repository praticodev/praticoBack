using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Pratico.Api.ViewModels;
using Pratico.Dominio.Intefaces;
using Pratico.Dominio.Intefaces.Repository;
using Pratico.Dominio.Intefaces.Service;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pratico.Api.Controllers
{
    [Route("api/Usuario")]
    public class UsuarioController : MainController
    {
        private readonly IMapper _mapper;
        private readonly ICondominioRepository _condominioRepository;
        private readonly IConjuntoRepository _conjuntoRepository;
        private readonly IImovelService _imovelService;
        public UsuarioController(INotificador notificador,
                                    IUser user,
                                    ICondominioRepository condominioRepository,
                                    IConjuntoRepository conjuntoRepository,
                                    IImovelService imovelService,
                                    IMapper mapper) : base(notificador, user)
        {
            _mapper = mapper;
            _condominioRepository = condominioRepository;
            _conjuntoRepository = conjuntoRepository;
            _imovelService = imovelService;
        }

        [HttpGet("ObterPorUsuario/{cond}")]
        public async Task<CondominioViewModel> ObterPorUsuario(int cond)
        {
            return _mapper.Map<CondominioViewModel>(await _condominioRepository.ObterCondominioPorCodigo(cond));
        }

        [HttpGet("ObterTodosConjunto/{id:guid}")]
        public async Task<ActionResult> ObterTodosConjunto(Guid id)
        {
            var result = _mapper.Map<List<ConjuntoViewModel>>(await _conjuntoRepository.ObterTodosPorId(id));
            return CustomResponse(result);
        }

        [HttpPost("IndetinficarProprietario")]
        public async Task<ActionResult> Propriedade([FromBody] ImoveilPropriedadeViewModel propriedade)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);
            var result = await _imovelService.AdicionarProprietarioImovelValidacao(propriedade.ImovelId, propriedade.ProprietarioId);
            return CustomResponse(result);
        }
    }
}
