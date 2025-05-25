using System;                         // Temel sistem işlevlerini sağlar.
using System.Collections.Generic;     // Koleksiyonlar (List, Dictionary, vb.) için gerekli.
using System.Linq;                    // LINQ sorguları için kullanılır.
using System.Web;                     // HTTP özellikleri (request, response, vb.) için kullanılır.
using System.Web.Mvc;                 // ASP.NET MVC framework'ü için gerekli temel sınıfları içerir.

namespace WebApplication1.Controllers  // Bu sınıfın ait olduğu namespace (proje yapısı).
{
    // AccountController sınıfı, kullanıcı hesabı işlemleriyle ilgili HTTP isteklerini işler.
    public class AccountController : Controller
    {
        // SignIn isimli action metodu, kullanıcıya giriş yapma sayfasını gösterir.
        // Kullanıcı giriş yap butonuna bastığında bu metot çalışır ve ilgili view (görünüm) döndürülür.
        public ActionResult SignIn()
        {
            // SignIn.cshtml adlı view dosyasını tarayıcıya render eder.
            return View();
        }

        // SignUp isimli action metodu, kullanıcıya kayıt olma sayfasını gösterir.
        // Kullanıcı kayıt ol butonuna bastığında bu metot çalışır ve ilgili view (görünüm) döndürülür.
        public ActionResult SignUp()
        {
            // SignUp.cshtml adlı view dosyasını tarayıcıya render eder.
            return View();
        }

        // Bu controller'a daha sonra POST işlemleri, model binding, validasyon gibi özellikler de eklenebilir.
    }
}
