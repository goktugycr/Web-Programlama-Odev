using Microsoft.AspNetCore.Http;

namespace Berber44.Models
{
    public class AIViewModel
    {
        // Fotoğraf yükleme için dosya
        public IFormFile Photo { get; set; }

        // OpenAI API'sinden gelen sonuç
        public string Result { get; set; }
    }
}
