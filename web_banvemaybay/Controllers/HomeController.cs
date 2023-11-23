using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using web_banvemaybay.Models;

namespace web_banvemaybay.Controllers
{
    public class HomeController : Controller
    {
        web_banvemaybayEntities db = new web_banvemaybayEntities();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        public ActionResult Clear()
        {

            // Kiểm tra xem người dùng đã đăng nhập thành công chưa
            if (Session["EmailTaiKhoan"] != null)
            {
                // Người dùng đã đăng nhập thành công, xóa tất cả dữ liệu khác trừ thông tin đăng nhập
                Session.Remove("giatien");
                Session.Remove("idcu");
                Session.Remove("idHangve");
                Session.Remove("idcu");
                // Xóa các phần tử khác

                // Chuyển hướng đến trang chủ
                return RedirectToAction("Home", "Home");
            }
            else
            {
                // Người dùng chưa đăng nhập hoặc đã đăng xuất, xóa tất cả dữ liệu
                Session.Clear();

                // Chuyển hướng đến trang chủ
                return RedirectToAction("Home", "Home");
            }
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult Home(string thongbao)
        {
            List<DiaDiem> diaDiems = db.DiaDiem.ToList();
            ViewBag.thongbao = thongbao;
            return View(diaDiems);
        }
        public ActionResult ChonVe()
        {
            return View();
        }
        public ActionResult NhapThongTin()
        {
            return View();
        }
        public ActionResult DatThanhCong()
        {
            return View();
        }
        public ActionResult GioiThieu()
        {
            return View();
        }
        public ActionResult LienHe()
        {
            return View();
        }
    }
}