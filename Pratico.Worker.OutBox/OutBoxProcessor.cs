using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Pratico.Data.Context;
using Pratico.Dominio.Enums;
using Pratico.Dominio.Model;

namespace Pratico.Worker.OutBox
{
    public class OutBoxProcessor
    {
        private readonly PraticoContext _context;
        private readonly IOutBoxMessageHandler[] _handlers;
        private readonly ILogger<OutBoxProcessor> _logger;
        private readonly OutBoxWorkerOptions _options;

        public OutBoxProcessor(
            PraticoContext context,
            IEnumerable<IOutBoxMessageHandler> handlers,
            IOptions<OutBoxWorkerOptions> options,
            ILogger<OutBoxProcessor> logger)
        {
            _context = context;
            _handlers = handlers.ToArray();
            _logger = logger;
            _options = options.Value;
        }

        public async Task ProcessarPendentesAsync(CancellationToken cancellationToken)
        {
            var agora = DateTime.Now;
            var tamanhoLote = Math.Max(1, _options.TamanhoLote);
            var maximoTentativas = Math.Max(1, _options.MaximoTentativas);

            var mensagens = await _context.OutBoxMessages
                .Where(mensagem =>
                    mensagem.Status == OutBoxMessageStatus.Pendente ||
                    (mensagem.Status == OutBoxMessageStatus.Falha &&
                     mensagem.Tentativas < maximoTentativas &&
                     (!mensagem.ProximaTentativaEm.HasValue || mensagem.ProximaTentativaEm <= agora)))
                .OrderBy(mensagem => mensagem.DataCadastro)
                .Take(tamanhoLote)
                .ToListAsync(cancellationToken);

            foreach (var mensagem in mensagens)
            {
                await ProcessarMensagemAsync(mensagem, cancellationToken);
            }
        }

        private async Task ProcessarMensagemAsync(OutBoxMessage mensagem, CancellationToken cancellationToken)
        {
            mensagem.Status = OutBoxMessageStatus.Processando;
            mensagem.Tentativas++;
            mensagem.DataUltimaTentativa = DateTime.Now;
            await _context.SaveChangesAsync(cancellationToken);

            try
            {
                var handler = _handlers.FirstOrDefault(h => h.Tipo == mensagem.Tipo);

                if (handler == null)
                    throw new InvalidOperationException($"Nenhum handler OutBox registrado para o tipo '{mensagem.Tipo}'.");

                await handler.ProcessarAsync(mensagem, cancellationToken);

                mensagem.Status = OutBoxMessageStatus.Processado;
                mensagem.DataProcessamento = DateTime.Now;
                mensagem.ProximaTentativaEm = null;
                mensagem.Erro = null;
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao processar mensagem OutBox {MensagemId}", mensagem.Id);

                mensagem.Status = OutBoxMessageStatus.Falha;
                mensagem.Erro = ex.Message.Length > 4000 ? ex.Message.Substring(0, 4000) : ex.Message;
                mensagem.ProximaTentativaEm = CalcularProximaTentativa(mensagem.Tentativas);
            }

            await _context.SaveChangesAsync(cancellationToken);
        }

        private static DateTime CalcularProximaTentativa(int tentativas)
        {
            var minutos = Math.Min(60, Math.Pow(2, tentativas));
            return DateTime.Now.AddMinutes(minutos);
        }
    }
}
