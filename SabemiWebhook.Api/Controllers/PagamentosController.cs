using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SabemiWebhook.Api.Data;

namespace SabemiWebhook.Api.Controllers;

[ApiController]
[Route("pagamentos")]
public class PagamentosController : ControllerBase
{
    private readonly AppDbContext _context;

    public PagamentosController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> Get(
        [FromQuery] string? status,
        [FromQuery] string? contrato)
    {
        var query = _context.EventosBrutos.AsQueryable();

        if (!string.IsNullOrWhiteSpace(status))
        {
            query = query.Where(x => x.Status == status);
        }

        if (!string.IsNullOrWhiteSpace(contrato))
        {
            query = query.Where(x => x.IdContrato.Contains(contrato));
        }

        var pagamentos = await query
            .OrderByDescending(x => x.Id)
            .ToListAsync();

        return Ok(pagamentos);
    }
}