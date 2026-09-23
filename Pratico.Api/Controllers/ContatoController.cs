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
    [Route("api/Contato")]
    public class ContatoController : MainController
    {
        private readonly IContatoService _contatoService;
        private readonly IContatoRepository _contatoRepository;
        private readonly IMapper _mapper;
        public ContatoController(INotificador notificador,
                                    IUser user,
                                    IMapper mapper,
                                    IContatoService contatoService,
                                    IContatoRepository contatoRepository
                                    ) : base(notificador, user)
        {
            _contatoRepository = contatoRepository;
            _contatoService = contatoService;
            _mapper = mapper;
        }

        //[ClaimsAuthorize("Administrador", "Cadastra,Edita,Visualiza,Exclui")]
        [HttpPost]
        public async Task<ActionResult> Inserir(ContatoCondominioViewModel contatoViewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);
            var contato = await _contatoService.Adicionar(_mapper.Map<Contato>(contatoViewModel));
            return CustomResponse(contato);
        }

        //[ClaimsAuthorize("Administrador", "Cadastra,Edita,Visualiza,Exclui")]
        [HttpGet("{cond:guid}")]
        public async Task<ActionResult> ObterContato(Guid cond)
        {
            var contato = _mapper.Map<ContatoViewModel>(await _contatoRepository.ObterContatoPorEntidade(cond));
            return CustomResponse(contato);
        }

        //[ClaimsAuthorize("Administrador", "Cadastra,Edita,Visualiza,Exclui")]
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<ContatoCondominioViewModel>> Atualizar(Guid id, ContatoCondominioViewModel contatoViewModel)
        {
            if (id != contatoViewModel.Id)
            {
                NotificarErro("O id informado não é o mesmo que foi passado na query");
                return CustomResponse(contatoViewModel);
            }

            if (!ModelState.IsValid) return CustomResponse(ModelState);

            await _contatoService.Atualizar(_mapper.Map<Contato>(contatoViewModel));

            return CustomResponse(contatoViewModel);
        }
    }
}
