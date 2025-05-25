using System.Linq;                          // LINQ sorguları yazabilmek için gerekli.
using System.Web.Mvc;                       // ASP.NET MVC Controller sınıfı ve ActionResult için gerekli.
using WebApplication1.Models;              // Veritabanı modeli ve entity sınıflarına erişim için gerekli.

namespace WebApplication1.Controllers
{
    // AdminController sınıfı, yönetici işlemlerini yöneten bir denetleyicidir.
    public class AdminController : Controller
    {
        // Veritabanı bağlantısını sağlayan entity context nesnesi.
        private EtkinlikDBEntities1 db = new EtkinlikDBEntities1();

        // GET: Admin/PendingUsers
        // Onay bekleyen kullanıcıları (IsApproved = false olanlar) veritabanından alır ve View'e gönderir.
        public ActionResult PendingUsers()
        {
            // Onaylanmamış kullanıcılar filtrelenir ve liste haline getirilir.
            var pendingUsers = db.Users.Where(u => u.IsApproved == false).ToList();

            // pendingUsers listesini ilgili view sayfasına gönderir (PendingUsers.cshtml).
            return View(pendingUsers);
        }

        // GET: Admin/ApproveUser/id
        // Parametre olarak verilen id'ye sahip kullanıcıyı bulur ve onaylar (IsApproved = true yapılır).
        public ActionResult ApproveUser(int id)
        {
            // ID'ye göre kullanıcıyı veritabanından bulur.
            var user = db.Users.Find(id);

            // Eğer kullanıcı bulunursa onay işlemi gerçekleştirilir.
            if (user != null)
            {
                user.IsApproved = true;  // Kullanıcı onaylanır.
                db.SaveChanges();        // Değişiklikler veritabanına kaydedilir.
            }

            // İşlem tamamlandıktan sonra tekrar onay bekleyen kullanıcılar listesine yönlendirilir.
            return RedirectToAction("PendingUsers");
        }
    }
}
