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
    public class HanhlisController : Controller
    {
        private web_banvemaybayEntities db = new web_banvemaybayEntities();

        // GET: Admin/Hanhlis
        public ActionResult Index()
        {
            return View(db.Hanhli.ToList());
        }

        // GET: Admin/Hanhlis/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Hanhli hanhli = db.Hanhli.Find(id);
            if (hanhli == null)
            {
                return HttpNotFound();
            }
            return View(hanhli);
        }

        // GET: Admin/Hanhlis/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Hanhlis/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IDhanhli,Kg,Giatien")] Hanhli hanhli)
        {
            if (ModelState.IsValid)
            {
                db.Hanhli.Add(hanhli);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(hanhli);
        }

        // GET: Admin/Hanhlis/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Hanhli hanhli = db.Hanhli.Find(id);
            if (hanhli == null)
            {
                return HttpNotFound();
            }
            return View(hanhli);
        }

        // POST: Admin/Hanhlis/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IDhanhli,Kg,Giatien")] Hanhli hanhli)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hanhli).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(hanhli);
        }

        // GET: Admin/Hanhlis/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Hanhli hanhli = db.Hanhli.Find(id);
            if (hanhli == null)
            {
                return HttpNotFound();
            }
            return View(hanhli);
        }

        // POST: Admin/Hanhlis/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Hanhli hanhli = db.Hanhli.Find(id);
            db.Hanhli.Remove(hanhli);
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
