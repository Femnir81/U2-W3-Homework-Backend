using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using U2_W3_Homework_Backend.Models;

namespace U2_W3_Homework_Backend.Controllers
{
    public class HomeController : Controller
    {
        ModelDBContext db = new ModelDBContext();
        public ActionResult Index()
        {
            var ListaPizze = db.Pizze.ToList();
            return View(ListaPizze);
        }
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pizze pizze = db.Pizze.Find(id);
            if (pizze == null)
            {
                return HttpNotFound();
            }
            return View(pizze);
        }
    }
}