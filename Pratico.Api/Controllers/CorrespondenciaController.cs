using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Pratico.Api.ViewModels;
using Pratico.Business.Services;
using Pratico.Dominio.Intefaces;
using Pratico.Dominio.Intefaces.Service;
using Pratico.Dominio.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pratico.Api.Controllers
{
    //[Authorize]
    [Route("api/Correspondencia")]
    public class CorrespondenciaController : MainController
    {
        private readonly IMapper _mapper;
        private readonly IEmpresaSimplificadaService _empresaService;
        public CorrespondenciaController(INotificador notificador,
                                  IUser appUser,
                                  IEmpresaSimplificadaService empresaService,
                                  IMapper mapper) : base(notificador, appUser)
        {
            _mapper = mapper;
            _empresaService = empresaService;
        }

        [HttpPost]
        public async Task<ActionResult> Inserir(EmpresaSimplificadaViewModel empresa)
        {
            var existe = await _empresaService.VerificaCpnj(empresa.Cnpj);
            if (existe)
                return CustomResponse(false);
            if (!ModelState.IsValid) return CustomResponse(ModelState);
            var result = await _empresaService.Adicionar(_mapper.Map<EmpresaSimplificada>(empresa), empresa.Codigo);
            return CustomResponse(result);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult> Atualizar(Guid id, EmpresaSimplificadaViewModel empresa)
        {
            if (id != empresa.Id)
            {
                NotificarErro("O id informado não é o mesmo que foi passado na query");
                return CustomResponse(empresa);
            }

            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var result = await _empresaService.Atualizar(_mapper.Map<EmpresaSimplificada>(empresa), empresa.Codigo);
            return CustomResponse(result);
        }

        [HttpGet("VerificaCnpj/{cnpj}")]
        public async Task<ActionResult> VerificaCnpj(string cnpj)
        {
            var result = await _empresaService.VerificaCpnj(cnpj);
            return CustomResponse(result);
        }

        [HttpGet("{codCondominio:int}")]
        public async Task<ActionResult> ListarEntregadoras(int codCondominio)
        {
            var result = _mapper.Map<List<EmpresaSimplificadaViewModel>>(await _empresaService.ListarEntregadoras(codCondominio));
            return CustomResponse(result.OrderBy(x => x.NomeFantasia));
        }

        [HttpGet("ListarTipoDocs")]
        public async Task<ActionResult> ListarTipoDocs()
        {
            var result = _mapper.Map<List<TipoDocumentoEntregaViewModel>>(await _empresaService.ObterTipoDocumentos());
            return CustomResponse(result);
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> Remover([FromRoute] Guid id, [FromQuery] int codCondominio)
        {
            if (id == Guid.Empty)
            {
                NotificarErro("O id informado é inválido");
                return CustomResponse(BadRequest());
            }

            var removido = await _empresaService.Remover(id, codCondominio);
            if (removido == true)
                return NoContent();

            return CustomResponse(removido);
        }
    }
}
