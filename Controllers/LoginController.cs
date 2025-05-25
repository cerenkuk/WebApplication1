using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class LoginController : Controller
    {
        EtkinlikDBEntities1 entity =new EtkinlikDBEntities1();
        // GET: login
        public ActionResult Index()
        {
            return View();
        }
    }
}