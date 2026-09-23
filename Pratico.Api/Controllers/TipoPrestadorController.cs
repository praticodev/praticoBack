using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Pratico.Api.ViewModels;
using Pratico.Dominio.Intefaces;
using Pratico.Dominio.Intefaces.Service;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pratico.Api.Controllers
{
    [Route("api/TipoPrestador")]
    public class TipoPrestadorController : MainController
    {
        private readonly IMapper _mapper;
        private readonly IPrestadorServicoService _prestadorServicoService;

        public TipoPrestadorController(INotificador notificador,
                                       IUser appUser,
                                       IMapper mapper,
                                       IPrestadorServicoService prestadorServicoService) : base(notificador, appUser)
        {
            _mapper = mapper;
            _prestadorServicoService = prestadorServicoService;
        }

        [HttpGet]
        [HttpGet("Listar")]
        public async Task<ActionResult> Listar()
        {
            var prestadores = await _prestadorServicoService.ListarPrestadores();
            var result = _mapper.Map<IEnumerable<TipoPrestadorServicoViewModel>>(prestadores);

            return CustomResponse(result);
        }
    }
}
