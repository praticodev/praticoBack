using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    [Route("api/Condominio")]
    public class CondominioController : MainController
    {
        private readonly ICondominioService _condominioService;
        private readonly ICondominioRepository _condominioRepository;
        private readonly IMapper _mapper;
        public CondominioController(INotificador notificador,
                                    IUser user,
                                    IMapper mapper,
                                    ICondominioService condominioService,
                                    ICondominioRepository condominioRepository) : base(notificador, user)
        {
            _condominioService = condominioService;
            _condominioRepository = condominioRepository;
            _mapper = mapper;
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public async Task<ActionResult> Inserir(CondominioViewModel condominioViewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);
            var condominio = await _condominioService.Adicionar(_mapper.Map<Condominio>(condominioViewModel));
            return CustomResponse(condominio);
        }

        [HttpGet]
        public async Task<IEnumerable<CondominioViewModel>> ObterTodos()
        {
            return _mapper.Map<IEnumerable<CondominioViewModel>>(await _condominioRepository.ObterCondominiosPorAdministrador());
        }

        [HttpGet("VerificaCodigo/{cod}")]
        public async Task<CondominioViewModel> VerificaCodigo(int cod)
        {
            var result = await _condominioRepository.ObterCondominioPorCodigo(cod);
            return _mapper.Map<CondominioViewModel>(await _condominioRepository.ObterCondominioPorCodigo(cod));
        }

        [Authorize(Roles = "Condomino")]
        [HttpGet("ObterPorMorador/{cond:guid}")]
        public async Task<CondominioViewModel> ObterPorMorador(Guid cond)
        {
            return _mapper.Map<CondominioViewModel>(await _condominioRepository.ObterPorId(cond));
        }

        //[ClaimsAuthorize("Administrador", "Cadastra,Edita,Visualiza,Exclui")]
        [HttpGet("ObterCondominioCompleto/{codigo}")]
        public async Task<ActionResult> ObterCondominioCompleto(int codigo)
        {
            return CustomResponse(_mapper.Map<CondominioCompletoViewModel>(await _condominioService.ObterCondominioCompleto(codigo)));
        }

        //[ClaimsAuthorize("Administrador", "Cadastra,Edita,Visualiza,Exclui")]
        [HttpGet("ObterCondominio/{cod}")]
        public async Task<CondominioViewModel> ObterCondominio(string cod)
        {
            return _mapper.Map<CondominioViewModel>(await _condominioService.ObterCondominioPorCnpj(cod));
        }

        //[ClaimsAuthorize("Administrador", "Cadastra,Edita,Visualiza,Exclui")]
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<CondominioViewModel>> Atualizar(Guid id, CondominioViewModel condominioViewModel)
        {
            if (id != condominioViewModel.Id)
            {
                NotificarErro("O id informado não é o mesmo que foi passado na query");
                return CustomResponse(condominioViewModel);
            }

            if (!ModelState.IsValid) return CustomResponse(ModelState);

            await _condominioService.Atualizar(_mapper.Map<Condominio>(condominioViewModel));

            return CustomResponse(condominioViewModel);
        }
    }
}
