using System.Threading.Channels;
using SabemiWebhook.Api.Models;

namespace SabemiWebhook.Api.Background;

public class PagamentoQueue
{
    public Channel<EventoBruto> Queue { get; } =
        Channel.CreateUnbounded<EventoBruto>();
}