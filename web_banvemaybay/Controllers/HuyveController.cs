using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using web_banvemaybay.Models;

namespace web_banvemaybay.Controllers
{
    public class HuyveController : Controller
    {
        // GET: Huyve
        public ActionResult Index()
        {
            web_banvemaybayEntities db = new web_banvemaybayEntities();
            int mave = int.Parse(@Session["mavemuontra"].ToString());
            var checkma = db.Ve.Where(c => c.IDve.Equals(mave)).FirstOrDefault();
            if (checkma.Tinhtrang == "Đã thanh toán")
            {
                double tienhoang;
               tienhoang= checkma.Gia - ((30 * checkma.Gia) / 100);
                ViewBag.TienHoang = tienhoang;
            }
            else
            {
                string message = "Đã hủy ";
                ViewBag.Message = message;
            }
            
            return View();
        }
    }
}