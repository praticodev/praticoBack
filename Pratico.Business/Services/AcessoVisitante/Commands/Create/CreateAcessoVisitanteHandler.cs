using MediatR;
using Pratico.Dominio.Intefaces.Repository;
using System.Threading;
using System.Threading.Tasks;

namespace Pratico.Business.Services.AcessoVisitante.Commands.Create
{
    public class CreateAcessoVisitanteHandler : IRequestHandler<CreateAcessoVisitanteCommand, Pratico.Dominio.Model.AcessoVisitante>
    {
        private readonly IAcessoVisitanteRepository _acessoVisitanteRepository;

        public CreateAcessoVisitanteHandler(IAcessoVisitanteRepository acessoVisitanteRepository)
        {
            _acessoVisitanteRepository = acessoVisitanteRepository;
        }

        public async Task<Pratico.Dominio.Model.AcessoVisitante> Handle(CreateAcessoVisitanteCommand request, CancellationToken cancellationToken)
        {
            var result = await _acessoVisitanteRepository.Adicionar(request.Visitante());

            return result;
        }
    }
}
