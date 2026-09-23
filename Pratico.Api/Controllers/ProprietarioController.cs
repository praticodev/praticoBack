using Amazon;
using Amazon.S3;
using Amazon.S3.Transfer;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pratico.Api.ViewModels;
using Pratico.Dominio.Intefaces;
using Pratico.Dominio.Intefaces.Service;
using Pratico.Dominio.Model;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Pratico.Api.Controllers
{
    [Authorize]
    [Route("api/Proprietario")]
    public class ProprietarioController : MainController
    {
        private readonly IMapper _mapper;
        private readonly IProprietarioService _proprietarioService;

        public ProprietarioController(INotificador notificador,
                                  IProprietarioService proprietarioService,
                                  IUser appUser,
                                  IMapper mapper) : base(notificador, appUser)
        {
            _mapper = mapper;
            _proprietarioService = proprietarioService;
        }

        [HttpGet]
        public async Task<ActionResult> ObterRelacaoDocs(DadosPropsViewModel propViewModel)
        {
            if (!ModelState.IsValid) 
                return CustomResponse(ModelState);

            bool result = true;
            return CustomResponse(result);
        }

        [Authorize(Roles = "Administrador,Condomino")]
        [HttpGet("ObterDadosProprietario/{id:guid}")]
        public async Task<ActionResult> ObterDadosProprietario(Guid id)
        {
            DadosPropsViewModel dadosPropsViewModel = new DadosPropsViewModel();

            if (!ModelState.IsValid)
                return CustomResponse(ModelState);
            var mostra = await _proprietarioService.ObterProprietarioEndereco(id);
            return CustomResponse(_mapper.Map<DadosPropsViewModel>(await _proprietarioService.ObterProprietarioEndereco(id)));
        }

        [Authorize(Roles = "Administrador,Condomino")]
        [HttpPost]
        public async Task<ActionResult> Inserir(DadosPropsViewModel propViewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);
            EnderecoViewModel endViewModel = new EnderecoViewModel
            {
                Bairro = propViewModel.Bairro,
                Cep = propViewModel.Cep.Replace("-",""),
                Cidade = propViewModel.Cidade,
                Cobranca = propViewModel.Cobranca,
                Complemento = propViewModel.Complemento,
                Logradouro = propViewModel.Logradouro,
                Numero = propViewModel.Numero,
                Referencia = propViewModel.Referencia,
                UF = propViewModel.UF
            };
            var proprietario = await _proprietarioService.Adicionar(_mapper.Map<Proprietario>(propViewModel), _mapper.Map<Endereco>(endViewModel), propViewModel.ImovelId);
            return CustomResponse(proprietario);
        }

        [RequestSizeLimit(30000000)]//libera upload de grandes arquivos via request 30 mb
        [HttpPost("Proprietario")]
        public async Task<ActionResult> Inserir(DocsPropsViewModel propViewModel)
        {
            if (!ModelState.IsValid) 
                return CustomResponse(ModelState);

            if (!await UploadArquivo(propViewModel.DocEscritura))
                return CustomResponse(ModelState);

            bool result = true;
            return CustomResponse(result);
        }

        [HttpGet("Amazon")]
        public async Task<ActionResult> AWS()
        {
            //UpAmazon();
            return CustomResponse("Foi");
        }

        private async Task<bool> UploadArquivo(IFormFile arquivo)
        {

            if (arquivo == null || arquivo.Length == 0)
            {
                NotificarErro("Forneça um arquivo!");
                return false;
            }

            MemoryStream ms = new MemoryStream();
            var upLoad = arquivo.OpenReadStream();
            await upLoad.CopyToAsync(ms);

            return await _proprietarioService.UpLoad(ms, arquivo.FileName);
        }
    }
}
