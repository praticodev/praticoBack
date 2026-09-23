using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Pratico.Api.Data;
using Pratico.Api.Extensions;
using Pratico.Api.ViewModels;
using Pratico.Business.Utils;
using Pratico.Data.Repository;
using Pratico.Dominio.Enums;
using Pratico.Dominio.Intefaces;
using Pratico.Dominio.Intefaces.Repository;
using Pratico.Dominio.Intefaces.Service;
using Pratico.Dominio.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pratico.Api.Controllers
{
    //[Authorize]
    [Route("api/Morador")]
    public class MoradorController : MainController
    {
        private readonly IMoradorService _moradorService;
        private readonly IMoradorRepository _moradorRepository;
        private readonly IContatoService _contatoService;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public MoradorController(INotificador notificador, 
                                 IUser appUser,
                                 IMoradorService moradorService,
                                 IMoradorRepository moradorRepository,
                                 IContatoService contatoService,
                                 IMapper mapper,
                                 UserManager<ApplicationUser> userManager) : base(notificador, appUser)
        {
            _moradorService = moradorService;
            _moradorRepository = moradorRepository;
            _contatoService = contatoService;
            _mapper = mapper;
            _userManager = userManager;
        }


        [Authorize(Roles = "Administrador,Condomino,Operador")]
        [HttpPost]
        public async Task<ActionResult> Inserir(MoradorCadastroViewModel moradorViewModel)
        {
            try
            {
                if (!ModelState.IsValid) return CustomResponse(ModelState);
                var morador = await _moradorService.GeraMorador(moradorViewModel.ImovelId, moradorViewModel.Nome, moradorViewModel.Cpf, moradorViewModel.Rg, moradorViewModel.DataNascimento, moradorViewModel.EstadoCivil, moradorViewModel.Sexo, TipoPessoa.Fisica, TipoDocumento.Rg, moradorViewModel.CodCondominio);
                var contato = new Contato
                {
                    Ddd = Convert.ToInt32(moradorViewModel.Telefone.Substring(0, 2)),//.ToInt32(moradorViewModel.Ddd),
                    Email = moradorViewModel.Email,
                    Telefone = moradorViewModel.Telefone,
                    Entidade = morador.Id,
                    Principal = true,
                    CondominioId = morador.CondominioId,
                };
                var moradorGravado = await _moradorService.Adicionar(morador);
                var tmp = await _contatoService.Adicionar(contato);
                return CustomResponse(moradorGravado);
            }
            catch (Exception e)
            {
                throw e;
            }
            
        }

        //[Authorize(Roles = "Administrador,Condomino,Operador")]
        [HttpPut("{id:guid}")]
        [HttpPut("Atualizar/{id:guid}")]
        [Consumes("application/json")]
        public async Task<ActionResult> Atualizar(Guid id, [FromBody] MoradorCadastroViewModel moradorViewModel)
        {
            return await AtualizarMorador(id, moradorViewModel, null);
        }

        //[Authorize(Roles = "Administrador,Condomino,Operador")]
        [HttpPut("{id:guid}")]
        [HttpPut("Atualizar/{id:guid}")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult> AtualizarComFoto(Guid id, [FromForm] MoradorCadastroViewModel moradorViewModel, [FromForm] IFormFile? foto)
        {
            return await AtualizarMorador(id, moradorViewModel, foto);
        }

        private async Task<ActionResult> AtualizarMorador(Guid id, MoradorCadastroViewModel moradorViewModel, IFormFile? foto)
        {
            if (id != moradorViewModel.Id)
            {
                NotificarErro("O id informado não é o mesmo que foi passado na query");
                return CustomResponse(moradorViewModel);
            }

            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var morador = await _moradorService.GeraMorador(
                moradorViewModel.ImovelId,
                moradorViewModel.Nome,
                moradorViewModel.Cpf,
                moradorViewModel.Rg,
                moradorViewModel.DataNascimento,
                moradorViewModel.EstadoCivil,
                moradorViewModel.Sexo,
                TipoPessoa.Fisica,
                moradorViewModel.TipoDocumento,
                moradorViewModel.CodCondominio);

            morador.Id = id;
            //morador.Ativo = moradorViewModel.Ativo;

            await _moradorService.Atualizar(morador, foto);
            var contato = new Contato
            {
                Ddd = Convert.ToInt32(moradorViewModel.Telefone.Substring(0, 2)),//Convert.ToInt32(moradorViewModel.Ddd),
                Email = moradorViewModel.Email,
                Telefone = moradorViewModel.Telefone.Substring(2, moradorViewModel.Telefone.Length - 2),//moradorViewModel.Telefone,
                Entidade = moradorViewModel.Id,
                Principal = true,
                CondominioId = morador.CondominioId,
            };
            await _contatoService.Atualizar(contato);
            return CustomResponse(moradorViewModel);
        }

        //[Authorize(Roles = "Administrador,Condomino,Operador")]
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> Remover([FromRoute] Guid id, [FromQuery] int codCondominio)
        {
            if (id == Guid.Empty)
            {
                NotificarErro("O id informado é inválido");
                return CustomResponse(BadRequest());
            }

            var removido = await _moradorService.Remover(id, codCondominio);
            
            if (!String.IsNullOrEmpty(removido))
            {
                var usuario = await _userManager.FindByIdAsync(removido);
                var claims = await _userManager.GetClaimsAsync(usuario);
                var resultado = await _userManager.RemoveClaimsAsync(usuario, claims);
                await _userManager.DeleteAsync(usuario);
            }
            return NoContent();
        }

        [HttpPost("inserir-app")]
        public async Task<ActionResult> InserirApp([FromForm] MoradorAppViewModel moradorViewModel, [FromForm] IFormFile? foto)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);
            var morador = await _moradorService.GeraMorador(moradorViewModel.ImovelId, moradorViewModel.Nome, moradorViewModel.Cpf, moradorViewModel.Rg, moradorViewModel.DataNascimento, moradorViewModel.EstadoCivil, moradorViewModel.Sexo,TipoPessoa.Fisica, TipoDocumento.Rg, moradorViewModel.CodCondominio);
            var contato = new Contato
            {
                Ddd = Convert.ToInt32(moradorViewModel.Telefone.Substring(0,2)),
                Email = moradorViewModel.Email,
                Telefone = moradorViewModel.Telefone.Substring(2, moradorViewModel.Telefone.Length-2),
                Entidade = morador.Id,
                Principal = true,
                CondominioId = morador.CondominioId,
            };
            var moradorGravado = await _moradorService.AdicionarApp(morador, foto);
            var nvoContato = await _contatoService.Adicionar(contato);
            return CustomResponse(moradorGravado);
        }

        [HttpGet("Imagem/{nome}")]
        public async Task<ActionResult> ObterImagem(string nome)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var result = await S3Service.DownloadAsync(nome, "moradores");

            return File(
                result.ResponseStream,
                result.Headers.ContentType
            );
        }        

        [HttpGet("ObterMorador/{imovel:guid}")]
        public async Task<IEnumerable<MoradorViewModel>> ObterMorador(Guid imovel)
        {
            return _mapper.Map<IEnumerable<MoradorViewModel>>(await _moradorRepository.ObterMoradorPorId(imovel));
        }

        [HttpGet("ObterTodosPorImovel/{imovelId:guid}/{codCondominio:int}")]
        public async Task<ActionResult> ObterTodosPorImovel(Guid imovelId, int codCondominio)
        {
            return CustomResponse(_mapper.Map<IEnumerable<MoradorViewModel>>(await _moradorService.ObterMoradoresPorImovel(imovelId, codCondominio)));
        }

        [HttpGet("total-moradores")]
        public async Task<ActionResult> ObterMoradores(Guid imovel, int codCondominio)
        {
            var total = await _moradorService.ObterQuantidadeMoradoresPorImovel(imovel, codCondominio);
            return CustomResponse(total);
        }
    }
}
