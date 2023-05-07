using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using web_banvemaybay.Models;

namespace web_banvemaybay.Controllers
{
    public class CheckController : Controller
    {
        // GET: Check
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]

        public ActionResult Checkve(string Email, int id)
        {
            web_banvemaybayEntities db = new web_banvemaybayEntities();
            var kq = db.Ve.Where(c => c.IDve ==id);
            return View(kq.OrderBy(c => c.IDve));
        }

    }
}