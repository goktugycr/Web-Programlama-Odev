// Data/ApplicationDbContext.cs
using Berber44.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Berber44.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Salon> Salonlar { get; set; }
        public DbSet<Calisan> Calisanlar { get; set; }

    }
}
