using Berber44.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Berber44.Models
{
    public class Islem
    {

        public int Id { get; set; }

        public string Ad { get; set; } = string.Empty;

        public int Sure { get; set; } // Dakika cinsinden işlem süresi

        public decimal Ucret { get; set; } // İşlem ücreti

        // Foreign key for Salon
        public int SalonId { get; set; }

        public Salon? Salon { get; set; }
        // Navigation property for the related Randevular
    }
}

