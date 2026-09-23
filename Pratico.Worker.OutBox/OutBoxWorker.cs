using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Pratico.Worker.OutBox
{
    public class OutBoxWorker : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<OutBoxWorker> _logger;
        private readonly OutBoxWorkerOptions _options;

        public OutBoxWorker(
            IServiceScopeFactory serviceScopeFactory,
            IOptions<OutBoxWorkerOptions> options,
            ILogger<OutBoxWorker> logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
            _options = options.Value;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            if (!_options.Habilitado)
            {
                _logger.LogInformation("Worker OutBox desabilitado por configuracao.");
                return;
            }

            _logger.LogInformation("Worker OutBox iniciado.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (var scope = _serviceScopeFactory.CreateScope())
                    {
                        var processor = scope.ServiceProvider.GetRequiredService<OutBoxProcessor>();
                        await processor.ProcessarPendentesAsync(stoppingToken);
                    }
                }
                catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
                {
                    return;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro inesperado no Worker OutBox.");
                }

                var intervaloSegundos = Math.Max(1, _options.IntervaloSegundos);
                await Task.Delay(TimeSpan.FromSeconds(intervaloSegundos), stoppingToken);
            }
        }
    }
}
