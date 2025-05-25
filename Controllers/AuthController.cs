using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    // Kimlik doğrulama işlemlerini yöneten controller
    public class AuthController : Controller
    {
        // Veritabanı ile iletişim kurmak için Entity Framework context nesnesi
        private EtkinlikDBEntities1 db = new EtkinlikDBEntities1();

        // GİRİŞ İŞLEMİ (LOGIN)
        [HttpPost]
        public JsonResult Login(string email, string password)
        {
            // Girilen şifre hash'lenir ve veritabanındaki hash ile karşılaştırılır
            var hashedPassword = HashPassword(password);

            // Kullanıcıyı email ve hash'e göre bul
            var user = db.Users.FirstOrDefault(u => u.Email == email && u.PasswordHash == hashedPassword);

            if (user == null)
            {
                // Kullanıcı bulunamazsa hata mesajı döner
                return Json(new { success = false, message = "Email veya şifre yanlış." });
            }

            if (user.IsApproved != true)
            {
                // Kullanıcı henüz onaylanmadıysa hata mesajı döner
                return Json(new { success = false, message = "Hesabınız henüz onaylanmadı." });
            }

            // Giriş başarılıysa, kullanıcıyı oturumda tanır
            FormsAuthentication.SetAuthCookie(user.Email, false);

            return Json(new { success = true, message = "Giriş başarılı!", userName = user.Email });
        }

        // KAYIT OLMA (REGISTER)
        [HttpPost]
        public JsonResult Register(string name, string surname, string email, string password)
        {
            try
            {
                // Aynı email ile daha önce kayıt yapılmış mı kontrol et
                var existingUser = db.Users.FirstOrDefault(u => u.Email == email);
                if (existingUser != null)
                {
                    return Json(new { success = false, message = "Bu email zaten kayıtlı." });
                }

                // Yeni kullanıcı nesnesi oluşturulur
                Users newUser = new Users
                {
                    // İsim ve soyisim alanları kullanılacaksa aşağıdaki satırlar açılır:
                    // Name = name,
                    // Surname = surname,
                    Email = email,
                    PasswordHash = HashPassword(password), // Şifre hash'lenerek kaydedilir
                    Role = "User",
                    IsApproved = false, // Yeni kullanıcı onaysız başlar
                    CreatedAt = DateTime.Now
                };

                // Kullanıcıyı veritabanına ekle ve kaydet
                db.Users.Add(newUser);
                db.SaveChanges();

                return Json(new { success = true, message = "Kayıt başarılı! Yönetici onayı bekleniyor." });
            }
            catch (Exception ex)
            {
                // Hata durumunda hata mesajı döndürülür
                return Json(new { success = false, message = "Kayıt işlemi sırasında hata oluştu: " + ex.Message });
            }
        }

        // ŞİFRE DEĞİŞTİRME (CHANGE PASSWORD)
        [HttpPost]
        public JsonResult ChangePassword(int userId, string oldPassword, string newPassword)
        {
            // Kullanıcı ID'sine göre kullanıcıyı getir
            var user = db.Users.FirstOrDefault(u => u.UserID == userId);

            if (user == null)
            {
                return Json(new { success = false, message = "Kullanıcı bulunamadı." });
            }

            // Girilen mevcut şifre hash'lenip kontrol edilir
            if (user.PasswordHash != HashPassword(oldPassword))
            {
                return Json(new { success = false, message = "Mevcut şifre yanlış." });
            }

            // Yeni şifre hash'lenerek kaydedilir
            user.PasswordHash = HashPassword(newPassword);
            db.SaveChanges();

            return Json(new { success = true, message = "Şifre başarıyla değiştirildi." });
        }

        // Basit SHA256 hash fonksiyonu (daha güvenli sistemlerde bcrypt önerilir)
        private string HashPassword(string password)
        {
            using (var sha = System.Security.Cryptography.SHA256.Create())
            {
                var bytes = System.Text.Encoding.UTF8.GetBytes(password);
                var hash = sha.ComputeHash(bytes);
                return Convert.ToBase64String(hash); // Hash sonucu base64 olarak döner
            }
        }
    }
}
