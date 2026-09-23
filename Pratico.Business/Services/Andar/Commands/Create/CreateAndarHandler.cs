using MediatR;
using Pratico.Dominio.Intefaces.Repository;
using Pratico.Dominio.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Pratico.Business.Services.Andar.Commands.Create
{
    public class CreateAndarHandler : IRequestHandler<CreateAndarCommand, Pratico.Dominio.Model.Andar>
    {
        private readonly IAndarRepository _andarRepository;

        public CreateAndarHandler(IAndarRepository andarRepository)
        {
            _andarRepository = andarRepository;
        }

        public async Task<Pratico.Dominio.Model.Andar> Handle(CreateAndarCommand request, CancellationToken cancellationToken)
        {
            foreach (var item in request.Imoveis)
            {
                GerenciamentoDocImovel doc = new GerenciamentoDocImovel()
                {
                    DocEnviado = false,
                    EmailEnviado = false,
                    DocValidado = false,
                    EmailLido = false
                };
                item.DocImovel = doc;
            }

            var result = await _andarRepository.Adicionar(request.Andar());

            return result;
        }
    }
}
