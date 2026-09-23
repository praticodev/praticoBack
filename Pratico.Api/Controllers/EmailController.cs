using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pratico.Api.ViewModels;
using Pratico.Dominio.Intefaces;
using Pratico.Dominio.Intefaces.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pratico.Api.Controllers
{
    //[Authorize]
    [Route("api/Email")]
    public class EmailController : MainController
    {

        private readonly IMapper _mapper;
        private readonly IEmailService _emailServide;
        public EmailController(IUser appUser,
                                IEmailService emailServide,
                                INotificador notificador,
                                IMapper mapper) : base(notificador, appUser)
        {
            _emailServide = emailServide;
            _mapper = mapper;
        }

        //[Authorize(Roles = "Administrador,Condomino")]
        [HttpPost]
        public async Task<ActionResult> EnviaEmailDocProprietario(EmailDocProprietarioViewModel email)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);
            var result = await _emailServide.EnviaLinkProprietario(email.IdImovel, email.Nome, email.Email);
            return CustomResponse(result);
        }

        [HttpPost("email-convite-app")]
        public async Task<ActionResult> EnviaEmailProprietarioCadastro(EmailConviteAppViewModel email)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);
            var result = await _emailServide.EnviaEmailConviteApp(new Guid(email.IdMorador), email.CodCondominio);
            return CustomResponse(result);
        }
    }
}
