using FluentValidation;
using MediatR;
using Pratico.Dominio.Intefaces.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Pratico.Business.Services.Andar.Commands.Delete
{
    public class DeleteAndarHandle : IRequestHandler<DeleteAndarCommand, bool>
    {
        private readonly IAndarRepository _andarRepository;

        public DeleteAndarHandle(IAndarRepository andarRepository)
        {
            _andarRepository = andarRepository;
        }

        public async Task<bool> Handle(DeleteAndarCommand request, CancellationToken cancellationToken)
        {
            var andar = await _andarRepository.ObterPorId(request.Id);
            if (andar == null)
                return false;

            await _andarRepository.Remover(request.Id);

            return true;
        }
    }
}
