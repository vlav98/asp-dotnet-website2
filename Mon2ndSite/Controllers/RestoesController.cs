using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Mon2ndSite.Models;

namespace Mon2ndSite.Controllers
{
    public class RestoesController : Controller
    {
        private BddContext db = new BddContext();

        // GET: Restoes
        public ActionResult Index()
        {
            return View(db.Restos.ToList());
        }

        // GET: Restoes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Resto resto = db.Restos.Find(id);
            if (resto == null)
            {
                return HttpNotFound();
            }
            return View(resto);
        }

        // GET: Restoes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Restoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nom,Telephone")] Resto resto)
        {
            if (ModelState.IsValid)
            {
                db.Restos.Add(resto);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(resto);
        }

        // GET: Restoes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Resto resto = db.Restos.Find(id);
            if (resto == null)
            {
                return HttpNotFound();
            }
            return View(resto);
        }

        // POST: Restoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nom,Telephone")] Resto resto)
        {
            if (ModelState.IsValid)
            {
                db.Entry(resto).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(resto);
        }

        // GET: Restoes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Resto resto = db.Restos.Find(id);
            if (resto == null)
            {
                return HttpNotFound();
            }
            return View(resto);
        }

        // POST: Restoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Resto resto = db.Restos.Find(id);
            db.Restos.Remove(resto);
            db.SaveChanges();
            return RedirectToAction("Index");
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
