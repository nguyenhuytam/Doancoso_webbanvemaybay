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
    public class HangHKsController : Controller
    {
        private web_banvemaybayEntities db = new web_banvemaybayEntities();

        // GET: Admin/HangHKs
        public ActionResult Index()
        {
            return View(db.HangHK.ToList());
        }

        // GET: Admin/HangHKs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HangHK hangHK = db.HangHK.Find(id);
            if (hangHK == null)
            {
                return HttpNotFound();
            }
            return View(hangHK);
        }

        // GET: Admin/HangHKs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/HangHKs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IDhang,TenHang,Hinhanh")] HangHK hangHK)
        {
            if (ModelState.IsValid)
            {
                db.HangHK.Add(hangHK);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(hangHK);
        }

        // GET: Admin/HangHKs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HangHK hangHK = db.HangHK.Find(id);
            if (hangHK == null)
            {
                return HttpNotFound();
            }
            return View(hangHK);
        }

        // POST: Admin/HangHKs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IDhang,TenHang,Hinhanh")] HangHK hangHK)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hangHK).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(hangHK);
        }

        // GET: Admin/HangHKs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HangHK hangHK = db.HangHK.Find(id);
            if (hangHK == null)
            {
                return HttpNotFound();
            }
            return View(hangHK);
        }

        // POST: Admin/HangHKs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            HangHK hangHK = db.HangHK.Find(id);
            db.HangHK.Remove(hangHK);
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
