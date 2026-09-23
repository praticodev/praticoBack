using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pratico.Api.ViewModels;
using Pratico.Dominio.Intefaces;
using Pratico.Dominio.Intefaces.Service;
using Pratico.Dominio.Model;
using System;
using System.Threading.Tasks;

namespace Pratico.Api.Controllers
{
    [Authorize]
    [Route("api/Inquilino")]
    public class InquilinoController : MainController
    {
        private readonly IMapper _mapper;
        private readonly IInquilinoService _inquilinoService;

        public InquilinoController(INotificador notificador,
                                  IInquilinoService inquilinooService,
                                  IUser appUser,
                                  IMapper mapper) : base(notificador, appUser)
        {
            _mapper = mapper;
            _inquilinoService = inquilinooService;
        }

        [Authorize(Roles = "Administrador,Condomino")]
        [HttpPost]
        public async Task<ActionResult> Inserir(DadosPropsViewModel propViewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);
            EnderecoViewModel endViewModel = new EnderecoViewModel
            {
                Bairro = propViewModel.Bairro,
                Cep = propViewModel.Cep.Replace("-", ""),
                Cidade = propViewModel.Cidade,
                Cobranca = propViewModel.Cobranca,
                Complemento = propViewModel.Complemento,
                Logradouro = propViewModel.Logradouro,
                Numero = propViewModel.Numero,
                Referencia = propViewModel.Referencia,
                UF = propViewModel.UF
            };
            var proprietario = await _inquilinoService.Adicionar(_mapper.Map<Inquilino>(propViewModel), _mapper.Map<Endereco>(endViewModel), propViewModel.ImovelId);
            return CustomResponse("ok");
        }

        [Authorize(Roles = "Administrador,Condomino")]
        [HttpGet("ObterDadosInquilino/{id:guid}")]
        public async Task<ActionResult> ObterDadosInquilino(Guid id)
        {
            DadosPropsViewModel dadosPropsViewModel = new DadosPropsViewModel();

            if (!ModelState.IsValid)
                return CustomResponse(ModelState);
            var mostra = await _inquilinoService.ObterInquilinoEndereco(id);
            return CustomResponse(_mapper.Map<DadosPropsViewModel>(await _inquilinoService.ObterInquilinoEndereco(id)));
        }
    }
}
