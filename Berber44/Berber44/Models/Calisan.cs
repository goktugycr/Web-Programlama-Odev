using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Berber44.Models
{
    public class Calisan
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Ad { get; set; }

        [Required]
        [StringLength(100)]
        public string Soyad { get; set; }

        [Required]
        [ForeignKey("Salon")]
        public int SalonId { get; set; } // Hangi salonda çalıştığını tutar

        public Salon Salon { get; set; } // İlgili salon bilgisi
    }
}
