using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using web_banvemaybay.Models;

namespace web_banvemaybay.Areas.Admin.Controllers
{
    public class chitietchuyenbayController : Controller
    {
        // GET: Admin/chitietchuyenbay
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult chitiet(int idchuyenbay)
        {
            web_banvemaybayEntities db = new web_banvemaybayEntities();
            var kt = db.Ve.Where(c => c.IDchuyenbay == idchuyenbay).ToList();
            double tong = 0;
            int soHangVePhoThong = db.Ve.Count(c => c.IDchuyenbay == idchuyenbay && c.IDhangve == 2);
            int soHangVeThuongGia = db.Ve.Count(c => c.IDchuyenbay == idchuyenbay && c.IDhangve == 1);
            var check = db.Chuyenbay.Where(c => c.IDchuyenbay == idchuyenbay).FirstOrDefault();
            if (kt != null)
            {
                foreach (var item in kt)
                {
                    tong += item.Gia;
                }
            }
            var PTtrong = check.PhoThong;
            var TGtrong = check.ThuongGia;
            Session["PTtrong"]=PTtrong;
            Session["TGtrong"] = TGtrong;
            Session["Phothong"]= soHangVePhoThong;
            Session["Thuonggia"]= soHangVeThuongGia;
            Session["Tongdoanhthu"]= tong;
            return View();
        }
    }
}