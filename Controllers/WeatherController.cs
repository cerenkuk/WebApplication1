using System.Configuration;         // Web.config'den API anahtarını okumak için
using System.Net.Http;             // HTTP istekleri göndermek için
using System.Threading.Tasks;      // Asenkron metodlar için
using System.Web.Mvc;              // MVC controller için
using Newtonsoft.Json.Linq;        // JSON verisini okumak için (Newtonsoft kütüphanesi)

namespace WebApplication1.Controllers
{
    public class WeatherController : Controller
    {
        // Bu action metodu, belirli bir şehir için hava durumu sorgulaması yapar.
        // Örnek: /Weather/Check?city=Istanbul
        public async Task<ActionResult> Check(string city)
        {
            // Şehir adı boş girilmişse hata mesajı döndür
            if (string.IsNullOrEmpty(city))
                return Json(new { success = false, message = "Şehir bilgisi boş olamaz." }, JsonRequestBehavior.AllowGet);

            // Web.config'den API anahtarı alınır
            string apiKey = ConfigurationManager.AppSettings["OpenWeatherApiKey"];

            // OpenWeatherMap API'sine istek URL'si hazırlanır
            string url = $"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={apiKey}&units=metric";

            // HTTP istemcisi ile API'ye GET isteği gönderilir
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(url);

                // API başarılı cevap dönmezse hata mesajı döndürülür
                if (!response.IsSuccessStatusCode)
                {
                    return Json(new { success = false, message = "Şehir bulunamadı veya API hatası." }, JsonRequestBehavior.AllowGet);
                }

                // API'den dönen içerik okunur ve JSON objesine dönüştürülür
                var content = await response.Content.ReadAsStringAsync();
                var json = JObject.Parse(content);

                // Hava durumu verisi JSON içinden çekilir
                string weather = json["weather"]?[0]?["main"]?.ToString();

                // Varsayılan olarak olumlu durum
                string status = "Etkinlik planlanabilir";

                // Eğer yağmur, kar veya fırtına varsa dış mekan için uygun olmadığı belirtilir
                if (weather == "Rain" || weather == "Snow" || weather == "Thunderstorm")
                {
                    status = "Etkinlik dış mekan için uygun değil";
                }

                // Kullanıcıya JSON formatında detaylı bilgi döndürülür
                return Json(new
                {
                    success = true,
                    city = city,
                    weather = weather,
                    status = status
                }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
