using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace web_banvemaybay.Controllers
{
    public class HomeController : Controller
    {
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
            Session.Clear();
            return RedirectToAction("Home", "Home");
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult Home(string thongbao)
        {
            ViewBag.thongbao = thongbao;
            return View();
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