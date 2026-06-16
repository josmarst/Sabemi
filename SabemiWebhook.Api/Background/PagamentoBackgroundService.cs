using Microsoft.EntityFrameworkCore;
using SabemiWebhook.Api.Data;
using SabemiWebhook.Api.Models;

namespace SabemiWebhook.Api.Background;

public class PagamentoBackgroundService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly PagamentoQueue _queue;

    public PagamentoBackgroundService(
        IServiceProvider serviceProvider,
        PagamentoQueue queue)
    {
        _serviceProvider = serviceProvider;
        _queue = queue;
    }

    protected override async Task ExecuteAsync(
        CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var evento =
                await _queue.Queue.Reader.ReadAsync(stoppingToken);

            try
            {
                // Simula processamento pesado
                await Task.Delay(2000, stoppingToken);

                using var scope =
                    _serviceProvider.CreateScope();

                var context =
                    scope.ServiceProvider.GetRequiredService<AppDbContext>();

                var contrato =
                    await context.StatusContratos
                        .FirstOrDefaultAsync(x =>
                            x.IdContrato == evento.IdContrato);

                if (contrato == null)
                {
                    contrato = new StatusContrato
                    {
                        IdContrato = evento.IdContrato
                    };

                    context.StatusContratos.Add(contrato);
                }

                contrato.Status = evento.Status;
                contrato.UltimaAtualizacao = DateTime.UtcNow;

                var eventoBanco =
                    await context.EventosBrutos
                        .FirstAsync(x => x.Id == evento.Id);

                eventoBanco.Processado = true;

                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                using var scope =
                    _serviceProvider.CreateScope();

                var context =
                    scope.ServiceProvider.GetRequiredService<AppDbContext>();

                var eventoBanco =
                    await context.EventosBrutos
                        .FirstOrDefaultAsync(x => x.Id == evento.Id);

                if (eventoBanco != null)
                {
                    eventoBanco.Erro = ex.Message;
                    await context.SaveChangesAsync();
                }
            }
        }
    }
}