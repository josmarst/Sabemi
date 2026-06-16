namespace SabemiWebhook.Api.DTOs;

public class PagamentoWebhookDto
{
    public string IdTransacao { get; set; } = string.Empty;

    public string IdContrato { get; set; } = string.Empty;

    public decimal Valor { get; set; }

    public DateTime DataPagamento { get; set; }

    public string Status { get; set; } = string.Empty;
}