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
    public class DattruocsController : Controller
    {
        private web_banvemaybayEntities db = new web_banvemaybayEntities();

        // GET: Admin/Dattruocs
        public ActionResult Index()
        {
            var dattruoc = db.Dattruoc.Include(d => d.TaiKhoan);
            return View(dattruoc.ToList());
        }

        // GET: Admin/Dattruocs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dattruoc dattruoc = db.Dattruoc.Find(id);
            if (dattruoc == null)
            {
                return HttpNotFound();
            }
            return View(dattruoc);
        }

        // GET: Admin/Dattruocs/Create
        public ActionResult Create()
        {
            ViewBag.IDtaikhoan = new SelectList(db.TaiKhoan, "IDtaikhoan", "Password");
            return View();
        }

        // POST: Admin/Dattruocs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IDdattruoc,NgayDat,Tinhtrang,IDtaikhoan")] Dattruoc dattruoc)
        {
            if (ModelState.IsValid)
            {
                db.Dattruoc.Add(dattruoc);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IDtaikhoan = new SelectList(db.TaiKhoan, "IDtaikhoan", "Password", dattruoc.IDtaikhoan);
            return View(dattruoc);
        }

        // GET: Admin/Dattruocs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dattruoc dattruoc = db.Dattruoc.Find(id);
            if (dattruoc == null)
            {
                return HttpNotFound();
            }
            ViewBag.IDtaikhoan = new SelectList(db.TaiKhoan, "IDtaikhoan", "Password", dattruoc.IDtaikhoan);
            return View(dattruoc);
        }

        // POST: Admin/Dattruocs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IDdattruoc,NgayDat,Tinhtrang,IDtaikhoan")] Dattruoc dattruoc)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dattruoc).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IDtaikhoan = new SelectList(db.TaiKhoan, "IDtaikhoan", "Password", dattruoc.IDtaikhoan);
            return View(dattruoc);
        }

        // GET: Admin/Dattruocs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dattruoc dattruoc = db.Dattruoc.Find(id);
            if (dattruoc == null)
            {
                return HttpNotFound();
            }
            return View(dattruoc);
        }

        // POST: Admin/Dattruocs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Dattruoc dattruoc = db.Dattruoc.Find(id);
            db.Dattruoc.Remove(dattruoc);
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
