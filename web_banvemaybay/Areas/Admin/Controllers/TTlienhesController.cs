using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using web_banvemaybay.Models;

namespace web_banvemaybay.Areas.Admin.Controllers
{
    public class TTlienhesController : Controller
    {
        private web_banvemaybayEntities db = new web_banvemaybayEntities();

        // GET: Admin/TTlienhes
        public ActionResult Index()
        {
            return View(db.TTlienhe.ToList());
        }

        // GET: Admin/TTlienhes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TTlienhe tTlienhe = db.TTlienhe.Find(id);
            if (tTlienhe == null)
            {
                return HttpNotFound();
            }
            return View(tTlienhe);
        }

        // GET: Admin/TTlienhes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/TTlienhes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IDlienhe,FullName,Email,Phone")] TTlienhe tTlienhe)
        {
            if (ModelState.IsValid)
            {
                db.TTlienhe.Add(tTlienhe);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tTlienhe);
        }

        // GET: Admin/TTlienhes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TTlienhe tTlienhe = db.TTlienhe.Find(id);
            if (tTlienhe == null)
            {
                return HttpNotFound();
            }
            return View(tTlienhe);
        }

        // POST: Admin/TTlienhes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IDlienhe,FullName,Email,Phone")] TTlienhe tTlienhe)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tTlienhe).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tTlienhe);
        }

        // GET: Admin/TTlienhes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TTlienhe tTlienhe = db.TTlienhe.Find(id);
            if (tTlienhe == null)
            {
                return HttpNotFound();
            }
            return View(tTlienhe);
        }

        // POST: Admin/TTlienhes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TTlienhe tTlienhe = db.TTlienhe.Find(id);
            db.TTlienhe.Remove(tTlienhe);
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
