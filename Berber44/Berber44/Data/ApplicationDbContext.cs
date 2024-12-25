using Berber44.Models;
using Microsoft.EntityFrameworkCore;

namespace Berber44.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Salon> Salonlar { get; set; } = null!;
        public DbSet<Calisan> Calisanlar { get; set; } = null!;
        public DbSet<Islem> Islemler { get; set; } = null!;
        public DbSet<Randevu> Randevular { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;
        // Randevular için DbSet
        public DbSet<Randevu> Randevus { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Calisan ve Salon ilişkisi
            modelBuilder.Entity<Calisan>()
                .HasOne(c => c.Salon)
                .WithMany(s => s.Calisanlar)
                .HasForeignKey(c => c.SalonId)
                .OnDelete(DeleteBehavior.Restrict);

            // Islem ve Salon ilişkisi
            modelBuilder.Entity<Islem>()
                .HasOne(i => i.Salon)
                .WithMany(s => s.Islemler)
                .HasForeignKey(i => i.SalonId)
                .OnDelete(DeleteBehavior.Restrict);

            // Randevu ve Calisan ilişkisi
            modelBuilder.Entity<Randevu>()
                .HasOne(r => r.Calisan)
                .WithMany(c => c.Randevular)
                .HasForeignKey(r => r.CalisanId)
                .OnDelete(DeleteBehavior.Restrict);

            // İşlem - Randevu İlişkisi
            modelBuilder.Entity<Randevu>()
                .HasOne(r => r.Islem)
                .WithMany(i => i.Randevular)
                .HasForeignKey(r => r.IslemId)
                .OnDelete(DeleteBehavior.Restrict);

            // Admin kullanıcıyı oluştur ve hashlenmiş şifre ile ekle
            // Seed Admin User with plain-text password
            modelBuilder.Entity<User>().HasData(new User
            {
                Id = 1,
                Email = "b221210058@sakarya.edu.tr",
                Password = "sau", // Plain-text password
                Role = "Admin"
            });
        }
    }
}
