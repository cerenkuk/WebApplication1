using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebApplication1
{
    public class RouteConfig
    {
        // Uygulama başladığında çalışacak olan route konfigürasyonu bu metod içerisinde tanımlanır.
        public static void RegisterRoutes(RouteCollection routes)
        {
            // .axd uzantılı kaynak dosyalarına (örneğin Trace.axd) gelen istekleri routing sisteminden hariç tutar.
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // Varsayılan route tanımı: 
            // URL yapısı şu şekildedir: /{controller}/{action}/{id}
            // Eğer URL'de controller belirtilmemişse "Account" controller'ı kullanılır.
            // Eğer action belirtilmemişse "SignIn" action'ı çalıştırılır.
            // "id" parametresi opsiyoneldir.
            routes.MapRoute(
                name: "Default", // Route'a verilen isim
                url: "{controller}/{action}/{id}", // URL kalıbı
                defaults: new { controller = "Account", action = "SignIn", id = UrlParameter.Optional } // Varsayılan değerler
            );
        }
    }
}
