using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using web_banvemaybay.Models;

namespace web_banvemaybay.Controllers
{
    public class BinhluanController : Controller
    {
        web_banvemaybayEntities db = new web_banvemaybayEntities();
        // GET: Binhluan
        public ActionResult Index()
        {
            List<Binhluan> binhluan = db.Binhluan.ToList();
            return View(binhluan);
        }
        public ActionResult binhluan(string binhluanmoi,int selectedRating) {
            int IDTK = int.Parse(Session["IDtaikhoan"].ToString());
            var kt = db.Binhluan.FirstOrDefault();
            if (IDTK !=null)
            {
                kt.binhluan1 = binhluanmoi;
                kt.sosao = selectedRating;
                kt.idtaikhoan = IDTK;
                db.Binhluan.Add(kt);
                db.SaveChanges();
            }
            else
            {
                ViewBag.thongbao = "Vui lòng đăng nhập khi bình luận";
                return RedirectToAction("Index", "Binhluan");
            }
            return RedirectToAction("Index", "Binhluan");
        }

    }
}