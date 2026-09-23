using AutoMapper;
using Microsoft.AspNetCore.Mvc;
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
    [Route("api/Endereco")]
    public class EnderecoController : MainController
    {
        private readonly IEnderecoService _enderecoService;
        private readonly IEnderecoRepository _enderecoRepository;
        private readonly IMapper _mapper;
        public EnderecoController(INotificador notificador,
                                    IUser user,
                                    IMapper mapper,
                                    IEnderecoService enderecoService,
                                    IEnderecoRepository enderecoRepository) : base(notificador, user)
        {
            _enderecoService = enderecoService;
            _enderecoRepository = enderecoRepository;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult> Inserir(EnderecoViewModel enderecoViewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);
            var endereco = await _enderecoService.Adicionar(_mapper.Map<Endereco>(enderecoViewModel));
            return CustomResponse(endereco);
        }

        [HttpGet("{cond:guid}")]
        public async Task<ActionResult> ObterEndereco(Guid cond)
        {
            var contato = _mapper.Map<EnderecoViewModel>(await _enderecoRepository.ObterEnderecoPorCondominio(cond));
            return CustomResponse(contato);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<EnderecoViewModel>> Atualizar(Guid id, EnderecoViewModel enderecoViewModel)
        {
            if (id != enderecoViewModel.Id)
            {
                NotificarErro("O id informado não é o mesmo que foi passado na query");
                return CustomResponse(enderecoViewModel);
            }

            if (!ModelState.IsValid) return CustomResponse(ModelState);

            await _enderecoService.Atualizar(_mapper.Map<Endereco>(enderecoViewModel));

            return CustomResponse(enderecoViewModel);
        }
    }
}
