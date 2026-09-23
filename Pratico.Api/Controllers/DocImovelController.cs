using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pratico.Api.Extensions;
using Pratico.Api.ViewModels;
using Pratico.Dominio.Intefaces;
using Pratico.Dominio.Intefaces.Service;
using System;
using System.Threading.Tasks;

namespace Pratico.Api.Controllers
{
    //[Authorize]
    [Route("api/DocImovel")]
    public class DocImovelController : MainController
    {
        private readonly IMapper _mapper;
        private readonly IDocImovelService _docService;
        public DocImovelController(INotificador notificador,
                                  IDocImovelService docService,
                                  IUser appUser,
                                  IMapper mapper) : base(notificador, appUser)
        {
            _docService = docService;
            _mapper = mapper;
        }

        //[ClaimsAuthorize("Administrador", "Cadastra,Edita,Visualiza,Exclui")]
        //[ClaimsAuthorize("Sindico", "Cadastra,Edita,Visualiza,Exclui")]
        //[ClaimsAuthorize("Secretaria", "Cadastra,Edita,Visualiza,Exclui")]
        //[ClaimsAuthorize("AssAdministrativo", "Cadastra,Edita,Visualiza,Exclui")]
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<DocImovelViewModel>> Atualizar(Guid id, int etapa)
        {
            if (etapa < 1)
            {
                NotificarErro("Etapa de alteração inexistente");
                return CustomResponse();
            }
            var result = await _docService.Atualizar(id,etapa);
            return CustomResponse(result);
        }

        //[ClaimsAuthorize("Administrador", "Cadastra,Edita,Visualiza,Exclui")]
        //[ClaimsAuthorize("Sindico", "Cadastra,Edita,Visualiza,Exclui")]
        //[ClaimsAuthorize("Secretaria", "Cadastra,Edita,Visualiza,Exclui")]
        //[ClaimsAuthorize("AssAdministrativo", "Cadastra,Edita,Visualiza,Exclui")]
        //[ClaimsAuthorize("Gerente", "Cadastra,Edita,Visualiza,Exclui")]
        //[ClaimsAuthorize("Zelador", "Cadastra,Edita,Visualiza,Exclui")]
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<DocImovelViewModel>> BuscaDocs(Guid id, int etapa)
        {
            if (etapa < 1)
            {
                NotificarErro("Etapa de alteração inexistente");
                return CustomResponse();
            }
            var result = await _docService.Atualizar(id, etapa);
            return CustomResponse(result);
        }
    }
}
