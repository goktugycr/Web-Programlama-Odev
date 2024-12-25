using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Berber44.Controllers
{
    public class AIController : Controller
    {
        private readonly IConfiguration _configuration;

        public AIController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadPhoto(IFormFile photo)
        {
            if (photo == null || photo.Length == 0)
            {
                ViewBag.ErrorMessage = "Lütfen bir fotoğraf seçin.";
                return View("Upload");
            }

            try
            {
                var base64Image = await ConvertToBase64(photo);
                Console.WriteLine($"Base64 Görsel Verisi: {base64Image.Substring(0, 100)}...");

                var result = await AnalyzePhoto(base64Image);
                ViewBag.Result = result;
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Bir hata oluştu: " + ex.Message;
            }

            return View("Upload");
        }

        private async Task<string> ConvertToBase64(IFormFile photo)
        {
            using (var memoryStream = new System.IO.MemoryStream())
            {
                await photo.CopyToAsync(memoryStream);
                return System.Convert.ToBase64String(memoryStream.ToArray());
            }
        }

        private async Task<string> AnalyzePhoto(string base64Image)
        {
            var apiKey = _configuration["OpenAI:ApiKey"];
            var apiUrl = "https://api.openai.com/v1/chat/completions";

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

                var requestBody = new
                {
                    model = "gpt-4o-mini",
                    messages = new object[]
                    {
                        new { role = "system", content = "Sen bir saç uzmanısın ve fotoğraf analiz ediyorsun." },
                        new
                        {
                            role = "user",
                            content = new object[]
                            {
                                new { type = "text", text = "Sen bir saç uzmanısın ve öneri veriyorsun size şu saç rengi şu saç uzunluğu yakışır gibi." },
                                new { type = "image_url", image_url = new { url = $"data:image/jpeg;base64,{base64Image}" } }
                            }
                        }
                    },
                    max_tokens = 300
                };

                var jsonContent = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");
                var response = await client.PostAsync(apiUrl, jsonContent);
                var responseString = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    dynamic jsonResponse = JsonConvert.DeserializeObject(responseString);
                    return jsonResponse.choices[0]?.message?.content ?? "Sonuç bulunamadı.";
                }
                else
                {
                    throw new Exception($"API isteği başarısız oldu. Hata: {responseString}");
                }
            }
        }
    }
}
