using System;
using System.Linq;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class EventController : Controller
    {
        // Veritabanı bağlantısı için context nesnesi
        private EtkinlikDBEntities1 db = new EtkinlikDBEntities1();

        // Etkinlik ana sayfası (görsel arayüzü döner)
        public ActionResult Index()
        {
            return View();
        }

        // Tüm aktif etkinlikleri JSON formatında döner
        [HttpGet]
        public JsonResult GetActiveEvents()
        {
            var events = db.Events
                .Where(e => (bool)e.IsActive) // Sadece aktif etkinlikler
                .OrderBy(e => e.EventDate)    // Tarihe göre sıralama
                .Select(e => new
                {
                    e.EventID,
                    Title = e.Title,
                    EventType = e.EventType,
                    EventDate = e.EventDate.ToString("yyyy-MM-dd"),
                    Quota = e.Quota,
                    Price = e.Price
                })
                .ToList();

            // Json verisi frontend tarafına gönderilir (örneğin JS ile kullanılmak üzere)
            return Json(events, JsonRequestBehavior.AllowGet);
        }

        // Yeni etkinlik eklemek için POST endpoint (AJAX ile çağrılabilir)
        [HttpPost]
        public JsonResult CreateEvent(Events newEvent)
        {
            if (!ModelState.IsValid) // Model doğrulama başarısızsa
                return Json(new { success = false, message = "Geçersiz veri." });

            newEvent.IsActive = true; // Yeni etkinlik varsayılan olarak aktif olur
            db.Events.Add(newEvent); // Veritabanına eklenir
            db.SaveChanges();

            return Json(new { success = true, message = "Etkinlik eklendi.", eventId = newEvent.EventID });
        }

        // Belirli bir etkinliği ID ile getir (düzenleme formunda kullanılabilir)
        [HttpGet]
        public JsonResult GetEvent(int id)
        {
            var ev = db.Events.Find(id);
            if (ev == null || ev.IsActive == false)
                return Json(new { success = false, message = "Etkinlik bulunamadı." }, JsonRequestBehavior.AllowGet);

            return Json(new
            {
                success = true,
                eventData = new
                {
                    ev.EventID,
                    Title = ev.Title,
                    EventType = ev.EventType,
                    EventDate = ev.EventDate.ToString("yyyy-MM-dd"),
                    Quota = ev.Quota,
                    Price = ev.Price
                }
            }, JsonRequestBehavior.AllowGet);
        }

        // Mevcut bir etkinliği güncelleme işlemi (AJAX POST)
        [HttpPost]
        public JsonResult EditEvent(Events updatedEvent)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, message = "Geçersiz veri." });

            var ev = db.Events.Find(updatedEvent.EventID);
            if (ev == null)
                return Json(new { success = false, message = "Etkinlik bulunamadı." });

            // Güncellenen alanlar
            ev.Title = updatedEvent.Title;
            ev.EventType = updatedEvent.EventType;
            ev.EventDate = updatedEvent.EventDate;
            ev.Quota = updatedEvent.Quota;
            ev.Price = updatedEvent.Price;
            ev.IsActive = true;

            db.SaveChanges(); // Değişiklikleri kaydet

            return Json(new { success = true, message = "Etkinlik güncellendi." });
        }

        // Etkinliği silmek yerine pasif duruma getirir (mantıksal silme)
        [HttpPost]
        public JsonResult DeleteEvent(int id)
        {
            var ev = db.Events.Find(id);
            if (ev == null)
                return Json(new { success = false, message = "Etkinlik bulunamadı." });

            ev.IsActive = false; // Silinmedi, sadece aktiflik durumu kapatıldı
            db.SaveChanges();

            return Json(new { success = true, message = "Etkinlik silindi." });
        }
    }
}
