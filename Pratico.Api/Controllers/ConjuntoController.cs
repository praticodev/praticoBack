using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pratico.Api.Extensions;
using Pratico.Api.ViewModels;
using Pratico.Dominio.Intefaces;
using Pratico.Dominio.Intefaces.Repository;
using Pratico.Dominio.Intefaces.Service;
using Pratico.Dominio.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pratico.Api.Controllers
{
    //[Authorize]
    [Route("api/Conjunto")]
    public class ConjuntoController : MainController
    {
        private readonly IConjuntoService _conjuntoService;
        private readonly IConjuntoRepository _conjuntoRepository;
        private readonly IMapper _mapper;
        public ConjuntoController(INotificador notificador,
                                    IConjuntoService conjuntoService,
                                    IConjuntoRepository conjuntoRepository,
                                    IUser user,
                                    IMapper mapper) : base(notificador, user)
        {
            _mapper = mapper;
            _conjuntoService = conjuntoService;
            _conjuntoRepository = conjuntoRepository;
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public async Task<ActionResult> Inserir(ConjuntoViewModel conjuntoViewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var result = await _conjuntoService.Adicionar(_mapper.Map<Conjunto>(conjuntoViewModel), conjuntoViewModel.NumTorre);
            return CustomResponse(result);
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost("InserirIguais")]
        public async Task<ActionResult> InserirIguais(ConjuntoIguaisViewModel conjuntoViewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);
            Conjunto conjunto = new Conjunto
            {
                CondominioId = conjuntoViewModel.CondominioId,
                Descricao = conjuntoViewModel.Descricao,
                Nome = conjuntoViewModel.NomeTorre
            };
            var result = await _conjuntoService.AdicionarIguais(conjunto, conjuntoViewModel.QtdAndares, conjuntoViewModel.QtdImoveis, conjuntoViewModel.Area, conjuntoViewModel.Fracao, conjuntoViewModel.NumTorre, conjuntoViewModel.NumPrimeiroImovel);
            return CustomResponse(result);
            //return CustomResponse(true);
        }

        [Authorize(Roles = "Administrador")]
        [HttpGet("ContaConjuntos/{id:guid}")]
        public async Task<ActionResult> ContaConjuntos(Guid id)
        {
            var total = await _conjuntoRepository.ContaTotalConjuntos(id);
            return CustomResponse(total);
        }

        //[Authorize(Roles = "Administrador, Operador")]
        [HttpGet("ContaConjuntosPorCodigo/{codigo}")]
        public async Task<ActionResult> ContaConjuntosPorCodigo(int codigo)
        {
            var total = await _conjuntoService.ObterTodosPorCodigo(codigo);
            return CustomResponse(total);
        }

        [Authorize(Roles = "Administrador,Condomino")]
        [HttpGet("ObterTodos/{id:guid}")]
        public async Task<ActionResult> ObterTodos(Guid id)
        {
            var result = _mapper.Map<List<ConjuntoViewModel>>(await _conjuntoRepository.ObterTodosPorId(id));
            return CustomResponse(result);
        }

        [Authorize(Roles = "Administrador")]
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> Exclui(Guid id)
        {
            var conjunto = await ObterConjunto(id);

            if (conjunto == null)
                return NotFound();

            await _conjuntoService.Remover(id);

            return CustomResponse(conjunto);
        }

        private async Task<ConjuntoViewModel> ObterConjunto(Guid id)
        {
            return _mapper.Map<ConjuntoViewModel>(await _conjuntoRepository.ObterConjuntoPorId(id));
        }
    }
}
