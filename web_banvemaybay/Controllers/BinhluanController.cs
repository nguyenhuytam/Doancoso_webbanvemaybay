using PagedList;
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
        public ActionResult Index(int? page)
        {
            int pageSize = 5    ;
            int pageNumber = (page ?? 1);
            var binhluanList = db.Binhluan.ToList().ToPagedList(pageNumber, pageSize);
            return View(binhluanList);
        }
        public ActionResult binhluan(string binhluanmoi, int selectedRating)
        {
            if (Session["IDtaikhoan"] != null)
            {
                int IDTK;
                if (int.TryParse(Session["IDtaikhoan"].ToString(), out IDTK))
                {
                    Binhluan newBinhLuan = new Binhluan
                    {
                        binhluan1 = binhluanmoi,
                        sosao = selectedRating,
                        idtaikhoan = IDTK,
                        ngaybl =  DateTime.Now // Thêm thời gian bình luận ở đây
                    };
                    db.Binhluan.Add(newBinhLuan);
                    db.SaveChanges();
                }
                else
                {
                    ViewBag.thongbao = "Lỗi xảy ra khi lấy ID tài khoản";
                    return RedirectToAction("Index", "Binhluan");
                }
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