using Berber44.Models;
using Microsoft.EntityFrameworkCore;

namespace Berber44.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Salon> Salonlar { get; set; } = null!;
        public DbSet<Calisan> Calisanlar { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Calisan ve Salon ilişkisi
            modelBuilder.Entity<Calisan>()
                .HasOne(c => c.Salon)
                .WithMany(s => s.Calisanlar)
                .HasForeignKey(c => c.SalonId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
