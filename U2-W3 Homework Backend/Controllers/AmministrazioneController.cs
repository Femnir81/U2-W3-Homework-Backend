using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using U2_W3_Homework_Backend.Models;

namespace U2_W3_Homework_Backend.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AmministrazioneController : Controller
    {
        private ModelDBContext db = new ModelDBContext();

        // GET: Pizze
        public ActionResult Index()
        {
            //var ListaPizze = db.Pizze.SqlQuery("Select * from Pizze").ToList();
            return View(db.Pizze.ToList());
        }

        // GET: Pizze/Details/5
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

        // GET: Pizze/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Pizze/Create
        // Per la protezione da attacchi di overposting, abilitare le proprietà a cui eseguire il binding. 
        // Per altri dettagli, vedere https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include ="ID,Nome,Prezzo,TempoPreparazione,Ingredienti")] Pizze pizze, HttpPostedFileBase FotoPizza)
        {
            if (ModelState.IsValid == true && FotoPizza != null)
            {
                pizze.Foto = FotoPizza.FileName;
                FotoPizza.SaveAs(Server.MapPath("/Content/Img/" + pizze.Foto));
                
                db.Pizze.Add(pizze);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.message = "Devi inserire un'immagine";
            return View(pizze);
        }

        // GET: Pizze/Edit/5
        public ActionResult Edit(int? id)
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

        // POST: Pizze/Edit/5
        // Per la protezione da attacchi di overposting, abilitare le proprietà a cui eseguire il binding. 
        // Per altri dettagli, vedere https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Pizze pizze, HttpPostedFileBase FotoPizza)
        {
            if (ModelState.IsValid)
            {
                Pizze pizzaInDb = db.Pizze.Find(pizze.ID);

                pizzaInDb.Prezzo = pizze.Prezzo;
                pizzaInDb.Ingredienti = pizze.Ingredienti;
                pizzaInDb.Nome = pizze.Nome;
                pizzaInDb.TempoPreparazione = pizze.TempoPreparazione;

                if (FotoPizza != null)
                {
                    pizzaInDb.Foto = FotoPizza.FileName;
                    FotoPizza.SaveAs(Server.MapPath("/Content/Img/" + pizze.Foto));
                }
               
                db.Entry(pizzaInDb).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(pizze);

            //if (ModelState.IsValid)
            //{
            //    Pizze pizzaInDb = db.Pizze.Find(pizze.ID);

            //    if (FotoPizza == null)
            //    {
            //        pizze.Foto = pizzaInDb.Foto;
            //    }
            //    else
            //    {
            //        pizze.Foto = FotoPizza.FileName;
            //    }
            //    ModelDBContext ctx = new ModelDBContext();
            //    ctx.Entry(pizze).State = EntityState.Modified;
            //    ctx.SaveChanges();
            //    return RedirectToAction("Index");
            //}
            //return View(pizze);
        }

        // GET: Pizze/Delete/5
        public ActionResult Delete(int? id)
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

        // POST: Pizze/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Pizze pizze = db.Pizze.Find(id);
            db.Pizze.Remove(pizze);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult ListaOrdini()
        {
            return View(db.Ordini.Where(x => x.OrdineConfermato == true).OrderByDescending(x => x.ID));
        }

        public ActionResult Consegnato(int id)
        {
            Ordini OrdineInDB = db.Ordini.Find(id);
            if (OrdineInDB.OrdineConsegnato)
            {
                OrdineInDB.OrdineConsegnato = false;
                db.Entry(OrdineInDB).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ListaOrdini");
            }
            OrdineInDB.OrdineConsegnato = true;
            db.Entry(OrdineInDB).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("ListaOrdini");
        }

        public ActionResult Riepilogo()
        {
            return View();
        }
        
        public JsonResult OrdiniConsegnati()
        {
            int TotOrdini = db.Ordini.Where(x => x.OrdineConsegnato == true).Count();
            return Json(TotOrdini, JsonRequestBehavior.AllowGet);
        }

        public JsonResult IncassoGiornata(DateTime data)
        {
            List<Ordini> ListaOrdini = db.Ordini.Where(x => x.DataOrdine == data && x.OrdineConsegnato == true).ToList();
            decimal totale = 0;
            foreach (Ordini item in ListaOrdini)
            {
                decimal costoOrdine = item.Quantita * item.Pizze.Prezzo;
                totale += costoOrdine;
            }
            return Json(totale, JsonRequestBehavior.AllowGet);
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
