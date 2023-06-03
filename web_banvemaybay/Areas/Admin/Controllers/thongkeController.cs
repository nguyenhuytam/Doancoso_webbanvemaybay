using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using web_banvemaybay.Models;

namespace web_banvemaybay.Areas.Admin.Controllers
{
    public class thongkeController : Controller
    {

        // GET: Admin/thongke
            public ActionResult Index(String monthYear)
            {
                DateTime monthy = new DateTime(int.Parse(monthYear), 1,1);
                web_banvemaybayEntities db = new web_banvemaybayEntities();
                if (monthy == null) { monthy = DateTime.Now; }
                DateTime startDate = new DateTime(monthy.Year, 1, 1);
                DateTime endDate = new DateTime(monthy.Year, 12, 31);
                var ves = db.Ve.Where(c => c.Ngaydatve >= startDate && c.Ngaydatve <= endDate).ToList();
                var tongDanhThuThang = new List<Doanhthu>();

                for (int month = 1; month <= 12; month++)
                {
                    var vesThang = ves.Where(c => c.Ngaydatve.Month == month);
                    double tongTienThang = vesThang.Sum(c => c.Gia);

                    var doanhThuThang = new Doanhthu
                    {
                        thang = $"{month}",
                        doanhthu = tongTienThang
                    };

                    tongDanhThuThang.Add(doanhThuThang);
                }
            Session["NamDuocChon"] = monthy.Year.ToString();
            return View(tongDanhThuThang);
            }
        public ActionResult DoanhThuNamNay()
        {
            web_banvemaybayEntities db = new web_banvemaybayEntities();
            DateTime monthYear = DateTime.Now;
            DateTime startDate = new DateTime(monthYear.Year, 1, 1);
            DateTime endDate = new DateTime(monthYear.Year, 12, 31);
            var ves = db.Ve.Where(c => c.Ngaydatve >= startDate && c.Ngaydatve <= endDate).ToList();
            var tongDanhThuThang = new List<Doanhthu>();

            for (int month = 1; month <= 12; month++)
            {
                var vesThang = ves.Where(c => c.Ngaydatve.Month == month);
                double tongTienThang = vesThang.Sum(c => c.Gia);

                var doanhThuThang = new Doanhthu
                {
                    //thang = $"{month}/{monthYear.Year}",
                    thang = $"{month}",
                    doanhthu = tongTienThang
                };

                tongDanhThuThang.Add(doanhThuThang);
            }
            Session["NamDuocChon"] = monthYear.Year.ToString();
            return View(tongDanhThuThang);
        }
    }
}