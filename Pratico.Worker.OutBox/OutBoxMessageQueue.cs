using System;
using System.Threading;
using System.Threading.Tasks;
using Pratico.Data.Context;
using Pratico.Dominio.Model;

namespace Pratico.Worker.OutBox
{
    public class OutBoxMessageQueue : IOutBoxMessageQueue
    {
        private readonly PraticoContext _context;

        public OutBoxMessageQueue(PraticoContext context)
        {
            _context = context;
        }

        public async Task<OutBoxMessage> EnfileirarAsync(
            string tipo,
            string conteudo,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(tipo))
                throw new ArgumentException("O tipo da mensagem OutBox deve ser informado.", nameof(tipo));

            if (string.IsNullOrWhiteSpace(conteudo))
                throw new ArgumentException("O conteudo da mensagem OutBox deve ser informado.", nameof(conteudo));

            var mensagem = new OutBoxMessage
            {
                Tipo = tipo,
                Conteudo = conteudo
            };

            _context.OutBoxMessages.Add(mensagem);
            await _context.SaveChangesAsync(cancellationToken);

            return mensagem;
        }
    }
}
