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
    public class VesController : Controller
    {
        private web_banvemaybayEntities db = new web_banvemaybayEntities();

        // GET: Admin/Ves
        public ActionResult Index()
        {
            var ve = db.Ve.Include(v => v.Chuyenbay).Include(v => v.Dattruoc).Include(v => v.Hanhkhach).Include(v => v.Hanhli).Include(v => v.TTlienhe);
            return View(ve.ToList());
        }

        // GET: Admin/Ves/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ve ve = db.Ve.Find(id);
            if (ve == null)
            {
                return HttpNotFound();
            }
            return View(ve);
        }

        // GET: Admin/Ves/Create
        public ActionResult Create()
        {
            ViewBag.IDchuyenbay = new SelectList(db.Chuyenbay, "IDchuyenbay", "Diadiemdi");
            ViewBag.IDdattruoc = new SelectList(db.Dattruoc, "IDdattruoc", "Tinhtrang");
            ViewBag.IDhanhkhach = new SelectList(db.Hanhkhach, "IDhanhkhach", "Tenhanhkhach");
            ViewBag.IDhanhli = new SelectList(db.Hanhli, "IDhanhli", "Kg");
            ViewBag.IDlienhe = new SelectList(db.TTlienhe, "IDlienhe", "FullName");
            return View();
        }

        // POST: Admin/Ves/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IDve,IDdattruoc,IDchuyenbay,IDhanhkhach,Giatien,Tinhtrang,IDlienhe,IDhanhli,Ngaydatve,Gia")] Ve ve)
        {
            if (ModelState.IsValid)
            {
                db.Ve.Add(ve);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IDchuyenbay = new SelectList(db.Chuyenbay, "IDchuyenbay", "Diadiemdi", ve.IDchuyenbay);
            ViewBag.IDdattruoc = new SelectList(db.Dattruoc, "IDdattruoc", "Tinhtrang", ve.IDdattruoc);
            ViewBag.IDhanhkhach = new SelectList(db.Hanhkhach, "IDhanhkhach", "Tenhanhkhach", ve.IDhanhkhach);
            ViewBag.IDhanhli = new SelectList(db.Hanhli, "IDhanhli", "Kg", ve.IDhanhli);
            ViewBag.IDlienhe = new SelectList(db.TTlienhe, "IDlienhe", "FullName", ve.IDlienhe);
            return View(ve);
        }

        // GET: Admin/Ves/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ve ve = db.Ve.Find(id);
            if (ve == null)
            {
                return HttpNotFound();
            }
            ViewBag.IDchuyenbay = new SelectList(db.Chuyenbay, "IDchuyenbay", "Diadiemdi", ve.IDchuyenbay);
            ViewBag.IDdattruoc = new SelectList(db.Dattruoc, "IDdattruoc", "Tinhtrang", ve.IDdattruoc);
            ViewBag.IDhanhkhach = new SelectList(db.Hanhkhach, "IDhanhkhach", "Tenhanhkhach", ve.IDhanhkhach);
            ViewBag.IDhanhli = new SelectList(db.Hanhli, "IDhanhli", "Kg", ve.IDhanhli);
            ViewBag.IDlienhe = new SelectList(db.TTlienhe, "IDlienhe", "FullName", ve.IDlienhe);
            return View(ve);
        }

        // POST: Admin/Ves/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IDve,IDdattruoc,IDchuyenbay,IDhanhkhach,Giatien,Tinhtrang,IDlienhe,IDhanhli,Ngaydatve,Gia")] Ve ve)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ve).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IDchuyenbay = new SelectList(db.Chuyenbay, "IDchuyenbay", "Diadiemdi", ve.IDchuyenbay);
            ViewBag.IDdattruoc = new SelectList(db.Dattruoc, "IDdattruoc", "Tinhtrang", ve.IDdattruoc);
            ViewBag.IDhanhkhach = new SelectList(db.Hanhkhach, "IDhanhkhach", "Tenhanhkhach", ve.IDhanhkhach);
            ViewBag.IDhanhli = new SelectList(db.Hanhli, "IDhanhli", "Kg", ve.IDhanhli);
            ViewBag.IDlienhe = new SelectList(db.TTlienhe, "IDlienhe", "FullName", ve.IDlienhe);
            return View(ve);
        }

        // GET: Admin/Ves/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ve ve = db.Ve.Find(id);
            if (ve == null)
            {
                return HttpNotFound();
            }
            return View(ve);
        }

        // POST: Admin/Ves/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Ve ve = db.Ve.Find(id);
            db.Ve.Remove(ve);
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
