using System.ComponentModel.DataAnnotations;

namespace Berber44.Models
{
    public class Randevu
    {
        public int Id { get; set; }

   
        public int CalisanId { get; set; }
        public Calisan? Calisan { get; set; }

     
        public int IslemId { get; set; }
        public Islem? Islem { get; set; }

       
        public string UserEmail { get; set; } = string.Empty; // Kullanıcı e-posta adresi


        [FutureDate(ErrorMessage = "Randevu tarihi geçmiş bir tarih olamaz.")]

        
        [DataType(DataType.Currency)] // Ücret için para birimi formatı
       
        public decimal? Ucret { get; set; }

        public DateTime TarihSaat { get; set; }

        public string Durum { get; set; } = "Bekliyor"; // Varsayılan durum

    }

    // Custom validation attribute for future dates
    public class FutureDateAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is DateTime date)
            {
                if (date < DateTime.UtcNow)
                {
                    return new ValidationResult(ErrorMessage ?? "Tarih geçmiş bir tarih olamaz.");
                }
            }
            return ValidationResult.Success;
        }
    }
}
