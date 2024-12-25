using System.ComponentModel.DataAnnotations;
namespace Berber44.Models
{
    public class Salon
    {
        public int Id { get; set; }
        public string Ad { get; set; } = string.Empty;

        // Yeni özellikler
        public string Tur { get; set; } = string.Empty; // Bay/Bayan
        public string CalismaSaatleri { get; set; } = string.Empty; // Çalışma saatleri

        // İlişkiler
        public ICollection<Calisan> Calisanlar { get; set; } = new List<Calisan>();
        public ICollection<Islem> Islemler { get; set; } = new List<Islem>();
       
    }
}

