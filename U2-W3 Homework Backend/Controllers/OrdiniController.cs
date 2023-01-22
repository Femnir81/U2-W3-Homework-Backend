using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using U2_W3_Homework_Backend.Models;

namespace U2_W3_Homework_Backend.Controllers
{
    [Authorize]
    public class OrdiniController : Controller
    {
        private ModelDBContext db = new ModelDBContext();

        // GET: Ordini/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ordini ordini = db.Ordini.Find(id);
            if (ordini == null)
            {
                return HttpNotFound();
            }
            ViewBag.IDPizze = new SelectList(db.Pizze, "ID", "Nome", ordini.IDPizze);
            ViewBag.IDUtenti = new SelectList(db.Utenti, "ID", "Username", ordini.IDUtenti);
            return View(ordini);
        }

        // POST: Ordini/Edit/5
        // Per la protezione da attacchi di overposting, abilitare le proprietà a cui eseguire il binding. 
        // Per altri dettagli, vedere https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Quantita,Nota,IDPizze")] Ordini ordini)
        {
            if (ModelState.IsValid)
            {
                //Utenti utente = db.Utenti.Where(m => m.Username == User.Identity.Name).First();
                Ordini OrdiniInDB = db.Ordini.Find(ordini.ID);
                OrdiniInDB.Quantita = ordini.Quantita;
                OrdiniInDB.Nota = ordini.Nota;
                OrdiniInDB.IDPizze = ordini.IDPizze;
                db.Entry(OrdiniInDB).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Carrello");
            }
            ViewBag.IDPizze = new SelectList(db.Pizze, "ID", "Nome", ordini.IDPizze);
            ViewBag.IDUtenti = new SelectList(db.Utenti, "ID", "Username", ordini.IDUtenti);
            return View(ordini);
        }

        // GET: Ordini/Delete/5
        public ActionResult Delete(int? id)
        {
            Utenti utente = db.Utenti.Where(m => m.Username == User.Identity.Name).First();
            Ordini ordini = db.Ordini.Find(id);
            db.Ordini.Remove(ordini);
            db.SaveChanges();
            return RedirectToAction("Carrello");
        }

        public ActionResult PartialViewCreateByClient()
        {
            return PartialView("_PartialViewCreateByClient");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PartialViewCreateByClient(Ordini ordini, int id)
        {
            if (ModelState.IsValid && ordini.Quantita > 0)
            {
            Utenti utente = db.Utenti.Where(m => m.Username == User.Identity.Name).First();
            //Pizze pizze = db.Pizze.Find(id);
            ordini.IndirizzoSpedizione = " ";
            ordini.IDPizze = id;
            ordini.IDUtenti = utente.ID;
            db.Ordini.Add(ordini);
            db.SaveChanges();
            return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Details", "Home", db.Pizze.Find(id));           
        }

        public ActionResult Carrello()
        {
            Utenti utente = db.Utenti.Where(m => m.Username == User.Identity.Name).FirstOrDefault();
            return View(db.Ordini.Where(x => x.IDUtenti == utente.ID && x.OrdineConfermato == false));
        }

        public ActionResult PartialViewCheckout()
        {
            return PartialView("_PartialViewCheckout");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PartialViewCheckout([Bind(Include = "IndirizzoSpedizione")] Ordini ordini)
        {
            if (ModelState.IsValid == true && ordini.IndirizzoSpedizione != null)
            {
                Utenti utente = db.Utenti.Where(m => m.Username == User.Identity.Name).FirstOrDefault();
                List<Ordini> Carrello = db.Ordini.Where(x => x.IDUtenti == utente.ID && x.OrdineConfermato == false).ToList();
                foreach (Ordini item in Carrello)
                {
                    item.IndirizzoSpedizione = ordini.IndirizzoSpedizione;
                    item.DataOrdine = DateTime.Now;
                    item.OrdineConfermato = true;
                    db.Entry(item).State = EntityState.Modified;
                    db.SaveChanges();
                }
                return RedirectToAction("Index", "Home");
            }
            return PartialView("_PartialViewCheckout");
        }

        public ActionResult PartialViewOrdiniInConsegna()
        {
            Utenti utente = db.Utenti.Where(m => m.Username == User.Identity.Name).FirstOrDefault();
            return PartialView("_PartialViewOrdiniInConsegna", db.Ordini.Where(x => x.IDUtenti == utente.ID && x.OrdineConfermato == true && x.OrdineConsegnato == false));
        }

        public ActionResult PartialViewOrdiniConsegnati()
        {
            Utenti utente = db.Utenti.Where(m => m.Username == User.Identity.Name).FirstOrDefault();
            return PartialView("_PartialViewOrdiniConsegnati", db.Ordini.Where(x => x.IDUtenti == utente.ID && x.OrdineConfermato == true && x.OrdineConsegnato == true));
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
