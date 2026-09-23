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
using System.Linq;
using System.Threading.Tasks;

namespace Pratico.Api.Controllers
{
    //[Authorize]
    [Route("api/Andar")]
    public class AndarController : MainController
    {
        private readonly IAndarService _andarService;
        private readonly IAndarRepository _andarRepository;
        private readonly IMapper _mapper;

        public AndarController(INotificador notificador,
                                  IAndarService andarService,
                                  IAndarRepository andarRepository,
                                  IUser appUser,
                                  IMapper mapper) : base(notificador, appUser)
        {
            _andarService = andarService;
            _andarRepository = andarRepository;
            _mapper = mapper;
        }

        //[ClaimsAuthorize("Administrador", "Cadastra,Edita,Visualiza,Exclui")]
        [HttpPost]
        public async Task<ActionResult> Inserir(AndarViewModel andarViewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            foreach (var item in andarViewModel.Imoveis)
                item.NumAndar = andarViewModel.NumandarInterno;

            var result = await _andarService.Adicionar(_mapper.Map<Andar>(andarViewModel));
            return CustomResponse(result);
        }

        //[Authorize(Roles = "Administrador, Operador")]
        [HttpGet("ObterTodos/{id:guid}")]
        public async Task<ActionResult> ObterTodos(Guid id)
        {
            var result = await _andarRepository.ObterTodosPorId(id);
            return CustomResponse(_mapper.Map<List<AndarViewModel>>(result));
        }

        //[ClaimsAuthorize("Administrador", "Cadastra,Edita,Visualiza,Exclui")]
        //[ClaimsAuthorize("Zelador", "Cadastra,Edita,Visualiza,Exclui")]
        //[ClaimsAuthorize("Sindico", "Cadastra,Edita,Visualiza,Exclui")]
        //[ClaimsAuthorize("Secretaria", "Cadastra,Edita,Visualiza,Exclui")]
        //[ClaimsAuthorize("Contador", "Cadastra,Edita,Visualiza,Exclui")]
        [HttpGet("{id:guid}")]
        public async Task<ActionResult> Testar(Guid id)
        {
            //var tmp = await _andarService.Imove(id);
            return CustomResponse("resposta");
        }

        //[ClaimsAuthorize("Administrador", "Cadastra,Edita,Visualiza,Exclui")]
        [HttpPut("{id:guid}")]
        public async Task<ActionResult> Atualizar(Guid id, AndarViewModel andarViewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var result = await _andarService.Atualizar(_mapper.Map<Andar>(andarViewModel));
            return CustomResponse(result);
        }

        //[ClaimsAuthorize("Administrador", "Cadastra,Edita,Visualiza,Exclui")]
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> Exclui(Guid id)
        {
            var conjunto = await ObterAndar(id);

            if (conjunto == null)
                return NotFound();

            await _andarService.Remover(id);

            return CustomResponse(conjunto);
        }

        private async Task<AndarViewModel> ObterAndar(Guid id)
        {
            return _mapper.Map<AndarViewModel>(await _andarRepository.ObterAndarPorId(id));
        }
    }
}
