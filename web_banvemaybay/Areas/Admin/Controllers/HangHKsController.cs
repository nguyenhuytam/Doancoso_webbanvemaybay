using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
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
        public ActionResult Create(HangHK Create, HttpPostedFileBase fileAnh)
        {
            if (ModelState.IsValid)
            {
                web_banvemaybayEntities db = new web_banvemaybayEntities();
                var existingUser = db.HangHK.FirstOrDefault(c => c.IDhang == Create.IDhang);
                if (existingUser != null)
                {
                    ModelState.AddModelError("", "Lỗi vui lòng kiểm tra id  ");
                    return View(Create);
                }
                if (fileAnh.ContentLength > 0)
                {
                    string file = Server.MapPath("/Areas/Admin/ContentAdmin/image/");
                    string path = file + fileAnh.FileName;
                    fileAnh.SaveAs(path);
                    Create.Hinhanh = "/Areas/Admin/ContentAdmin/image/" + fileAnh.FileName;
                }
                db.HangHK.Add(Create);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Tên hãng không được để trống");
                return View(Create);
            }
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
        public ActionResult Edit(HangHK hang, HttpPostedFileBase fileAnh, HttpPostedFileBase filevideo)
        {
            if (ModelState.IsValid)
            {
                web_banvemaybayEntities db = new web_banvemaybayEntities();
                var existingUser = db.HangHK.FirstOrDefault(c => c.IDhang == hang.IDhang);
                if (existingUser == null)
                {
                    // Mã hóa mật khẩu trước khi lưu vào CSDL\
                    // user.pass = MD5Hash(user.pass);
                    ModelState.AddModelError("", "Lỗi vui lòng kiểm tra id  ");
                    return View(hang);
                }
                existingUser.TenHang = hang.TenHang;
                if (fileAnh.ContentLength > 0)
                {
                    string fileName = Path.GetFileName(fileAnh.FileName);
                    string fileExtension = Path.GetExtension(fileName).ToLower();
                    string filePath = Path.Combine(Server.MapPath("~/Areas/Admin/ContentAdmin/image/"), fileName);
                    fileAnh.SaveAs(filePath);
                    existingUser.Hinhanh = "/Areas/Admin/ContentAdmin/image/" + fileName;
                }
                db.Entry(existingUser).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(hang);
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
