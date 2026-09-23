using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pratico.Api.ViewModels;
using Pratico.Business.Utils;
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
    [Route("api/Remessa")]
    public class RemessaController : MainController
    {
        private readonly IMapper _mapper;
        private readonly IRemessaService _remessaService;
        private readonly IRemessaRepository _remessaRepository;
        private readonly IRemessaInternaRepository _remessaInternaRepository;

        public RemessaController(IRemessaService remessaService,
                                    IRemessaRepository remessaRepository,
                                    IRemessaInternaRepository remessaInternaRepository,
                                    INotificador notificador,
                                    IUser appUser,
                                    IMapper mapper) : base(notificador, appUser)
        {
            _remessaInternaRepository = remessaInternaRepository;
            _remessaRepository = remessaRepository;
            _remessaService = remessaService;
            _mapper = mapper;
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult> Inserir([FromForm] RemessaExternaViewModel remessa, [FromForm] IFormFile? foto)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);
            var remessaInterna = _mapper.Map<RemessaExterna>(remessa);
            var result = await _remessaService.Adicionar(remessaInterna, Convert.ToInt32(remessa.CodCondominio), foto);
            return CustomResponse();
        }

        [HttpPost("InserirInterna")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult> InserirInterna([FromForm] RemessaInternaViewModel remessa, [FromForm] IFormFile? foto)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);
            //_mapper.Map<RemessaInterna>(remessa);
            var remessaInterna = new RemessaInterna()
            {
                //Numero = remessa.Numero,
                RemessaExternaId = remessa.RemessaExternaId,
                QuantidadeItens = remessa.QuantidadeItens,
                ConjuntoId = remessa.ConjuntoId,
                PessoaId = remessa.MoradorId,
                AndarId = remessa.Andar,
                ImovelId = remessa.ImovelId,
                OperadorId = remessa.OperadorId,
                TipoPacote = remessa.TipoPacote,
                Imagem = foto != null ? foto.Name : String.Empty,
            };

            var result = await _remessaService.AdicionarInterna(remessaInterna, Convert.ToInt32(remessa.CodCondominio), foto);
            if (result != null)
            {
                await _remessaService.EnviaEmail(result);
                await _remessaService.EnviaSMS(result);
            }
            return CustomResponse();
        }

        [HttpGet("Testa/{remessa}")]
        public async Task<ActionResult> Testa(Guid remessa)
        {
            var guid = Guid.NewGuid().ToString();
            //if (!ModelState.IsValid) return CustomResponse(ModelState);
            //await _remessaService.Testa(remessa);
            return CustomResponse(guid);
        }

        [HttpGet("ObterExternas/{condominio}/{dataInicio}/{dataFim}")]
        public async Task<ActionResult> ObterExternas(Guid condominio, string dataInicio, string dataFim)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var result = await _remessaService.ObterRemessaPorCodigoCondominio(condominio, Convert.ToDateTime(dataInicio), Convert.ToDateTime(dataFim));
            var t = result.OrderByDescending(x => x.DataCadastro);
            return CustomResponse(result.OrderByDescending(x => x.DataCadastro));
        }

        [HttpGet("ObterInternas/{conjunto:guid}")]
        public async Task<ActionResult> ObterInternas(Guid conjunto)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var result = await _remessaInternaRepository.Buscar(x => x.ConjuntoId == conjunto);
            return CustomResponse(result.OrderByDescending(x => x.DataCadastro));
        }

        [HttpGet("ObterRemessa/{remessa:guid}")]
        public async Task<ActionResult> ObterRemessa(Guid remessa)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var result = await _remessaService.ObterRemessa(remessa);
            return CustomResponse(result);
        }

        [HttpGet("ObterRemessaPorNumero/{numero}")]
        public async Task<ActionResult> ObterRemessaPorNumero(string numero)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var result = await _remessaService.ObterPorNumero(numero);
            return CustomResponse(result);
        }

        [HttpGet("ObterRemessaAbertas/{codigo}")]
        public async Task<ActionResult> ObterRemessaAbertas(int codigo)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var result = await _remessaService.ObterRemessasExternasAbertas(codigo);
            return CustomResponse(result);
        }

        
        [HttpGet("ObterInternasParametros/{conjunto:guid}/{andar:guid}/{imovel:guid}")]
        public async Task<ActionResult> ObterInternasParametros(Guid conjunto, Guid andar, Guid imovel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var result = await _remessaService.ObterPorRemessaInternaParametros(conjunto, andar, imovel);
            return CustomResponse(result.OrderByDescending(x => x.DataCadastro));
        }

        [HttpGet("ObterInternasPorImovel/{codigo:int}/{imovel:guid}")]
        public async Task<ActionResult> ObterInternasParametros(int codigo, Guid imovel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var result = await _remessaService.ObterInternasPorImovel(codigo, imovel);
            return CustomResponse(result.OrderByDescending(x => x.DataCadastro));
        }

        [HttpGet("BuscaMorador/{nome}")]
        public async Task<ActionResult> BuscaMorador(string nome)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var result = await _remessaService.ObterPessoasNome(nome);
            return CustomResponse(_mapper.Map<IEnumerable<MoradorCorrespondeciaViewModel>>(result));
        }

        [HttpGet("BuscaEncomendasMorador/{morador:guid}")]
        public async Task<ActionResult> BuscaEncomendasMorador2(Guid morador)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var result = await _remessaService.ObterRemessasPorMorador2(morador);
            return CustomResponse(result);
        }

        [HttpPost("BaixaEncomendasMorador")]
        public async Task<ActionResult> BaixaEncomendasMorador(BaixaRemessaInternaViewModel remessa)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var result = await _remessaService.BaixaRemessa(remessa.RemessaId, remessa.Pessoa, remessa.Imovel, remessa.Operador, remessa.CodCondominio);
            return CustomResponse(result);
        }

        [HttpGet("Imagem/{nome}")]
        public async Task<ActionResult> ObterImagem(string nome)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var result = await S3Service.DownloadAsync(nome, "remessas");

            return File(
                result.ResponseStream,
                result.Headers.ContentType
            );
        }
    }
}
