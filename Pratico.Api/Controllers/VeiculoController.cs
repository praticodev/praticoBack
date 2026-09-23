using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pratico.Api.Data;
using Pratico.Api.ViewModels;
using Pratico.Business.Utils;
using Pratico.Dominio.Intefaces;
using Pratico.Dominio.Intefaces.Service;
using Pratico.Dominio.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pratico.Api.Controllers
{
    [Route("api/Veiculo")]
    public class VeiculoController : MainController
    {
        private readonly IMapper _mapper;
        private readonly IVeiculoCorService _veiculoCorService;
        private readonly IVeiculoFabricanteService _veiculoFabricanteService;
        private readonly IVeiculoTipoService _veiculoTipoService;
        private readonly IVeiculoService _veiculoService;

        public VeiculoController(INotificador notificador,
                                 IUser user,
                                 IMapper mapper,
                                 IVeiculoCorService veiculoCorService,
                                 IVeiculoFabricanteService veiculoFabricanteService,
                                 IVeiculoTipoService veiculoTipoService,
                                 IVeiculoService veiculoService) : base(notificador, user)
        {
            _mapper = mapper;
            _veiculoCorService = veiculoCorService;
            _veiculoFabricanteService = veiculoFabricanteService;
            _veiculoTipoService = veiculoTipoService;
            _veiculoService = veiculoService;
        }

        [HttpPost("Inserir")]
        public async Task<ActionResult> Inserir([FromForm] VeiculoViewModel veiculoViewModel, [FromForm] IFormFile? foto)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var veiculo = _mapper.Map<Veiculo>(veiculoViewModel);
            var result = await _veiculoService.Adicionar(veiculo, foto, 0);

            return CustomResponse(result);
        }

        [HttpPost("Inserir-app")]
        public async Task<ActionResult> InserirApp([FromForm] VeiculoAppViewModel veiculoViewModel, [FromForm] IFormFile? foto)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var veiculo = _mapper.Map<Veiculo>(veiculoViewModel);
            var result = await _veiculoService.Adicionar(veiculo, foto, veiculoViewModel.CodCondominio);

            return CustomResponse(result);
        }

        [HttpPut("{id:guid}")]
        [HttpPut("Atualizar/{id:guid}")]
        [Consumes("application/json")]
        public async Task<ActionResult> Atualizar(Guid id, [FromBody] VeiculoViewModel veiculoViewModel)
        {
            return await AtualizarVeiculo(id, veiculoViewModel, null);
        }

        [HttpPut("{id:guid}")]
        [HttpPut("Atualizar/{id:guid}")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult> AtualizarComFoto(Guid id, [FromForm] VeiculoViewModel veiculoViewModel, [FromForm] IFormFile? foto)
        {
            return await AtualizarVeiculo(id, veiculoViewModel, foto);
        }

        private async Task<ActionResult> AtualizarVeiculo(Guid id, VeiculoViewModel veiculoViewModel, IFormFile? foto)
        {
            if (id != veiculoViewModel.Id)
            {
                NotificarErro("O id informado nao e o mesmo que foi passado na query");
                return CustomResponse(veiculoViewModel);
            }

            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var veiculo = _mapper.Map<Veiculo>(veiculoViewModel);
            veiculo.Id = id;

            var atualizado = await _veiculoService.Atualizar(veiculo, foto, veiculoViewModel.CodCondominio);
            return CustomResponse(atualizado);
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> Remover([FromRoute] Guid id, [FromQuery] int codCondominio)
        {
            if (id == Guid.Empty)
            {
                NotificarErro("O id informado e invalido");
                return CustomResponse();
            }

            await _veiculoService.Remover(id, codCondominio);
            return NoContent();
        }

        [HttpGet("Listar-app")]
        public async Task<ActionResult> ObterPorUnidade([FromQuery] Guid imovelId, [FromQuery] int codCondominio)
        {
            if (imovelId == Guid.Empty || codCondominio == 0)
            {
                NotificarErro("Os parametros informados sao invalidos");
                return CustomResponse();
            }

            var result = await _veiculoService.ObterPorUnidade(imovelId, codCondominio);
            return CustomResponse(_mapper.Map<IEnumerable<VeiculoViewModel>>(result));
        }

        [HttpGet("Imagem/{nome}")]
        public async Task<ActionResult> ObterImagem(string nome)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var result = await S3Service.DownloadAsync(nome, "veiculos");

            return File(
                result.ResponseStream,
                result.Headers.ContentType
            );
        }

        [HttpGet("ObterCores")]
        public async Task<ActionResult> ObterCores()
        {
            var result = await _veiculoCorService.ListarCoresCadastradas();
            return CustomResponse(_mapper.Map<IEnumerable<VeiculoCorViewModel>>(result));
        }

        [HttpGet("ObterFabricantes")]
        public async Task<ActionResult> ObterFabricantes()
        {
            var result = await _veiculoFabricanteService.ListarFabricantesCadastrados();
            return CustomResponse(_mapper.Map<IEnumerable<VeiculoFabricanteViewModel>>(result));
        }

        [HttpGet("ObterTipos")]
        public async Task<ActionResult> ObterTipos()
        {
            var result = await _veiculoTipoService.ListarTiposCadastrados();
            return CustomResponse(_mapper.Map<IEnumerable<VeiculoTipoViewModel>>(result));
        }
    }
}
