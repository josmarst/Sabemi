namespace SabemiWebhook.Api.Models;

public class StatusContrato
{
    public int Id { get; set; }

    public string IdContrato { get; set; } = string.Empty;

    public string Status { get; set; } = string.Empty;

    public DateTime UltimaAtualizacao { get; set; }
}