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
    public class ChuyenbaysController : Controller
    {
        private web_banvemaybayEntities db = new web_banvemaybayEntities();

        // GET: Admin/Chuyenbays
        public ActionResult Index()
        {
            return View(db.Chuyenbay.ToList());
        }
        public ActionResult DoanhThu(int? id)
        {
            Chuyenbay chuyenbay = db.Chuyenbay.Find(id);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var kt = db.Ve.Where(c => c.IDchuyenbay == id).ToList();
            double tong = 0;
            int soHangVePhoThong = db.Ve.Count(c => c.IDchuyenbay == id && c.IDhangve == 2);
            int soHangVeThuongGia = db.Ve.Count(c => c.IDchuyenbay == id && c.IDhangve == 1);
            var check = db.Chuyenbay.Where(c => c.IDchuyenbay == id).FirstOrDefault();
            if (kt != null)
            {
                foreach (var item in kt)
                {
                    tong += item.Gia;
                }
            }
            Session["PTDaBan"] = soHangVePhoThong;
            Session["TGDaBan"] = soHangVeThuongGia;
            Session["SoVeDaBan"] = soHangVePhoThong + soHangVeThuongGia;
            Session["Tongdoanhthu"] = tong;
            return View(chuyenbay);
            
            //Chuyenbay chuyenbay = db.Chuyenbay.Find(id);
            //int sovedaban = int.Parse((162 - chuyenbay.PhoThong).ToString()) + int.Parse((16 - chuyenbay.ThuongGia).ToString());
            //Session["SoVeDaBan"] = sovedaban;
            //double doanhthu = (double.Parse((162 - chuyenbay.PhoThong).ToString()) * chuyenbay.Giatien) + (double.Parse((16 - chuyenbay.ThuongGia).ToString()) * (chuyenbay.Giatien + 100000));
            //Session["DoanhThuChuyen"] = doanhthu.ToString();
            //if (chuyenbay == null)
            //{
            //    return HttpNotFound();
            //}
            //return View(chuyenbay);
        }
        // GET: Admin/Chuyenbays/Details/5
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

        // GET: Admin/Chuyenbays/Create
        public ActionResult Create()
        {
            ViewBag.IDhang = new SelectList(db.HangHK, "IDhang", "TenHang");
            return View();
        }

        // POST: Admin/Chuyenbays/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IDchuyenbay,Diadiemdi,Diadiemden,Ngaydi,Ngayve,Giatien,IDhang")] Chuyenbay chuyenbay)
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

        // GET: Admin/Chuyenbays/Edit/5
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

        // POST: Admin/Chuyenbays/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IDchuyenbay,Diadiemdi,Diadiemden,Ngaydi,Ngayve,Giatien,IDhang")] Chuyenbay chuyenbay)
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

        // GET: Admin/Chuyenbays/Delete/5
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

        // POST: Admin/Chuyenbays/Delete/5
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
