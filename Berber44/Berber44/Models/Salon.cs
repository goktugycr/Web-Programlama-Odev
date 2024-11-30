// Models/Salon.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Berber44.Models
{
    public class Salon
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Ad { get; set; }

        [Required]
        public string Tur { get; set; } // Kuaför veya Berber

        public string CalismaSaatleri { get; set; }
    }
}

