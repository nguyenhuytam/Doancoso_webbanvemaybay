using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
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
            ViewBag.id = mave;
            var checkma = db.Ve.Where(c => c.IDve.Equals(mave)).FirstOrDefault();
            var checkmafrom = db.Ve.Where(c => c.IDve == checkma.IDchuyenbayve).FirstOrDefault();
            ViewBag.Tenkhach = checkma.TTlienhe.FullName;
            ViewBag.NgayBay = checkma.Chuyenbay.Ngaydi.Value;
            ViewBag.dddi = checkma.Chuyenbay.Diadiemdi;
            ViewBag.ddden = checkma.Chuyenbay.Diadiemden;
            ViewBag.hang = checkma.Chuyenbay.HangHK.TenHang;
            ViewBag.macb = checkma.Chuyenbay.IDchuyenbay;
            if (checkma.Tinhtrang == "Đã thanh toán")
            {
                DateTime ngayhienhtai = DateTime.Today;
                DateTime ngayxuatphat = checkma.Chuyenbay.Ngaydi.Value;
                int sosanh = (ngayxuatphat - ngayhienhtai).Days;
                if (sosanh >= 2)
                {
                    double tienhoang;
                    tienhoang = checkma.Gia - ((30 * checkma.Gia) / 100);
                    double tienphat = checkma.Gia - tienhoang;
                    string message = "Yêu Cầu Hủy Vé";
                    checkma.Tinhtrang = message;
                    checkma.Tienphat = tienphat;
                    db.Ve.AddOrUpdate(checkma);
                    db.SaveChanges();
                    string thongbao = "Số tiền hoàng sẽ được hoàng trả trong vòng 7 ngày vui vòng liên hệ quầy bán vé gần nhất để thực hiện giải quyết";
                    var chuyenbay = db.Chuyenbay.Where(c => c.IDchuyenbay == checkma.IDchuyenbay).FirstOrDefault();
                    if (checkma.Hangve.IDhangve == 1)
                    {
                        chuyenbay.ThuongGia += 1 * checkma.Sove;
                    }
                    else
                    {
                        chuyenbay.PhoThong += 1 * checkma.Sove;
                    }
                    db.Chuyenbay.AddOrUpdate(chuyenbay);
                    db.SaveChanges();
                }
                else
                {
                    string thongbao = "Xin lỗi bạn không thể thực hiện hủy vé trong thời gian xác ngày xuất phát !!! Chỉ được thực hiện hủy vé cách thời gian xuất phát là 3 ngày";
                    ViewBag.thongbao = thongbao;
                }
               
            }
            else
            {
                string message = "Đã hủy ";
                ViewBag.Message = message;
                checkma.Tinhtrang = message;
                db.Ve.AddOrUpdate(checkma);
               db.SaveChanges();  
                var chuyenbay = db.Chuyenbay.Where(c=> c.IDchuyenbay == checkma.IDchuyenbay).FirstOrDefault();
                if (checkma.Hangve.IDhangve == 1)
                {
                    chuyenbay.ThuongGia += 1 * checkma.Sove;
                }
                else
                {
                    chuyenbay.PhoThong += 1 * checkma.Sove;
                }
                db.Chuyenbay.AddOrUpdate(chuyenbay);
                db.SaveChanges();             
            }

            if(checkma.IDchuyenbayve != null)
            {
                Session["mavefrom"] = checkma.IDchuyenbayve;
                ViewBag.idfrom = checkma.IDchuyenbayve;
                ViewBag.Tenkhachfrom = checkmafrom.TTlienhe.FullName;
                ViewBag.NgayBayfrom = checkmafrom.Chuyenbay.Ngaydi.Value;
                ViewBag.dddifrom = checkmafrom.Chuyenbay.Diadiemdi;
                ViewBag.dddenfrom = checkmafrom.Chuyenbay.Diadiemden;
                ViewBag.hangfrom = checkmafrom.Chuyenbay.HangHK.TenHang;
                ViewBag.macbfrom = checkmafrom.Chuyenbay.IDchuyenbay;
                if (checkmafrom.Tinhtrang == "Đã thanh toán")
                {
                    DateTime ngayhienhtai = DateTime.Today;
                    DateTime ngayxuatphat = checkmafrom.Chuyenbay.Ngaydi.Value;
                    int sosanh = (ngayxuatphat - ngayhienhtai).Days;
                    if (sosanh >= 3)
                    {
                        double tienhoang;
                        tienhoang = checkmafrom.Gia - ((30 * checkmafrom.Gia) / 100);
                        double tienphat = checkmafrom.Gia - tienhoang;
                        string message = "Yêu Cầu Hủy Vé";
                        checkmafrom.Tinhtrang = message;
                        checkmafrom.Tienphat = tienphat;
                        db.Ve.AddOrUpdate(checkmafrom);
                        db.SaveChanges();
                        string thongbao = "Số tiền hoàng sẽ được hoàng trả trong vòng 7 ngày vui vòng liên hệ quầy bán vé gần nhất để thực hiện giải quyết";
                        var chuyenbay = db.Chuyenbay.Where(c => c.IDchuyenbay == checkmafrom.IDchuyenbay).FirstOrDefault();
                        if (checkmafrom.Hangve.IDhangve == 1)
                        {
                            chuyenbay.ThuongGia += 1 * checkmafrom.Sove;
                        }
                        else
                        {
                            chuyenbay.PhoThong += 1 * checkmafrom.Sove;
                        }
                        db.Chuyenbay.AddOrUpdate(chuyenbay);
                        db.SaveChanges();
                    }
                    else
                    {
                        string thongbao = "Xin lỗi bạn không thể thực hiện hủy vé trong thời gian xác ngày xuất phát !!! Chỉ được thực hiện hủy vé cách thời gian xuất phát là 3 ngày";
                        ViewBag.thongbao = thongbao;
                    }

                }
                else
                {
                    string message = "Đã hủy ";
                    ViewBag.Message = message;
                    checkmafrom.Tinhtrang = message;
                    db.Ve.AddOrUpdate(checkmafrom);
                    db.SaveChanges();
                    var chuyenbay = db.Chuyenbay.Where(c => c.IDchuyenbay == checkmafrom.IDchuyenbay).FirstOrDefault();
                    if (checkmafrom.Hangve.IDhangve == 1)
                    {
                        chuyenbay.ThuongGia += 1 * checkmafrom.Sove;
                    }
                    else
                    {
                        chuyenbay.PhoThong += 1 * checkmafrom.Sove;
                    }
                    db.Chuyenbay.AddOrUpdate(chuyenbay);
                    db.SaveChanges();
                }
            }    

            

            return View();
        }
    }
}