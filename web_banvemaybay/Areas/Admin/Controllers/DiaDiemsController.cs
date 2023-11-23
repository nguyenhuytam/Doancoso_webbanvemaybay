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
    public class DiaDiemsController : Controller
    {
        private web_banvemaybayEntities db = new web_banvemaybayEntities();

        // GET: Admin/DiaDiems
        public ActionResult Index()
        {
            return View(db.DiaDiem.ToList());
        }

        // GET: Admin/DiaDiems/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DiaDiem diaDiem = db.DiaDiem.Find(id);
            if (diaDiem == null)
            {
                return HttpNotFound();
            }
            return View(diaDiem);
        }

        // GET: Admin/DiaDiems/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/DiaDiems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,DiaDiem1")] DiaDiem diaDiem)
        {
            if (ModelState.IsValid)
            {
                db.DiaDiem.Add(diaDiem);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(diaDiem);
        }

        // GET: Admin/DiaDiems/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DiaDiem diaDiem = db.DiaDiem.Find(id);
            if (diaDiem == null)
            {
                return HttpNotFound();
            }
            return View(diaDiem);
        }

        // POST: Admin/DiaDiems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,DiaDiem1")] DiaDiem diaDiem)
        {
            if (ModelState.IsValid)
            {
                db.Entry(diaDiem).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(diaDiem);
        }

        // GET: Admin/DiaDiems/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DiaDiem diaDiem = db.DiaDiem.Find(id);
            if (diaDiem == null)
            {
                return HttpNotFound();
            }
            return View(diaDiem);
        }

        // POST: Admin/DiaDiems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DiaDiem diaDiem = db.DiaDiem.Find(id);
            db.DiaDiem.Remove(diaDiem);
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
