using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pratico.Api.Extensions;
using Pratico.Api.ViewModels;
using Pratico.Dominio.Intefaces;
using Pratico.Dominio.Intefaces.Repository;
using Pratico.Dominio.Intefaces.Service;
using Pratico.Dominio.Model;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Pratico.Api.Controllers
{
    //[Authorize]
    [Route("api/Imovel")]
    public class ImovelController : MainController
    {
        private readonly IMapper _mapper;
        private readonly IImovelService _imovelService;
        private readonly IImovelRepository _imovelRepository;
        public ImovelController(INotificador notificador,
                                IUser user,
                                IImovelService imovelService,
                                IImovelRepository imovelRepository,
                                IMapper mapper) : base(notificador, user)
        {
            _mapper = mapper;
            _imovelService = imovelService;
            _imovelRepository = imovelRepository;
        }

        [ClaimsAuthorize("Administrador", "Cadastra")]
        [HttpPost]
        public async Task<ActionResult> Inserir([FromForm] DocumentoImovelViewModel documento)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);
            var imgPrefixo = Guid.NewGuid() + "_";
            if (!await UploadArquivo(documento.Doc, imgPrefixo))
                return CustomResponse(ModelState);

            documento.NomeDoc = imgPrefixo + documento.Doc.FileName.Replace(" ", "");
            var doc = await _imovelService.AdicionarDocumento(_mapper.Map<DocumentoImovel>(documento));
            return CustomResponse(doc);
        }

        [Authorize(Roles = "Condomino")]
        [HttpPost("IndetinficarProprietario")]
        public async Task<ActionResult> Propriedade([FromBody] ImoveilPropriedadeViewModel propriedade)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);
            var result = await _imovelService.AdicionarProprietarioImovelValidacao(propriedade.ImovelId, propriedade.ProprietarioId);
            return CustomResponse(result);
        }

        [ClaimsAuthorize("Administrador", "Cadastra")]
        [HttpPut("doc/{id:guid}")]
        public async Task<ActionResult> AlteraDoc(Guid id, DocumentoImovel doc)
        {
            if (id != doc.Id)
            {
                NotificarErro("O id informado não é o mesmo que foi passado na query");
                return CustomResponse(doc);
            }
            var atualizado = await _imovelService.AtualizarValidacao(id);
            return CustomResponse(atualizado);
        }

        [ClaimsAuthorize("Administrador", "Cadastra")]
        [HttpGet("ObterDocumentosImovel/{id:guid}")]
        public async Task<ActionResult> ObterDocumentosImovel(Guid id)
        {
            //return CustomResponse(_mapper.Map<ImoveDocViewModel>(await _imovelService.ObterDocumentosPorImovel(id)));
            return CustomResponse(await _imovelService.ObterDocumentosPorImovel(id));
        }

        [HttpGet("ObterDocumentosImovelEtapas/{id:guid}")]
        public async Task<ActionResult> ObterDocumentosImovelEtapas(Guid id)
        {
            //return CustomResponse(_mapper.Map<ImoveDocViewModel>(await _imovelService.ObterDocumentosPorImovel(id)));
            return CustomResponse(await _imovelService.ObterDocumentosPorImovel(id));
        }

        [Authorize(Roles = "Administrador,Condomino")]
        [HttpGet("DadosImovel/{id:guid}")]
        public async Task<ActionResult> ObterImovelMorador(Guid id)
        {
            //return CustomResponse(_mapper.Map<CondominioCompletoViewModel>(await _imovelService.ObterImovelProprietario(id)));
            return CustomResponse(_mapper.Map<ImovelCondominioViewModel>(await _imovelService.ObterImovelProprietario(id)));
        }

        //[Authorize(Roles = "Administrador, Operador")]
        [HttpGet("ObterTodos/{id:guid}")]
        public async Task<ActionResult> ObterTodos(Guid id)
        {
            var result = await _imovelRepository.ObterImoveisPorAndar(id);
            return CustomResponse(result);
        }

        [Authorize(Roles = "Administrador,Condomino")]
        [HttpGet("ObterTodosMoradores/{id:guid}/{condId:guid}")]
        public async Task<ActionResult> ObterTodosMoradores(Guid id, Guid condId)
        {
            return CustomResponse(_mapper.Map<ImoveisProprietarioViewModel>(_imovelService.ObterImoveisProprietario(id, condId))); 
        }

        [Authorize(Roles = "Administrador,Condomino")]
        [HttpGet("ObterDadosImovel/{id:guid}")]
        public async Task<ActionResult> ObterDadosImovel(Guid id)
        {
            return CustomResponse(_mapper.Map<ImovelCondominioViewModel>(await _imovelService.ObterDadosImovel(id)));
        }

        [Authorize(Roles = "Administrador,Condomino")]
        [RequestSizeLimit(30000000)]//libera upload de grandes arquivos via request 30 mb
        [HttpPost("Arquivo")]
        public async Task<ActionResult> Arquivo([FromForm] DocsImovelViewModel docsImovelViewModel)
        {
            if (!ModelState.IsValid)
                return CustomResponse(ModelState);

            if (docsImovelViewModel.Doc == null || docsImovelViewModel.Doc.Length == 0)
            {
                NotificarErro("Forneça um arquivo !");
                return CustomResponse();
            }

            var upLoad = docsImovelViewModel.Doc.OpenReadStream();
            var result = await _imovelService.UpLoad(upLoad, docsImovelViewModel.Doc.FileName, docsImovelViewModel.IdImovel, docsImovelViewModel.TipoDoc);
            return CustomResponse(result);
        }

        [RequestSizeLimit(30000000)]//libera upload de grandes arquivos via request 30 mb
        [HttpPost("ArquivoEtapa")]
        public async Task<ActionResult> ArquivoEtapa([FromForm] DocsImovelViewModel docsImovelViewModel)
        {
            if (!ModelState.IsValid)
                return CustomResponse(ModelState);

            if (docsImovelViewModel.Doc == null || docsImovelViewModel.Doc.Length == 0)
            {
                NotificarErro("Forneça um arquivo !");
                return CustomResponse();
            }

            var upLoad = docsImovelViewModel.Doc.OpenReadStream();
            var result = await _imovelService.UpLoad(upLoad, docsImovelViewModel.Doc.FileName, docsImovelViewModel.IdImovel, docsImovelViewModel.TipoDoc);
            return CustomResponse(result);
        }

        [Authorize(Roles = "Administrador,Condomino")]
        [RequestSizeLimit(30000000)]//libera upload de grandes arquivos via request 30 mb
        [HttpPost("ArquivoValidado")]
        public async Task<ActionResult> ArquivoValidado([FromForm] DocsImovelViewModel docsImovelViewModel)
        {
            if (!ModelState.IsValid)
                return CustomResponse(ModelState);

            if (docsImovelViewModel.Doc == null || docsImovelViewModel.Doc.Length == 0)
            {
                NotificarErro("Forneça um arquivo !");
                return CustomResponse();
            }

            var upLoad = docsImovelViewModel.Doc.OpenReadStream();
            var result = await _imovelService.UpLoadValidado(upLoad, docsImovelViewModel.Doc.FileName, docsImovelViewModel.IdImovel, docsImovelViewModel.TipoDoc);
            return CustomResponse(result);
        }

        [Authorize(Roles = "Administrador,Condomino")]
        [HttpDelete("Deleta/{id:guid}/{nome}")]
        public async Task<ActionResult> RemoveArquivo(Guid id, string nome)
        {
            if (id == Guid.Empty)
            {
                NotificarErro("Forneça um arquivo !");
                return CustomResponse();
            }
            var result = await _imovelService.RemoveArquivoAzure(id,nome);
            return CustomResponse(result);
        }

        [Authorize(Roles = "Administrador,Condomino")]
        [HttpGet("BaixaArquivo/{nome}")]
        public async Task<ActionResult> BaixaArquivo(string nome)
        {
            if (String.IsNullOrEmpty(nome))
            {
                NotificarErro("Arquivo inválido !");
                return CustomResponse();
            }

            var result = await _imovelService.BaixaArquivoAzure(nome);
            Stream responseStream = result.ResponseStream;
            var stream = new MemoryStream();
            await responseStream.CopyToAsync(stream);
            stream.Position = 0;

            var retorna = new FileStreamResult(stream, result.Headers["Content-Type"])
            {
                FileDownloadName = nome
            };
            return retorna;
        }
        private async Task<bool> UploadArquivo(IFormFile arquivo, string imgPrefixo)
        {
            if (arquivo == null || arquivo.Length == 0)
            {
                NotificarErro("Forneça um arquivo !");
                return false;
            }
            var nome = arquivo.FileName.Replace(" ", "");
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", imgPrefixo + nome);

            if (System.IO.File.Exists(path))
            {
                NotificarErro("Já existe um arquivo com este nome!");
                return false;
            }

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await arquivo.CopyToAsync(stream);
            }

            return true;
        }
    }
}
