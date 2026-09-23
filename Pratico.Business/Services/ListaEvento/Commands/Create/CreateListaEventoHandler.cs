using MediatR;
using Pratico.Business.Services.Andar.Commands.Create;
using Pratico.Dominio.Intefaces.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Pratico.Business.Services.ListaEvento.Commands.Create
{
    public class CreateListaEventoHandler : IRequestHandler<CreateListaEventoCommand, Dominio.Model.ListaEvento>
    {
        private readonly IListaEventoRepository _listaEventoRepository;

        public CreateListaEventoHandler(IListaEventoRepository listaEventoRepository)
        {
            _listaEventoRepository = listaEventoRepository;
        }
        public async Task<Dominio.Model.ListaEvento> Handle(CreateListaEventoCommand request, CancellationToken cancellationToken)
        {
            try
            {
                return await _listaEventoRepository.Adicionar(request.ListaEvento());
            }
            catch (Exception e)
            {

                throw e;
            }
            
        }
    }
}
