using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
    [Route("api/PrestadorServico")]
    public class PrestadorServicoController : MainController
    {
        private readonly IMapper _mapper;
        private readonly IPrestadorServicoService _prestadorServicoService;

        public PrestadorServicoController(INotificador notificador,
                                   IUser appUser,
                                   IMapper mapper,
                                   IPrestadorServicoService prestadorServicoService) : base(notificador, appUser)
        {
            _mapper = mapper;
            _prestadorServicoService = prestadorServicoService;
        }

        [HttpPost]
        [HttpPost("Inserir")]
        public async Task<ActionResult> Inserir([FromForm] PrestadorServicoViewModel prestadorViewModel, [FromForm] IFormFile foto)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var prestador = _mapper.Map<PrestadorServico>(prestadorViewModel);
            var result = await _prestadorServicoService.Adicionar(prestador, foto, prestadorViewModel.CodCondominio);

            return CustomResponse(result);
        }

        [HttpPut("{id:guid}")]
        [HttpPut("Atualizar/{id:guid}")]
        [Consumes("application/json")]
        public async Task<ActionResult> Atualizar(Guid id, [FromBody] PrestadorServicoViewModel prestadorViewModel)
        {
            return await AtualizarPrestador(id, prestadorViewModel, null);
        }

        [HttpPut("{id:guid}")]
        [HttpPut("Atualizar/{id:guid}")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult> AtualizarComFoto(Guid id, [FromForm] PrestadorServicoViewModel prestadorViewModel, [FromForm] IFormFile foto)
        {
            return await AtualizarPrestador(id, prestadorViewModel, foto);
        }

        private async Task<ActionResult> AtualizarPrestador(Guid id, PrestadorServicoViewModel prestadorViewModel, IFormFile foto)
        {
            if (id != prestadorViewModel.Id)
            {
                NotificarErro("O id informado nao e o mesmo que foi passado na query");
                return CustomResponse(prestadorViewModel);
            }

            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var prestador = _mapper.Map<PrestadorServico>(prestadorViewModel);
            prestador.Id = id;

            var atualizado = await _prestadorServicoService.Atualizar(prestador, foto, prestadorViewModel.CodCondominio);
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

            await _prestadorServicoService.Remover(id, codCondominio);
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

            var result = await _prestadorServicoService.ObterPorUnidade(imovelId, codCondominio);
            return CustomResponse(_mapper.Map<IEnumerable<PrestadorServicoViewModel>>(result));
        }

        [HttpGet("Imagem/{nome}")]
        public async Task<ActionResult> ObterImagem(string nome)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var result = await S3Service.DownloadAsync(nome, "prestadores");

            return File(
                result.ResponseStream,
                result.Headers.ContentType
            );
        }
    }
}
