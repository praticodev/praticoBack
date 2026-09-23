using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Pratico.Api.ViewModels;
using Pratico.Dominio.Intefaces;
using Pratico.Dominio.Intefaces.Service;
using Pratico.Dominio.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pratico.Api.Controllers
{
    [Route("api/RepresentanteLegal")]
    public class RepresentanteLegalController : MainController
    {
        private readonly IRepresentanteLegalService _representanteLegalService; 
        private readonly IMapper _mapper;
        public RepresentanteLegalController(INotificador notificador,
                                    IUser user,
                                    IRepresentanteLegalService representanteLegalService,
                                    IMapper mapper) : base(notificador, user)
        {
            _representanteLegalService = representanteLegalService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult> Inserir(RepresentanteLegalViewModel representanteViewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var condominio = await _representanteLegalService.Adicionar(_mapper.Map<RepresentanteLegal>(representanteViewModel));
            return CustomResponse(condominio);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<RepresentanteLegalViewModel>> Atualizar(Guid id, RepresentanteLegalViewModel representanteViewModel)
        {
            if (id != representanteViewModel.Id)
            {
                NotificarErro("O id informado não é o mesmo que foi passado na query");
                return CustomResponse(representanteViewModel);
            }

            if (!ModelState.IsValid) return CustomResponse(ModelState);

            await _representanteLegalService.Atualizar(_mapper.Map<RepresentanteLegal>(representanteViewModel));

            return CustomResponse(representanteViewModel);
        }
    }
}
