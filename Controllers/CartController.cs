using System;
using System.Linq;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class CartController : Controller
    {
        // Veritabanı bağlantısı (Entity Framework context)
        private EtkinlikDBEntities1 db = new EtkinlikDBEntities1();

        // Kullanıcının sepetini görüntüleyen sayfa
        public ActionResult Index()
        {
            var userId = GetCurrentUserId(); // Aktif kullanıcı ID'sini al
            var cartItems = db.CartItems.Where(c => c.UserID == userId).ToList(); // Kullanıcının sepet öğelerini al
            return View(cartItems); // View'a gönder
        }

        // Sepete ürün (etkinlik bileti) ekleme işlemi
        [HttpPost]
        public JsonResult AddToCart(int eventId, int quantity)
        {
            var userId = GetCurrentUserId(); // Kullanıcı kimliği alınır
            if (userId == null)
                return Json(new { success = false, message = "Giriş yapmalısınız." }); // Kullanıcı oturum açmamışsa

            var ev = db.Events.Find(eventId); // Etkinlik veritabanından bulunur
            if (ev == null || ev.IsActive != true)
                return Json(new { success = false, message = "Etkinlik bulunamadı." });

            if (ev.Quota < quantity) // Yeterli kontenjan kontrolü
                return Json(new { success = false, message = "Yeterli kontenjan yok." });

            // Aynı etkinlikten daha önce sepete eklenmiş mi kontrol edilir
            var existingItem = db.CartItems.FirstOrDefault(c => c.UserID == userId && c.EventID == eventId);
            if (existingItem != null)
            {
                existingItem.Quantity += quantity; // Varsa miktar artırılır
            }
            else
            {
                // Yoksa yeni kayıt olarak sepete eklenir
                db.CartItems.Add(new CartItems
                {
                    UserID = userId.Value,
                    EventID = eventId,
                    Quantity = quantity,
                    TicketType = "Standart" // Varsayılan bilet tipi
                });
            }

            ev.Quota -= quantity; // Etkinlik kontenjanı düşürülür
            db.SaveChanges(); // Veritabanına değişiklikler kaydedilir

            return Json(new { success = true, message = "Sepete eklendi." });
        }

        // Sepetten ürün kaldırma işlemi
        [HttpPost]
        public JsonResult RemoveFromCart(int cartItemId)
        {
            var userId = GetCurrentUserId(); // Oturumdaki kullanıcı ID'si
            var item = db.CartItems.FirstOrDefault(c => c.CartItemID == cartItemId && c.UserID == userId);
            if (item == null)
                return Json(new { success = false, message = "Ürün bulunamadı." });

            var ev = db.Events.Find(item.EventID);
            if (ev != null)
            {
                ev.Quota += item.Quantity ?? 0; // Kontenjan iade edilir
            }

            db.CartItems.Remove(item); // Sepet öğesi silinir
            db.SaveChanges();

            return Json(new { success = true, message = "Sepetten kaldırıldı." });
        }

        // Sepeti satın alma (biletleri oluşturma) işlemi
        [HttpPost]
        public JsonResult Purchase()
        {
            var userId = GetCurrentUserId(); // Kullanıcı kimliği
            var cartItems = db.CartItems.Where(c => c.UserID == userId).ToList();

            if (!cartItems.Any())
                return Json(new { success = false, message = "Sepet boş." });

            // Her sepet öğesi için bilet oluşturulur
            foreach (var item in cartItems)
            {
                db.Tickets.Add(new Tickets
                {
                    UserID = userId.Value,
                    EventID = item.EventID,
                    TicketType = item.TicketType,
                    Price = db.Events.Find(item.EventID)?.Price, // Etkinlik fiyatı alınır
                    PurchasedAt = DateTime.Now
                });

                db.CartItems.Remove(item); // Sepetten kaldırılır
            }

            db.SaveChanges(); // Değişiklikler kaydedilir

            return Json(new { success = true, message = "Satın alma işlemi başarılı." });
        }

        // Yardımcı fonksiyon: oturumdaki kullanıcıyı getirir
        private int? GetCurrentUserId()
        {
            // Session'dan UserID çekilir, null dönebilir
            if (Session["UserID"] != null)
                return (int)Session["UserID"];
            return null;
        }
    }
}
