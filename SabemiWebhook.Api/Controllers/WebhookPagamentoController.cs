using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SabemiWebhook.Api.Data;
using SabemiWebhook.Api.DTOs;
using SabemiWebhook.Api.Models;
using SabemiWebhook.Api.Background;

namespace SabemiWebhook.Api.Controllers;

[ApiController]
[Route("webhooks/pagamento")]
public class WebhookPagamentoController : ControllerBase
{
    private readonly AppDbContext _context;
	private readonly PagamentoQueue _queue;

    public WebhookPagamentoController(AppDbContext context, PagamentoQueue queue)
    {
        _context = context;
		_queue = queue;
    }

    [HttpPost]
    public async Task<IActionResult> Receber(
        [FromBody] PagamentoWebhookDto dto)
    {
        var existe = await _context.EventosBrutos
            .AnyAsync(x => x.IdTransacao == dto.IdTransacao);

        if (existe)
        {
            return Ok(new
            {
                mensagem = "Evento já recebido anteriormente"
            });
        }

        // var evento = new EventoBruto
        // {
            // IdTransacao = dto.IdTransacao,
            // IdContrato = dto.IdContrato,
            // Valor = dto.Valor,
            // DataPagamento = dto.DataPagamento,
            // Status = dto.Status,
            // Processado = false
        // };
		
	var evento = new EventoBruto
    {
        IdTransacao = dto.IdTransacao,
        IdContrato = dto.IdContrato,
        Valor = dto.Valor,
        DataPagamento = DateTime.SpecifyKind(
            dto.DataPagamento,
            DateTimeKind.Utc),
        Status = dto.Status,
        Processado = false
    };


        _context.EventosBrutos.Add(evento);

        await _context.SaveChangesAsync();
		await _queue.Queue.Writer.WriteAsync(evento);

        return Ok(new
        {
            mensagem = "Webhook recebido com sucesso"
        });
    }
}