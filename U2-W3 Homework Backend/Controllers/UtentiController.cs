using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using U2_W3_Homework_Backend.Models;

namespace U2_W3_Homework_Backend.Controllers
{
    public class UtentiController : Controller
    {
        private ModelDBContext db = new ModelDBContext();

        // GET: Utenti/Create
        public ActionResult Register()
        {
            return View();
        }

        // POST: Utenti/Create
        // Per la protezione da attacchi di overposting, abilitare le proprietà a cui eseguire il binding. 
        // Per altri dettagli, vedere https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register([Bind(Include = "ID,Username,Password,Nome,Cognome")] Utenti utenti)
        {
            if (ModelState.IsValid == true && db.Utenti.Where(x => x.Username == utenti.Username).Count() == 0)
            {
                utenti.Ruolo = "Client";
                db.Utenti.Add(utenti);
                db.SaveChanges();
                return RedirectToAction("Index","Home");
            }
            return View(utenti);
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Utenti utenti)
        {
            if (db.Utenti.Where(x => x.Username == utenti.Username && x.Password == utenti.Password).Count() == 1)
            {
                FormsAuthentication.SetAuthCookie(utenti.Username, false);
                return Redirect(FormsAuthentication.DefaultUrl);
            }
            return View();
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return Redirect(FormsAuthentication.LoginUrl);
        }

        [Authorize]
        public ActionResult Profilo()
        {
            Utenti utente = db.Utenti.Where(m => m.Username == User.Identity.Name).FirstOrDefault();
            if (utente == null)
            {
                return Redirect(FormsAuthentication.LoginUrl);
            }
            return View(utente);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
