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
    [Route("api/VisitanteImovel")]
    public class VisitanteImovelController : MainController
    {
        private readonly IMapper _mapper;
        private readonly IVisitanteImovelService _visitanteImovelService;

        public VisitanteImovelController(INotificador notificador,
                                         IUser appUser,
                                         IMapper mapper,
                                         IVisitanteImovelService visitanteImovelService) : base(notificador, appUser)
        {
            _mapper = mapper;
            _visitanteImovelService = visitanteImovelService;
        }

        [HttpPost]
        [HttpPost("Inserir")]
        public async Task<ActionResult> Inserir([FromForm] VisitanteImovelViewModel visitanteViewModel, [FromForm] IFormFile foto)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var visitante = _mapper.Map<VisitanteImovel>(visitanteViewModel);
            var result = await _visitanteImovelService.Adicionar(visitante, foto, visitanteViewModel.CodCondominio);

            return CustomResponse(result);
        }

        [HttpPut("{id:guid}")]
        [HttpPut("Atualizar/{id:guid}")]
        [Consumes("application/json")]
        public async Task<ActionResult> Atualizar(Guid id, [FromBody] VisitanteImovelViewModel visitanteViewModel)
        {
            return await AtualizarVisitante(id, visitanteViewModel, null);
        }

        [HttpPut("{id:guid}")]
        [HttpPut("Atualizar/{id:guid}")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult> AtualizarComFoto(Guid id, [FromForm] VisitanteImovelViewModel visitanteViewModel, [FromForm] IFormFile foto)
        {
            return await AtualizarVisitante(id, visitanteViewModel, foto);
        }

        private async Task<ActionResult> AtualizarVisitante(Guid id, VisitanteImovelViewModel visitanteViewModel, IFormFile foto)
        {
            if (id != visitanteViewModel.Id)
            {
                NotificarErro("O id informado nao e o mesmo que foi passado na query");
                return CustomResponse(visitanteViewModel);
            }

            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var visitante = _mapper.Map<VisitanteImovel>(visitanteViewModel);
            visitante.Id = id;

            var atualizado = await _visitanteImovelService.Atualizar(visitante, foto, visitanteViewModel.CodCondominio);
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

            await _visitanteImovelService.Remover(id, codCondominio);
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

            var result = await _visitanteImovelService.ObterPorUnidade(imovelId, codCondominio);
            return CustomResponse(_mapper.Map<IEnumerable<VisitanteImovelViewModel>>(result));
        }

        [HttpGet("Imagem/{nome}")]
        public async Task<ActionResult> ObterImagem(string nome)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var result = await S3Service.DownloadAsync(nome, "visitantes");

            return File(
                result.ResponseStream,
                result.Headers.ContentType
            );
        }
    }
}
