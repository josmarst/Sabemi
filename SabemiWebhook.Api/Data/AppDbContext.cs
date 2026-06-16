using Microsoft.EntityFrameworkCore;
using SabemiWebhook.Api.Models;

namespace SabemiWebhook.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<EventoBruto> EventosBrutos => Set<EventoBruto>();

    public DbSet<StatusContrato> StatusContratos => Set<StatusContrato>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<EventoBruto>()
            .HasIndex(e => e.IdTransacao)
            .IsUnique();
    }
}