using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Zählerstände.Models;

namespace Zählerstände.Data;

public class ZählerständeDbContext : IdentityDbContext {
    public ZählerständeDbContext(DbContextOptions<ZählerständeDbContext> opts) : base(opts){}
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Meter> Meters => Set<Meter>();
    public DbSet<MeterReading> MeterReadings => Set<MeterReading>();

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Customer>(entity => {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.FirstName).IsRequired();
            entity.Property(e => e.LastName).IsRequired();
            entity.Property(e => e.City).IsRequired();
            entity.Property(e => e.Street).IsRequired();
            entity.Property(e => e.Zip).IsRequired();
            entity.HasMany(e => e.Meters)
                .WithOne(e => e.Customer)
                .HasForeignKey(e => e.CustomerId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
        });
        modelBuilder.Entity<Meter>(entity => {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.CustomerId).IsRequired();
            entity.Property(e => e.MeterNumber).IsRequired();
            entity.HasMany(e => e.MeterReadings)
                .WithOne(e => e.Meter)
                .HasForeignKey(e => e.MeterId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
        });
        modelBuilder.Entity<MeterReading>(entity => {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Date).IsRequired();
            entity.Property(e => e.Reading).IsRequired();
            entity.Property(e => e.MeterId).IsRequired();
        });
    }
}