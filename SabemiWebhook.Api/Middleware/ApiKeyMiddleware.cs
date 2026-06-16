namespace SabemiWebhook.Api.Middleware;

public class ApiKeyMiddleware
{
    private readonly RequestDelegate _next;
    private const string API_KEY = "SABEMI123";

    public ApiKeyMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        // Só protege o endpoint do webhook
        if (context.Request.Path.StartsWithSegments("/webhooks"))
        {
            if (!context.Request.Headers.TryGetValue("x-api-key", out var key))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("ApiKey não informada");
                return;
            }

            if (key != API_KEY)
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("ApiKey inválida");
                return;
            }
        }

        await _next(context);
    }
}