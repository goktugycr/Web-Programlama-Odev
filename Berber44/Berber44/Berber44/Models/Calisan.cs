using Berber44.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Berber44.Models
{
    public class Calisan
    {
        public int Id { get; set; }
        public string Ad { get; set; } = string.Empty;
        public string Soyad { get; set; } = string.Empty;
        public string UzmanlikAlanlari { get; set; } = string.Empty;
        public int SalonId { get; set; }
        public Salon? Salon { get; set; }
    }

}
