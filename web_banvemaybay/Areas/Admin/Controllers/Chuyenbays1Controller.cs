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
    public class Chuyenbays1Controller : Controller
    {
        private web_banvemaybayEntities db = new web_banvemaybayEntities();

        // GET: Admin/Chuyenbays1
        public ActionResult Index()
        {
            var chuyenbay = db.Chuyenbay.Include(c => c.HangHK);
            return View(chuyenbay.ToList());
        }

        // GET: Admin/Chuyenbays1/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Chuyenbay chuyenbay = db.Chuyenbay.Find(id);
            if (chuyenbay == null)
            {
                return HttpNotFound();
            }
            return View(chuyenbay);
        }

        // GET: Admin/Chuyenbays1/Create
        public ActionResult Create()
        {
            ViewBag.IDhang = new SelectList(db.HangHK, "IDhang", "TenHang");
            return View();
        }

        // POST: Admin/Chuyenbays1/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IDchuyenbay,Diadiemdi,Diadiemden,Ngaydi,Ngayve,Giatien,IDhang,PhoThong,ThuongGia")] Chuyenbay chuyenbay)
        {
            if (ModelState.IsValid)
            {
                db.Chuyenbay.Add(chuyenbay);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IDhang = new SelectList(db.HangHK, "IDhang", "TenHang", chuyenbay.IDhang);
            return View(chuyenbay);
        }

        // GET: Admin/Chuyenbays1/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Chuyenbay chuyenbay = db.Chuyenbay.Find(id);
            if (chuyenbay == null)
            {
                return HttpNotFound();
            }
            ViewBag.IDhang = new SelectList(db.HangHK, "IDhang", "TenHang", chuyenbay.IDhang);
            return View(chuyenbay);
        }

        // POST: Admin/Chuyenbays1/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IDchuyenbay,Diadiemdi,Diadiemden,Ngaydi,Ngayve,Giatien,IDhang,PhoThong,ThuongGia")] Chuyenbay chuyenbay)
        {
            if (ModelState.IsValid)
            {
                db.Entry(chuyenbay).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IDhang = new SelectList(db.HangHK, "IDhang", "TenHang", chuyenbay.IDhang);
            return View(chuyenbay);
        }

        // GET: Admin/Chuyenbays1/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Chuyenbay chuyenbay = db.Chuyenbay.Find(id);
            if (chuyenbay == null)
            {
                return HttpNotFound();
            }
            return View(chuyenbay);
        }

        // POST: Admin/Chuyenbays1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Chuyenbay chuyenbay = db.Chuyenbay.Find(id);
            db.Chuyenbay.Remove(chuyenbay);
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
