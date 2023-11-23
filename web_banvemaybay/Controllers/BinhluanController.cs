using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using web_banvemaybay.Models;
using PagedList;

namespace web_banvemaybay.Controllers
{
    public class BinhluanController : Controller
    {
        web_banvemaybayEntities db = new web_banvemaybayEntities();

        // GET: Binhluan
        public ActionResult Index(int? page)
        {
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            var binhluan = db.Binhluan.ToList().ToPagedList(pageNumber, pageSize);
            return View(binhluan);
        }

        [HttpPost]
        public ActionResult Binhluan(string binhluanmoi, int selectedRating, int? idTraloi)
        {
            if (Session["IDtaikhoan"] != null)
            {
                int IDTK = int.Parse(Session["IDtaikhoan"].ToString());
                var newBinhLuan = new Binhluan
                {
                    binhluan1 = binhluanmoi,
                    sosao = selectedRating,
                    idtaikhoan = IDTK,
                    idTraloi = idTraloi // Lưu giá trị idTraloi từ form vào đối tượng Binhluan
                };
                db.Binhluan.Add(newBinhLuan);
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
