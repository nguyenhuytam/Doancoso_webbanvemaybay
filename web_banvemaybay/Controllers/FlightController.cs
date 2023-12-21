using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Security.Cryptography.Pkcs;
using System.Threading.Tasks;
using System.Web.Helpers;
using System.Web.Mvc;
using Antlr.Runtime.Tree;
using Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using web_banvemaybay.Models;
using web_banvemaybay.Others.MoMo;
using web_banvemaybay.Others.Vnpay;
namespace FlightSearch.Controllers
{
    public class FlightController : Controller
    {
        web_banvemaybayEntities db = new web_banvemaybayEntities();
        public ActionResult Khuhoi()
        {
            return View();
        }
        [HttpPost]
        public JsonResult Hv(int idHangve)
        {
            int hangvekh = Session["hangvekh"] != null ? int.Parse(Session["hangvekh"].ToString()) : 0;
            Session["hangvekh"] = idHangve;
            var hangve = db.Hangve.Where(c=>c.IDhangve == idHangve).FirstOrDefault();
            var hangvecu = db.Hangve.Where(c => c.IDhangve == hangvekh).FirstOrDefault();
            double tong = double.Parse(Session["giatien"].ToString());
            double giahangve = 0;
            //if (hangvekh >0)
            //{
            //     giahangve = hangve.Gia + tong - hangvecu.Gia;
            //}
            //else
            //{
            //     giahangve = hangve.Gia + tong;
            //}
            if (idHangve == 1)
            {
                hangve.Gia *= 2;
            }

            if (hangvekh == 1)
            {
                hangvecu.Gia *= 2;
            }

            if (hangvekh > 0)
            {
                giahangve = hangve.Gia + tong - hangvecu.Gia;
            }
            else
            {
                giahangve = hangve.Gia + tong;
            }
            Session["giatien"]=giahangve;
            return Json(Session["giatien"]);
        }
        [HttpPost]
        public JsonResult UpdateSessionPrice(int toId, int? fromId)
        {
            try
            {
                int toIdcu = Session["toIdcu"] != null ? int.Parse(Session["toIdcu"].ToString()) : 0;
                int fromIdcu = Session["fromIdcu"] != null ? int.Parse(Session["fromIdcu"].ToString()) : 0;
                // Ví dụ: Lấy giá tiền từ database dựa trên toId và fromId
                double tong = double.Parse(Session["giatien"].ToString());
                var to = db.Hanhli.Where(c => c.IDhanhli == toId).FirstOrDefault();
                var from = db.Hanhli.Where(c => c.IDhanhli == fromId).FirstOrDefault();
                var tocu = db.Hanhli.Where(c => c.IDhanhli == toIdcu).FirstOrDefault();
                var fromcu = db.Hanhli.Where(c => c.IDhanhli == fromIdcu).FirstOrDefault();
                double giahanhli = 0;
                Session["toIdcu"] = toId;
                Session["fromIdcu"] = fromId;
                if (toIdcu > 0 && toIdcu!=toId )
                {
                    giahanhli = to.Giatien +tong - tocu.Giatien;
                    Session["giatien"] = giahanhli;// Giá tiền mới sau khi thực hiện các phép tính
                    return Json(Session["giatien"]);
                }
                if ( toId >1 && toIdcu != toId)
                {
                    giahanhli = to.Giatien + tong;
                    Session["giatien"] = giahanhli;// Giá tiền mới sau khi thực hiện các phép tính
                    return Json(Session["giatien"]);

                }
                if (fromIdcu > 0 && fromIdcu != fromId)
                {
                    giahanhli = from.Giatien + tong - fromcu.Giatien;
                    Session["giatien"] = giahanhli;// Giá tiền mới sau khi thực hiện các phép tính
                    return Json(Session["giatien"]);
                }
                else
                {
                    giahanhli=from.Giatien + tong;
                    Session["giatien"] = giahanhli;// Giá tiền mới sau khi thực hiện các phép tính
                    return Json(Session["giatien"]);
                }    
               
            }
            catch (Exception ex)
            {
                // Log exception hoặc trả về một thông báo lỗi cụ thể
                // Ví dụ: Log.Error(ex.Message);
                return Json(new { error = "Có lỗi xảy ra khi cập nhật giá tiền." });
            }
        }
        public ActionResult hl(int idTo, int idFrom)
        {
            float nl = Session["khnl"] != null ? float.Parse(Session["khnl"].ToString()) : 0;
            float te = Session["khte"] != null ? float.Parse(Session["khte"].ToString()) : 0;
            float eb = Session["kheb"] != null ? float.Parse(Session["kheb"].ToString()) : 0;
            float tongsl = nl + te + eb;
            Session["tongsl"]=tongsl;
            Session["idTo"] = idTo;
            Session["idFrom"]=idFrom;
            web_banvemaybayEntities db = new web_banvemaybayEntities();
            var to = db.Chuyenbay.Where(c => c.IDchuyenbay == idTo).FirstOrDefault();
            var from = db.Chuyenbay.Where(c => c.IDchuyenbay == idFrom).FirstOrDefault();
            double tong = to.Giatien + from.Giatien;
            Session["giatien"] = tong * nl + ((0.25 * to.Giatien+(0.25*from.Giatien)) * eb) + ((0.75 * to.Giatien + (0.75 * from.Giatien)) * te);
            Session["giave"] = tong * nl + ((0.25 * to.Giatien + (0.25 * from.Giatien)) * eb) + ((0.75 * to.Giatien + (0.75 * from.Giatien)) * te);
            var hanhli = db.Hanhli.ToList();
            return View();
        }
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Sohangve(int idHangve,int idChuyenBay,int? idHangvecu)
        {
            int kthl = Session["idcu"] != null ? int.Parse(Session["idcu"].ToString()) : 0;

            int idhanhli = Session["idhanhlimoi"] != null ? int.Parse(Session["idhanhlimoi"].ToString()) : 0;
            Session["idkthlcu"] = kthl;
            Session["idmoinhat"] = idhanhli;
            int nguoidat = int.Parse(Session["soluong"].ToString());
            int? idGhecu = idHangve;
            var idHv = db.Hangve.Where(c => c.IDhangve == idHangve).FirstOrDefault();
            var idgiave = db.Hangve.Where(c => c.IDhangve == 1).FirstOrDefault();
            var tt = db.Chuyenbay.Where(c => c.IDchuyenbay == idChuyenBay).FirstOrDefault();
            var sl =  new Chuyenbay();
            double giatienidcu = double.Parse(Session["giatien" + tt.IDchuyenbay].ToString());
            Session["idHangve"] = idHangve;

            if (tt.PhoThong < nguoidat && idHangve == 2)
            {
                double tongtien = (giatienidcu) - (tt.Giatien / 100 * 10 + 70000)- (idgiave.Gia*nguoidat);
                TempData["ErrorMessage"] = "Hết chỗ trống vui lòng chọn lại  !";
                Session["giatien" + tt.IDchuyenbay] = tongtien;
                Session["idHangvecu"] = idGhecu;
                Session["idHangve"] = idHangve;
                // Chuyển hướng đến action "Information" trong controller "Flight"
                return RedirectToAction("Information", "Flight", new { id = idChuyenBay });
            }
            if (tt.PhoThong < nguoidat && idHangve == 1)
            {
                double tongtien = (giatienidcu) - (tt.Giatien / 100 * 10 + 70000) -(idgiave.Gia * nguoidat);
                Session["idHangve"] = idHangve;
                Session["idHangvecu"] = idGhecu;
                Session["giatien" + tt.IDchuyenbay] = tongtien;
                // Chuyển hướng đến action "Information" trong controller "Flight"
                return RedirectToAction("Information", "Flight", new { id = idChuyenBay });
            }
            if (tt.ThuongGia<nguoidat && idHangve ==1)
            {
                double tongtien = giatienidcu - (tt.Giatien / 100 * 10 + 70000);
                TempData["ErrorMessage"] = "Hết chỗ trống vui lòng chọn lại  !";
                Session["giatien" + tt.IDchuyenbay] = tongtien;
                // Chuyển hướng đến action "Information" trong controller "Flight"
                return RedirectToAction("Information", "Flight", new { id = idChuyenBay });
            }
            if (tt!=null)
            {
                if (idHangvecu != null) // Kiểm tra idhanhlicu có tồn tại
                {
                    var hanhcu = db.Hangve.Where(hl => hl.IDhangve == idHangvecu).FirstOrDefault();
                    if (hanhcu != null)
                    {
                        double tongtien = 0;
                        if (idHangvecu == 1)
                        {
                            tongtien = ((giatienidcu - hanhcu.Gia * nguoidat));
                        }
                        else
                        {
                            tongtien = ((giatienidcu + idHv.Gia * nguoidat));
                        }
                        // Lưu lại tổng tiền vào session
                        Session["giaHangghe"] = idHv.Gia;
                        Session["giatien" + tt.IDchuyenbay] = tongtien;
                        Session["idHangvecu"] = idGhecu;
                        Session["idHangve"] = idHangve;
                        Session["giathanhtoan"] = Math.Round(tongtien);
                        Session["giathanhtoanvnpay"] = Math.Round(tongtien * 100);
                        return RedirectToAction("Information", "Flight", new { id = idChuyenBay });
                    }
                }
                if (idHangve != null && idhanhli != 0)
                {
                    double gia = double.Parse(Session["giatien" + tt.IDchuyenbay].ToString());
                    double tong = gia;
                    Session["giatien" + tt.IDchuyenbay] = tong;
                    double tongtien = (tong + idHv.Gia * nguoidat);
                    Session["giatien" + tt.IDchuyenbay] = tongtien;
                    Session["giathanhtoan"] = Math.Round(tongtien);
                    Session["giathanhtoanvnpay"] = Math.Round(tongtien * 100);
                    Session["giaHangghe"] = idHv.Gia;
                    Session["idHangvecu"] = idGhecu;
                    // Chuyển hướng đến action "Information" trong controller "Flight"
                    return RedirectToAction("Information", "Flight", new { id = idChuyenBay });
                }
                if (idHangve != null)
                {
                    double gia = double.Parse(Session["giatien" + tt.IDchuyenbay].ToString());
                    double tong = gia;
                    Session["giatien" + tt.IDchuyenbay] = tong;
                    double tongtien = (tong + idHv.Gia * nguoidat) - (tt.Giatien / 100 * 10 + 70000);
                    Session["giatien" + tt.IDchuyenbay] = tongtien;
                    Session["giathanhtoan"] = Math.Round(tongtien);
                    Session["giathanhtoanvnpay"] = Math.Round(tongtien * 100);
                    Session["giaHangghe"] = idHv.Gia;

                }
                Session["idHangvecu"] = idGhecu;
                // Chuyển hướng đến action "Information" trong controller "Flight"
                return RedirectToAction("Information", "Flight", new { id = idChuyenBay });
            }
            return RedirectToAction("Information", "Flight", new { id = idChuyenBay });
        }
        [HttpPost]
        public ActionResult SearchFlight(string to, string from, DateTime? dateto, DateTime? datefrom, string searchBy, FormCollection form, float nl, float te, float eb)
        {
            Session["khnl"] = nl;
            Session["khte"] = te;
            Session["kheb"] = eb;
            if (to == from)
            {
                return RedirectToAction("Home", "Home", new { thongbao = " Vui lòng không chọn ngày đi và ngày về trùng nhau " });
            }
            if (dateto == null)
            {
                return RedirectToAction("Home", "Home", new { thongbao = " Chọn Ngày đi! " });
            }
            // Thực hiện tìm kiếm dựa trên các tham số đầu vào
            var flights = db.Chuyenbay.Where(c => c.Diadiemdi.ToLower().Contains(to.ToLower()) && c.Diadiemden.ToLower().Contains(from.ToLower()));
            if (dateto != null && datefrom != null )
            {
                var datetoValue = dateto.Value.Date;
                var datefromValue = datefrom.Value.Date;
                if (datetoValue < DateTime.Now.Date)
                {
                    return RedirectToAction("Home", "Home", new { thongbao = "Vui lòng chọn Ngày đi lớn hơn ngày hiện tại hoặc bằng ngày hiện tại " });
                }
                else if (datetoValue == DateTime.Now.Date)
                {
                    var now = DateTime.Now;

                    // Tính toán giờ hiện tại cộng thêm 1 tiếng 30 p 
                    var nowPlusOneHour = now.AddHours(1).AddMinutes(30);

                    // Kiểm tra nếu khoảng thời gian giữa ngày đi và giờ đi của mỗi chuyến bay lớn hơn giờ hiện tại và không quá 1 tiếng, hiển thị kết quả
                    flights = flights.Where(c => c.Ngaydi > now && c.Ngaydi > nowPlusOneHour && (c.PhoThong > 0 || c.ThuongGia > 0) && (c.PhoThong >= (nl + te + eb) || c.ThuongGia >= (nl + te + eb)));
                }
                else if (datefromValue <= dateto)
                {
                    return RedirectToAction("Home", "Home", new { thongbao = "Vui lòng chọn Ngày về cách ngày đi ít nhất 1 ngày " });
                }
                flights = flights.Where(c => c.Ngaydi != null && SqlFunctions.DateDiff("day", c.Ngaydi, datetoValue) == 0 && ( c.PhoThong > 0 || c.ThuongGia > 0) && (c.PhoThong >=( nl + te + eb) || c.ThuongGia >= (nl + te + eb)));
            }
            else if (dateto != null && datefrom == null)
            {
                var datetoValue = dateto.Value.Date;
                if (datetoValue < DateTime.Now.Date)
                {
                    return RedirectToAction("Home", "Home", new { thongbao = "Vui lòng chọn Ngày đi lớn hơn ngày hiện tại hoặc bằng ngày hiện tại " });
                }
                if (datetoValue == DateTime.Now.Date)
                {
                    var now = DateTime.Now;

                    // Tính toán giờ hiện tại cộng thêm 1 tiếng 30 p
                    var nowPlusOneHour = now.AddHours(1).AddMinutes(30);

                    // Kiểm tra nếu khoảng thời gian giữa ngày đi và giờ đi của mỗi chuyến bay lớn hơn giờ hiện tại và không quá 1 tiếng, hiển thị kết quả
                    flights = flights.Where(c => c.Ngaydi > now && c.Ngaydi > nowPlusOneHour  &&( c.PhoThong > 0 || c.ThuongGia > 0) && (c.PhoThong >= (nl + te + eb) || c.ThuongGia >= (nl + te + eb)));
                }
                // Hiển thị chuyến bay có ngày đi và không có ngày về
                flights = flights.Where(c => c.Ngaydi != null && SqlFunctions.DateDiff("day", c.Ngaydi, datetoValue) == 0 && (c.PhoThong > 0 || c.ThuongGia > 0) && (c.PhoThong >=(nl + te + eb) || c.ThuongGia >= (nl + te + eb)));
            }
            var valueNL = Int32.Parse(form["nl"]);
            var valueTE = Int32.Parse(form["te"]);
            var valueEB = Int32.Parse(form["eb"]);
            foreach (var flight in flights)
            {
                double giaVe = flight.Giatien; // Lưu giá tiền hiện tại vào biến tạm thời
                if (valueNL != 0 && valueTE == 0 && valueEB == 0)
                {
                    flight.Giatien = giaVe * valueNL;

                }
                else if (valueNL != 0 && valueTE != 0 && valueEB == 0)
                {
                    flight.Giatien = giaVe * valueNL + ((giaVe * 0.75) * valueTE);

                }
                else if (valueNL == 0 && valueTE != 0 && valueEB == 0)
                {
                    flight.Giatien = ((0.75 * giaVe) * valueTE);

                }
                else if (valueNL == 0 && valueTE == 0 && valueEB != 0)
                {
                    flight.Giatien = ((0.25 * giaVe) * valueEB);

                }
                else if (valueNL != 0 && valueTE == 0 && valueEB != 0)
                {
                    flight.Giatien = giaVe * valueNL + ((0.25 * giaVe) * valueEB);

                }
                else if (valueNL != 0 && valueTE != 0 && valueEB != 0)
                {
                    flight.Giatien = giaVe * valueNL + ((0.25 * giaVe) * valueEB) + ((0.75 * giaVe) * valueTE);

                }
                Session["giatien" + flight.IDchuyenbay] = flight.Giatien;
            }
            Session["to"] = to;
            Session["from"] = from;
            Session["soluong"] = valueEB + valueNL + valueTE;
            Session["dateto"] = form["dateto"];
            Session["datefrom"] = form["datefrom"];
            // Lưu danh sách flights vào Session
            Session["flights"] = flights;
            Session["searchBy"] = searchBy;
            if (datefrom != null)
            {
                return RedirectToAction( "Khuhoi");
            }
            return View(flights);
        }
        public ActionResult Information(int? id,int? idTo,int? idFrom, int? idhanhli, int? idcu, int? hangveid,double? totalPrice)
        {
            if (id == null)
            {
                return View();
            }

            int ktidhlcu = Session["idkthlcu"] != null ? int.Parse(Session["idkthlcu"].ToString()) : 0;
            int kthl = Session["idmoinhat"] != null ? int.Parse(Session["idmoinhat"].ToString()) : 0;
            int kthv = Session["idHangve"] != null ? int.Parse(Session["idHangve"].ToString()) : 0;
            int checkkthv = Session["kthv"] != null ? int.Parse(Session["kthv"].ToString()) : 0;
            int checklai = Session["idHangvecu"] != null ? int.Parse(Session["idHangvecu"].ToString()) : 0;
            Session["idcheckhangvecu"] = checklai;

            if (kthl != 0 && checklai != 0 && idhanhli.HasValue)
            {
                kthl = idhanhli.Value;
            }

            if (kthl!= 0)
            {
                idhanhli = kthl;
            }
            if (kthv != null)
            {
                hangveid = kthv;
            }
            string errorMessage = TempData["ErrorMessage"] as string;
            ViewBag.ErrorMessage = errorMessage;
            int? idhanhlicu = idhanhli;
            var tt = db.Chuyenbay.Where(c => c.IDchuyenbay == id).FirstOrDefault();
            var idhl = db.Hanhli.Where(c => c.IDhanhli == idhanhli).FirstOrDefault();
            double gia = double.Parse(Session["giatien" + tt.IDchuyenbay].ToString());
            int nguoidat = int.Parse(Session["soluong"].ToString());      
            var giahangve = db.Hangve.FirstOrDefault();
            // Kiểm tra tình trạng số hạng vé thương gia và thiết lập giá trị mặc định cho idHangve

            if (tt.PhoThong < nguoidat && idhanhli == null && idcu == null)
            {
                hangveid = 1; // Thiết lập mặc định là Thương gia
                gia  += giahangve.Gia *nguoidat;
                Session["giaHangghe"] = giahangve.Gia * nguoidat; 
            }
             if (tt.PhoThong < nguoidat && idhanhli != null && idcu == null)
            {
                hangveid = 1; 
            }
            if (tt.PhoThong >= nguoidat && hangveid == 0)
            {
                hangveid = 2;
            }
            if (tt.PhoThong >= nguoidat && tt.ThuongGia >= nguoidat && hangveid == 1)
            {
                hangveid = 1;
            }
            if (tt.PhoThong >= nguoidat && tt.ThuongGia >= nguoidat && hangveid == 2)
            {
                hangveid = 2;
            }
            if (tt.PhoThong >= nguoidat && hangveid == 1 && tt.ThuongGia< nguoidat)
            {
                hangveid = 2;
            }
            if (tt.PhoThong < nguoidat && idhanhli != null && idcu != null)
            {
                hangveid = 1; // Thiết lập mặc định là Thương gia
            }
            // Truyền biến idHangve vào ViewBag
            ViewBag.IdHangve = hangveid;
            if (idcu != null) // Kiểm tra idhanhlicu có tồn tại
            {
                var hanhcu = db.Hanhli.Where(hl => hl.IDhanhli == idcu).FirstOrDefault();
                if (hanhcu != null)
                {
                    double giatienidcu = double.Parse(Session["giatien" + tt.IDchuyenbay].ToString());
                    double tongtien = (giatienidcu - hanhcu.Giatien) + idhl.Giatien;
                    // Lưu lại tổng tiền vào session
                    Session["giahanhly"] = idhl.Giatien;
                    Session["giatien" + tt.IDchuyenbay] = tongtien;
                    Session["idcu"] = idhanhlicu;
                    Session["giathanhtoan"] = Math.Round(tongtien);
                    Session["giathanhtoanvnpay"] = Math.Round(tongtien * 100);
                    Session["idhanhlimoi"] = idhanhli;
                    ViewBag.idhanhli = idhanhli;
                    // Chuyển hướng đến action "Information" trong controller "Flight"
                    return View(tt);
                }
            }
            if (idhanhli != null && tt.PhoThong >= nguoidat && tt.ThuongGia>= nguoidat && ktidhlcu != 0)
            {
                double tong = gia;
                Session["giatien" + tt.IDchuyenbay] = tong;
                Session["giathanhtoan"] = Math.Round(tong);
                Session["giathanhtoanvnpay"] = Math.Round(tong * 100);
                Session["giahanhly"] = idhl.Giatien;
                ViewBag.idhanhli = idhanhli;
                Session["idhanhlimoi"] = idhanhli;
                Session["idcu"] = idhanhlicu;
                return View(tt);
            }
            if (idhanhli == null && tt.PhoThong >= nguoidat && tt.ThuongGia >= nguoidat && ktidhlcu != 0)
            {
                double tong = gia;
                Session["giatien" + tt.IDchuyenbay] = tong;
                Session["giathanhtoan"] = Math.Round(tong);
                Session["giathanhtoanvnpay"] = Math.Round(tong * 100);
                Session["giahanhly"] = idhl.Giatien;
                ViewBag.idhanhli = idhanhli;
                Session["idhanhlimoi"] = idhanhli;
                Session["idcu"] = idhanhlicu;
                return View(tt);
            }
            if (idhanhli != null && tt.PhoThong >= nguoidat && tt.ThuongGia >= nguoidat && ktidhlcu == 0)
            {
                double tong = gia;
                Session["giatien" + tt.IDchuyenbay] = tong + idhl.Giatien;
                Session["giathanhtoan"] = Math.Round(tong);
                Session["giathanhtoanvnpay"] = Math.Round(tong * 100);
                Session["giahanhly"] = idhl.Giatien;
                ViewBag.idhanhli = idhanhli;
                Session["idhanhlimoi"] = idhanhli;
                Session["idcu"] = idhanhlicu;
                return View(tt);
            }
            if (idhanhli == null && tt.PhoThong < nguoidat && ktidhlcu != 0)
            {
                double tong = gia;
                Session["giatien" + tt.IDchuyenbay] = tong + idhl.Giatien;
                Session["giathanhtoan"] = Math.Round(tong);
                Session["giathanhtoanvnpay"] = Math.Round(tong * 100);
                Session["giahanhly"] = idhl.Giatien;
                ViewBag.idhanhli = idhanhli;
                Session["idhanhlimoi"] = idhanhli;
                Session["idcu"] = idhanhlicu;
                return View(tt);
            }
            if (idhanhli == null && tt.PhoThong < nguoidat && ktidhlcu ==0 && checklai != 0)
            {
                double tong = gia;
                Session["giatien" + tt.IDchuyenbay] = tong + (tt.Giatien / 100 * 10 + 70000);
                Session["giathanhtoan"] = Math.Round(tong);
                Session["giathanhtoanvnpay"] = Math.Round(tong * 100);
                Session["giahanhly"] = 0;
                ViewBag.idhanhli = 0;
                Session["idhanhlimoi"] = idhanhli;
                Session["idcu"] = idhanhlicu;
                return View(tt);
            }
            if (idhanhli != null && tt.PhoThong < nguoidat && ktidhlcu != 0)
            {
                double tong = gia;
                Session["giatien" + tt.IDchuyenbay] = tong + (tt.Giatien / 100 * 10 + 70000) + giahangve.Gia * nguoidat;
                Session["giathanhtoan"] = Math.Round(tong);
                Session["giathanhtoanvnpay"] = Math.Round(tong * 100);
                Session["giahanhly"] = idhl.Giatien;
                ViewBag.idhanhli = idhanhli;
                Session["idhanhlimoi"] = idhanhli;
                Session["idcu"] = idhanhlicu;
                return View(tt);
            }
            if (idhanhli != null && tt.PhoThong < nguoidat && ktidhlcu == 0)
            {
                double tong = gia;
                Session["giatien" + tt.IDchuyenbay] = tong + idhl.Giatien;
                Session["giathanhtoan"] = Math.Round(tong);
                Session["giathanhtoanvnpay"] = Math.Round(tong * 100);
                Session["giahanhly"] = idhl.Giatien;
                ViewBag.idhanhli = idhanhli;
                Session["idhanhlimoi"] = idhanhli;
                Session["idcu"] = idhanhlicu;
                return View(tt);
            }
            if (idhanhli == null && tt.ThuongGia < nguoidat && ktidhlcu != 0)
            {
                double tong = gia;
                Session["giatien" + tt.IDchuyenbay] = tong + idhl.Giatien;
                Session["giathanhtoan"] = Math.Round(tong);
                Session["giathanhtoanvnpay"] = Math.Round(tong * 100);
                Session["giahanhly"] = idhl.Giatien;
                ViewBag.idhanhli = idhanhli;
                Session["idhanhlimoi"] = idhanhli;
                Session["idcu"] = idhanhlicu;
                return View(tt);
            }
            if (idhanhli == null && tt.ThuongGia < nguoidat && ktidhlcu == 0 && checklai != 0)
            {
                double tong = gia;
                Session["giatien" + tt.IDchuyenbay] = tong + (tt.Giatien / 100 * 10 + 70000);
                Session["giathanhtoan"] = Math.Round(tong);
                Session["giathanhtoanvnpay"] = Math.Round(tong * 100);
                Session["giahanhly"] = 0;
                ViewBag.idhanhli = 0;
                Session["idhanhlimoi"] = idhanhli;
                Session["idcu"] = idhanhlicu;
                return View(tt);
            }
            if (idhanhli != null && tt.ThuongGia < nguoidat && ktidhlcu != 0)
            {
                double tong = gia;
                Session["giatien" + tt.IDchuyenbay] = tong + (tt.Giatien / 100 * 10 + 70000);
                Session["giathanhtoan"] = Math.Round(tong);
                Session["giathanhtoanvnpay"] = Math.Round(tong * 100);
                Session["giahanhly"] = idhl.Giatien;
                ViewBag.idhanhli = idhanhli;
                Session["idhanhlimoi"] = idhanhli;
                Session["idcu"] = idhanhlicu;
                return View(tt);
            }
            if (idhanhli != null && tt.ThuongGia < nguoidat && ktidhlcu == 0)
            {
                double tong = gia;
                Session["giatien" + tt.IDchuyenbay] = tong + idhl.Giatien;
                Session["giathanhtoan"] = Math.Round(tong);
                Session["giathanhtoanvnpay"] = Math.Round(tong * 100);
                Session["giahanhly"] = idhl.Giatien;
                ViewBag.idhanhli = idhanhli;
                Session["idhanhlimoi"] = idhanhli;
                Session["idcu"] = idhanhlicu;
                return View(tt);
            }
            else
            {          
                double tong = gia;
                Session["giatien" + tt.IDchuyenbay] = tong + (tt.Giatien / 100 * 10 + 70000);
                Session["giathanhtoan"] = Math.Round(tong);
                Session["giathanhtoanvnpay"] = Math.Round(tong * 100);
                ViewBag.idhanhli = 0;

            }
            Session["kthv"] = kthv;
            Session["idhanhlimoi"] = idhanhli;
            Session["idcu"] = idhanhlicu;
            return View(tt);
        }
        public ActionResult Pirec(string searchBy, string search)
        {
            Func<Chuyenbay, double?> sortBy = c => c.Giatien;
            bool isDescending = true;

            // Kiểm tra giá trị của radio button
            if (searchBy == "priceSort")
            {
                // Sắp xếp theo giảm dần
                sortBy = c => c.Giatien;
                isDescending = true;
            }
            else if (searchBy == "priceSort1")
            {
                // Sắp xếp theo tăng dần
                sortBy = c => c.Giatien;
                isDescending = false;
            }

            // Sử dụng phương thức sắp xếp dựa trên giá trị của radio button
            if (searchBy == "priceSort" || searchBy == "priceSort1")
            {
                if (isDescending)
                {
                    // Sắp xếp giảm dần theo thuộc tính Giatien
                    var flights = ((IEnumerable<Chuyenbay>)Session["flights"]).OrderByDescending(c => c.Giatien).ToList();
                    return View("SearchFlight", flights);
                }
                else
                {
                    // Sắp xếp tăng dần theo thuộc tính Giatien
                    var flights = ((IEnumerable<Chuyenbay>)Session["flights"]).OrderBy(c => c.Giatien).ToList();
                    return View("SearchFlight", flights);
                }
            }
            // Nếu không có lựa chọn, trả về View "SearchFlight" với dữ liệu ban đầu
            return View("SearchFlight", ((IEnumerable<Chuyenbay>)Session["flights"]).ToList());
        }
        [HttpPost]
        public ActionResult tt(FormCollection form, TTlienhe lienhe, HanhKhach hk , string name, DateTime? birthday, int? hangveid, int? sdtlh, string emaillh, string gioitinh, string namelh, string gioitinhlh, int? idcu, int? idchuyenbay, string payment, string cmnd)
        {
            int nguoidat = int.Parse(Session["soluong"].ToString());
            double giatien = Session["giatien" + idchuyenbay] != null ? double.Parse(@Session["giatien" + idchuyenbay].ToString()) : 0;
            int idFrom = Session["idFrom"] != null ? int.Parse(Session["idFrom"].ToString()) : 0;
            var now = DateTime.Now;
            if (birthday > now.AddYears(-18))
            {
                TempData["ErrorMessage"] = "Chưa đủ 18 tuổi vui lòng chọn  lại  !";
                return RedirectToAction("Information", "Flight", new {id = idchuyenbay});
            }
            if (name != null && birthday != null && sdtlh != null && emaillh != null && gioitinh != null && gioitinhlh != null && namelh != null  && cmnd != null)
            {
                    var ktlh = new TTlienhe();
                    ktlh.FullName = namelh;
                    ktlh.Phone = sdtlh;
                    ktlh.Email = emaillh;
                    db.TTlienhe.Add(ktlh);
                    db.SaveChanges();
                var khachhang = db.HanhKhach.Where(c=>c.IDhanhkhach == cmnd).FirstOrDefault();
                var kt = new HanhKhach();
                kt.IDhanhkhach = cmnd;
                if (khachhang == null)
                {
                    kt.Tenhanhkhach = name;
                    kt.NgaySinh = birthday;
                    kt.Gioitinh = gioitinh;
                    db.HanhKhach.Add(kt);
                    db.SaveChanges();
                }
                string Bag = "Không hành lý!";
                string DateHour = "";
                string DateHourKH = "";
                string Departure = "";
                string Destination = "";
                if (idFrom != 0)
                {
                    int idTo = Session["idTo"] != null ? int.Parse(Session["idTo"].ToString()) : 0;
                    double gia = Session["giatien"] != null ? double.Parse(Session["giatien"].ToString()) : 0;
                    double giave = Session["giave"] != null ? double.Parse(Session["giave"].ToString()) : 0;
                    double tong = gia + (giave / 100 * 10) + 70000;
                    int sl = Session["tongsl"] != null ? int.Parse(Session["tongsl"].ToString()) : 1;
                    int idtoHL = Session["toIdcu"] != null ? int.Parse(Session["toIdcu"].ToString()) : 1;
                    var giato = db.Hanhli.Where(c => c.IDhanhli == idtoHL).FirstOrDefault();
                    int idfromHL = Session["fromIdcu"] != null ? int.Parse(Session["fromIdcu"].ToString()) : 1;
                    var giafrom = db.Hanhli.Where(c => c.IDhanhli == idfromHL).FirstOrDefault();
                    int idhv = Session["hangvekh"] != null ? int.Parse(Session["hangvekh"].ToString()) : 2;
                    var giahv = db.Hangve.Where(c => c.IDhangve == idhv).FirstOrDefault();
                    var ve = new Ve();
                    var giaveto = db.Chuyenbay.Where(c => c.IDchuyenbay == idTo).FirstOrDefault();
                    if (giaveto != null)
                    {
                        double tonggiato = giaveto.Giatien + giato.Giatien + (giaveto.Giatien/100*10)+70000 + giahv.Gia;
                        ve.Gia = tonggiato;
                        Session["giaveto"] = tonggiato;
                    }
                    ve.IDchuyenbay = idTo;
                    ve.IDhangve = idhv;
                    var slhangdi = db.Chuyenbay.FirstOrDefault(cb => cb.IDchuyenbay == idTo);                   
                    if (slhangdi != null)
                    {
                        if (idhv == null)
                        {
                            slhangdi.PhoThong -= 1 * nguoidat;
                            ve.IDhangve = 2;
                        }
                        if (idhv == 1)
                        {
                            slhangdi.ThuongGia -= 1 * nguoidat;
                            ve.IDhangve = 1;
                        }
                        if (idhv == 2)
                        {
                            slhangdi.PhoThong -= 1 * nguoidat;
                            ve.IDhangve = 2;
                        }

                        db.SaveChanges();
                    }
                    Session["idchuyenbay"] = ve.IDchuyenbay;
                    ve.IDhanhKhach = kt.IDhanhkhach;
                    ve.IDlienhe = ktlh.IDlienhe;
                    ve.IDhangve = idhv;
                    ve.IDhanhli = idtoHL;
                    ve.Sove = sl;
                    ve.Ngaydatve = DateTime.Now;
                    ve.Tinhtrang = "Chờ thanh toán";
                    Session["TinhTrang"] = ve.Tinhtrang;
                    Session["emaillh"] = emaillh;
                    Session["namelh"] = namelh;
                    Session["sdtlh"] = sdtlh;
                    Session["payment"] = payment;
                    Session["KgHanhly"] = giato.Kg;
                    Session["Hangge"] = giahv.TenHangve;
                    Session["VeDi"] = ve.IDve;
                    db.Ve.Add(ve);
                    db.SaveChanges();
                    int idveTO = ve.IDve;
                    Session["idVeMoi"] = idveTO;
                    //Ve khu hoi
                    var vekh = new Ve();
                    var giavefrom = db.Chuyenbay.Where(c => c.IDchuyenbay == idFrom).FirstOrDefault();
                    if (giaveto != null)
                    {
                        double tonggiafrom = giavefrom.Giatien + giafrom.Giatien + (giavefrom.Giatien / 100 * 10) + 70000 + giahv.Gia;
                        vekh.Gia = tonggiafrom;
                        double tongvekh = tonggiafrom + double.Parse(Session["giaveto"].ToString());
                        Session["giathanhtoanvnpay"] = Math.Round(tongvekh * 100);
                    }
                    vekh.IDchuyenbay = idFrom;
                    vekh.IDhangve = idhv;
                    var slhangve = db.Chuyenbay.FirstOrDefault(cb => cb.IDchuyenbay == vekh.IDchuyenbay);
                    if (vekh.IDhangve == null)
                    {
                        if (slhangve.PhoThong > 0)
                        {
                            vekh.IDhangve = 2;
                        }
                        else
                        {
                            vekh.IDhangve = 1;
                        }
                    }
                    if (slhangve != null)
                    {
                        if (slhangve == null)
                        {
                            slhangve.PhoThong -= 1 * nguoidat;
                            vekh.IDhangve = 2;
                        }
                        if (idhv == 1)
                        {
                            slhangve.ThuongGia -= 1 * nguoidat;
                            vekh.IDhangve = 1;
                        }
                        if (idhv == 2)
                        {
                            slhangve.PhoThong -= 1 * nguoidat;
                            vekh.IDhangve = 2;
                        }

                        db.SaveChanges();
                    }
                    Session["idchuyenbaykh"] = vekh.IDchuyenbay;
                    vekh.IDhanhKhach = kt.IDhanhkhach;
                    vekh.IDlienhe = ktlh.IDlienhe;
                    vekh.IDhangve = idhv;
                    vekh.IDhanhli = idfromHL;
                    vekh.Sove = sl;
                    Session["soluong"] = vekh.Sove;
                    vekh.Ngaydatve = DateTime.Now;
                    vekh.Tinhtrang = "Chờ thanh toán";
                    Session["KgHanhlyHK"] = giafrom.Kg;
                    Session["TinhTrang"] = vekh.Tinhtrang;
                    vekh.IDchuyenbayve = idveTO;
                    db.Ve.Add(vekh);
                    db.SaveChanges();
                    int idVeFrom = vekh.IDve;
                    Session["idVeKH"] = idVeFrom;
                    //Session["giatien"] = gia;
                    var vedi = db.Ve.Where(c => c.IDve == idveTO).FirstOrDefault();
                    vedi.IDchuyenbayve = idVeFrom;
                    db.SaveChanges();
                    var chuyenbay = db.Chuyenbay.Where(hl => hl.IDchuyenbay == idTo).FirstOrDefault();
                    Departure = chuyenbay.Diadiemdi.ToString();
                    Session["Departure"] = Departure;
                    Destination = chuyenbay.Diadiemden.ToString();
                    Session["Destination"] = Destination;
                    Session["TenHang"] = chuyenbay.HangHK.TenHang;
                    DateHour = chuyenbay.Ngaydi.ToString();
                    var chuyenbaykh = db.Chuyenbay.Where(hl => hl.IDchuyenbay == idFrom).FirstOrDefault();
                    DateHourKH = chuyenbaykh.Ngaydi.ToString();
                    Session["TenHangKH"] = chuyenbaykh.HangHK.TenHang;
                    string DateHour1 = (DateTime.Parse(DateHour)).ToString();
                    Session["DateHour1"] = DateHour1;
                    string DateHour2 = (DateTime.Parse(DateHourKH)).ToString();
                    Session["DateHour2"] = DateHour2;
                    if (payment == "payment3")
                    {
                        Session["payment"] = "Thanh toán Vnpay";
                        Ve veVn = db.Ve.Find(idTo);
                        return RedirectToAction("PaymentVnpay", "Flight", veVn);
                    }
                    if (payment == "payment1")
                    {
                        Session["payment"] = "Thanh toán momo";
                        Ve veMomo = db.Ve.Find(idTo);
                        return RedirectToAction("PaymentMomo", "Flight", veMomo);
                    }
                    string content = System.IO.File.ReadAllText(Server.MapPath("~/Content/template/ChoThanhToanKH.html"));
                    content = content.Replace("{{CustomerName}}", namelh);
                    content = content.Replace("{{MaVe}}", Session["idVeMoi"].ToString());
                    content = content.Replace("{{MaVeKH}}", Session["idVeKH"].ToString());
                    content = content.Replace("{{DateHour}}", DateHour1);
                    content = content.Replace("{{DateHour2}}", DateHour2);
                    content = content.Replace("{{Departure}}", Session["Departure"].ToString());
                    content = content.Replace("{{Destination}}", Session["Destination"].ToString());
                    content = content.Replace("{{Bag}}", Session["KgHanhly"].ToString());
                    content = content.Replace("{{BagKH}}", Session["KgHanhlyHK"].ToString());
                    content = content.Replace("{{idchuyenbay}}", Session["idchuyenbay"].ToString());
                    content = content.Replace("{{idchuyenbaykh}}", Session["idchuyenbaykh"].ToString());
                    content = content.Replace("{{TenHang}}", Session["TenHang"].ToString());
                    content = content.Replace("{{TenHangKH}}", Session["TenHangKH"].ToString());
                    var toEmail = ConfigurationManager.AppSettings["ToEmailAddress"].ToString();

                    new MailHelper().SendMail(Session["emaillh"].ToString(), "Bạn đã đặt vé tại AirplaneTicket", content);
                   // new MailHelper().SendMail(toEmail, "Đơn hàng mới từ AirplaneTicket", content);
                    return RedirectToAction("DatThanhCong", "Home", new { idTo = idveTO ,idFrom=idVeFrom});

                }



                
                if (idchuyenbay != null)
                {
                    var ve = new Ve();
                    ve.IDchuyenbay = idchuyenbay;
                    ve.IDhanhKhach = kt.IDhanhkhach;
                    ve.IDlienhe = ktlh.IDlienhe;
                    ve.IDhangve = hangveid;
                    var slhang = db.Chuyenbay.FirstOrDefault(cb => cb.IDchuyenbay == idchuyenbay);
                    if (ve.IDhangve == null)
                    {
                        if (slhang.PhoThong > 0)
                        {
                            ve.IDhangve = 2;
                        }
                        else
                        {
                            ve.IDhangve = 1;
                        }
                    }
                    if (slhang!=null)
                    {
                        if (hangveid == null)
                        {
                            slhang.PhoThong -= 1 *nguoidat ;
                            ve.IDhangve = 2;
                        }
                        if (hangveid == 1)
                        {
                            slhang.ThuongGia -= 1 * nguoidat;
                            ve.IDhangve = 1;
                        }
                        if (hangveid == 2)
                        {
                            slhang.PhoThong -= 1 * nguoidat;
                            ve.IDhangve = 2;
                        }

                        db.SaveChanges();
                    }
                    ve.Sove = nguoidat;
                    var hangghe = db.Hangve.Where(hl => hl.IDhangve ==ve.IDhangve ).FirstOrDefault();
                    var Baghangghe = hangghe.TenHangve;
                    if (idcu == null)
                    {
                        ve.IDhanhli = 1;
                    }
                    else
                    {
                        ve.IDhanhli = idcu;
                    }
                    var hanhli = db.Hanhli.Where(hl => hl.IDhanhli == ve.IDhanhli).FirstOrDefault();
                    Bag = hanhli.Kg;
                    ve.Ngaydatve = DateTime.Now;
                    ve.Gia = giatien;
                    var chuyenbay = db.Chuyenbay.Where(hl => hl.IDchuyenbay == idchuyenbay).FirstOrDefault();
                    DateHour = chuyenbay.Ngaydi.ToString();
                    string DateHour1= (DateTime.Parse(DateHour)).ToString();
                    Departure = chuyenbay.Diadiemdi.ToString();
                    Destination = chuyenbay.Diadiemden.ToString();
                    ve.Tinhtrang = "Chờ thanh toán";
                    db.Ve.Add(ve);
                    db.SaveChanges();
                    int idVeMoi = ve.IDve;
                    Session["DateHour"] = DateHour;
                    Session["DateHour1"] = DateHour1;
                    Session["Departure"] = Departure;
                    Session["Destination"] = Destination;
                    Session["idVeMoi"] = idVeMoi;
                    Session["giatien"] = giatien;
                    Session["idchuyenbay"] = idchuyenbay;
                    Session["idhanhli"] = idcu;
                    Session["emaillh"] = emaillh;
                    Session["namelh"] = namelh;
                    Session["sdtlh"] = sdtlh;
                    Session["payment"] = payment;
                    Session["KgHanhly"] = Bag;
                    Session["Hangge"] = Baghangghe;
                    Session["TinhTrang"] = ve.Tinhtrang;
                    Session["TenHang"] = chuyenbay.HangHK.TenHang;
                    Session["giathanhtoanvnpay"] = Math.Round(giatien * 100);
                    if (payment == "payment1")
                    {
                        Session["payment"] = "Thanh toán momo";
                        Ve veMomo = db.Ve.Find(idVeMoi);
                        return RedirectToAction("PaymentMomo", "Flight", veMomo);
                    }
                    if (payment == "payment3")
                    {
                        Session["payment"] = "Thanh toán Vnpay";
                        Ve veVn = db.Ve.Find(idVeMoi);
                        return RedirectToAction("PaymentVnpay", "Flight", veVn);
                    }
                    string content = System.IO.File.ReadAllText(Server.MapPath("~/Content/template/ChoThanhToan.html"));
                    content = content.Replace("{{CustomerName}}", namelh);
                    content = content.Replace("{{MaVe}}", Session["idVeMoi"].ToString());
                    content = content.Replace("{{DateHour}}", DateHour1);
                    content = content.Replace("{{Departure}}", Session["Departure"].ToString());
                    content = content.Replace("{{Destination}}", Session["Destination"].ToString());
                    content = content.Replace("{{Bag}}", Session["KgHanhly"].ToString());
                    content = content.Replace("{{idchuyenbay}}", Session["idchuyenbay"].ToString());
                    content = content.Replace("{{TenHang}}", Session["TenHang"].ToString());
                    var toEmail = ConfigurationManager.AppSettings["ToEmailAddress"].ToString();

                    new MailHelper().SendMail(Session["emaillh"].ToString(), "Bạn đã đặt vé tại AirplaneTicket", content);
                    //new MailHelper().SendMail(toEmail, "Đơn hàng mới từ AirplaneTicket", content);  
                    return RedirectToAction("DatThanhCong", "Home", new { id = idVeMoi });
                }
            }
            return RedirectToAction("Information", "Flight", new { thongbao = "không lấy được dữ liệu chuyến bay  " });
        }
        public ActionResult PaymentMomo(Ve ve)
        {
            string link = "https://localhost:44378/Home/DatThanhCong/" + Session["idVeMoi"];
            //request params need to request to MoMo system
            string endpoint = "https://test-payment.momo.vn/gw_payment/transactionProcessor";
            string partnerCode = "MOMOOJOI20210710";
            string accessKey = "iPXneGmrJH0G8FOP";
            string serectkey = "sFcbSGRSJjwGxwhhcEktCHWYUuTuPNDB";
            string orderInfo = "Thanh toán momo";
            string returnUrl = "https://localhost:44378/Flight/ConfirmPaymentClient";
            string notifyurl = "https://4c8d-2001-ee0-5045-50-58c1-b2ec-3123-740d.ap.ngrok.io/Home/SavePayment"; //lưu ý: notifyurl không được sử dụng localhost, có thể sử dụng ngrok để public localhost trong quá trình test

            string amount = Session["giathanhtoan"].ToString();
            string orderid = "0000000000000"+ Session["idVeMoi"]; //mã đơn hàng
            string requestId = Session["idVeMoi"].ToString();
            string extraData = "";

            //Before sign HMAC SHA256 signature
            string rawHash = "partnerCode=" +
                partnerCode + "&accessKey=" +
                accessKey + "&requestId=" +
                requestId + "&amount=" +
                amount + "&orderId=" +
                orderid + "&orderInfo=" +
                orderInfo + "&returnUrl=" +
                returnUrl + "&notifyUrl=" +
                notifyurl + "&extraData=" +
                extraData;

            MoMoSecurity crypto = new MoMoSecurity();
            //sign signature SHA256
            string signature = crypto.signSHA256(rawHash, serectkey);

            //build body json request
            JObject message = new JObject
            {
                { "partnerCode", partnerCode },
                { "accessKey", accessKey },
                { "requestId", requestId },
                { "amount", amount },
                { "orderId", orderid },
                { "orderInfo", orderInfo },
                { "returnUrl", returnUrl },
                { "notifyUrl", notifyurl },
                { "extraData", extraData },
                { "requestType", "captureMoMoWallet" },
                { "signature", signature }

            };

            string responseFromMomo = PaymentRequest.sendPaymentRequest(endpoint, message.ToString());

            JObject jmessage = JObject.Parse(responseFromMomo);

            return Redirect(jmessage.GetValue("payUrl").ToString());
        }
        //Khi thanh toán xong ở cổng thanh toán Momo, Momo sẽ trả về một số thông tin, trong đó có errorCode để check thông tin thanh toán
        //errorCode = 0 : thanh toán thành công (Request.QueryString["errorCode"])
        //Tham khảo bảng mã lỗi tại: https://developers.momo.vn/#/docs/aio/?id=b%e1%ba%a3ng-m%c3%a3-l%e1%bb%97i
        public ActionResult ConfirmPaymentClient(ResultMomo result)
        {
            //lấy kết quả Momo trả về và hiển thị thông báo cho người dùng (có thể lấy dữ liệu ở đây cập nhật xuống db)
            string rMessage = result.message;
            string idmoi = result.requestId;
            int rOrderId = int.Parse(Session["idVeMoi"].ToString());
            string rErrorCode = result.errorCode; // = 0: thanh toán thành công

            if (rErrorCode == "0")
            {
                var ve = db.Ve.FirstOrDefault(p => p.IDve == rOrderId);
                ve.Tinhtrang = "Đã thanh toán";
                Session["TinhTrang"] = ve.Tinhtrang;
                db.Ve.AddOrUpdate(ve);
                db.SaveChanges();
                string content = System.IO.File.ReadAllText(Server.MapPath("~/Content/template/EmailVe.html"));
                content = content.Replace("{{CustomerName}}", Session["namelh"].ToString());
                content = content.Replace("{{MaVe}}", Session["idVeMoi"].ToString());
                content = content.Replace("{{DateHour}}", Session["DateHour"].ToString());
                content = content.Replace("{{Departure}}", Session["Departure"].ToString());
                content = content.Replace("{{Destination}}", Session["Destination"].ToString());
                content = content.Replace("{{Bag}}", Session["KgHanhly"].ToString());
                var toEmail = ConfigurationManager.AppSettings["ToEmailAddress"].ToString();

                new MailHelper().SendMail(Session["emaillh"].ToString(), "Bạn đã đặt vé thành công tại AirplaneTicket", content);
                //new MailHelper().SendMail(toEmail, "Đơn hàng mới từ AirplaneTicket", content);
                return RedirectToAction("DatThanhCong", "Home", new { id = ve.IDve });
            }

            else
            {
                return RedirectToAction("DatThanhCong", "Home", new { id = idmoi });
            }
            return View();
        }
       public ActionResult PaymentVnpay(Ve ve)
        {
            string url = ConfigurationManager.AppSettings["Url"];
            string returnUrl = ConfigurationManager.AppSettings["ReturnUrl"];
            string tmnCode = ConfigurationManager.AppSettings["TmnCode"];
            string hashSecret = ConfigurationManager.AppSettings["HashSecret"];

            PayLib pay = new PayLib();

            pay.AddRequestData("vnp_Version", "2.1.0"); //Phiên bản api mà merchant kết nối. Phiên bản hiện tại là 2.1.0
            pay.AddRequestData("vnp_Command", "pay"); //Mã API sử dụng, mã cho giao dịch thanh toán là 'pay'
            pay.AddRequestData("vnp_TmnCode", tmnCode); //Mã website của merchant trên hệ thống của VNPAY (khi đăng ký tài khoản sẽ có trong mail VNPAY gửi về)
            pay.AddRequestData("vnp_Amount", Session["giathanhtoanvnpay"].ToString()); //số tiền cần thanh toán, công thức: số tiền * 100 - ví dụ 10.000 (mười nghìn đồng) --> 1000000
            pay.AddRequestData("vnp_BankCode", ""); //Mã Ngân hàng thanh toán (tham khảo: https://sandbox.vnpayment.vn/apis/danh-sach-ngan-hang/), có thể để trống, người dùng có thể chọn trên cổng thanh toán VNPAY
            pay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss")); //ngày thanh toán theo định dạng yyyyMMddHHmmss
            pay.AddRequestData("vnp_CurrCode", "VND"); //Đơn vị tiền tệ sử dụng thanh toán. Hiện tại chỉ hỗ trợ VND
            pay.AddRequestData("vnp_IpAddr", Util.GetIpAddress()); //Địa chỉ IP của khách hàng thực hiện giao dịch
            pay.AddRequestData("vnp_Locale", "vn"); //Ngôn ngữ giao diện hiển thị - Tiếng Việt (vn), Tiếng Anh (en)
            pay.AddRequestData("vnp_OrderInfo", "Thanh toan don hang"); //Thông tin mô tả nội dung thanh toán
            pay.AddRequestData("vnp_OrderType", "other"); //topup: Nạp tiền điện thoại - billpayment: Thanh toán hóa đơn - fashion: Thời trang - other: Thanh toán trực tuyến
            pay.AddRequestData("vnp_ReturnUrl", returnUrl); //URL thông báo kết quả giao dịch khi Khách hàng kết thúc thanh toán
            pay.AddRequestData("vnp_TxnRef", Session["idVeMoi"].ToString()); //mã hóa đơn

            string paymentUrl = pay.CreateRequestUrl(url, hashSecret);

            return Redirect(paymentUrl);
        }
        public ActionResult PaymentConfirm()
        {
            if (Request.QueryString.Count > 0)
            {
                string hashSecret = ConfigurationManager.AppSettings["HashSecret"]; //Chuỗi bí mật
                var vnpayData = Request.QueryString;
                PayLib pay = new PayLib();

                //lấy toàn bộ dữ liệu được trả về
                foreach (string s in vnpayData)
                {
                    if (!string.IsNullOrEmpty(s) && s.StartsWith("vnp_"))
                    {
                        pay.AddResponseData(s, vnpayData[s]);
                    }
                }

                long orderId = Convert.ToInt64(pay.GetResponseData("vnp_TxnRef")); //mã hóa đơn
                long vnpayTranId = Convert.ToInt64(pay.GetResponseData("vnp_TransactionNo")); //mã giao dịch tại hệ thống VNPAY
                string vnp_ResponseCode = pay.GetResponseData("vnp_ResponseCode"); //response code: 00 - thành công, khác 00 - xem thêm https://sandbox.vnpayment.vn/apis/docs/bang-ma-loi/
                string vnp_SecureHash = Request.QueryString["vnp_SecureHash"]; //hash của dữ liệu trả về

                bool checkSignature = pay.ValidateSignature(vnp_SecureHash, hashSecret); //check chữ ký đúng hay không?

                if (checkSignature)
                {
                    if (vnp_ResponseCode == "00")
                    {
                        if(Session["idVeKH"] != null)
                        {
                            int idveve = int.Parse(Session["idVeKH"].ToString());
                            var veve = db.Ve.FirstOrDefault(p => p.IDve == idveve);
                            veve.Tinhtrang = "Đã thanh toán";
                            Session["TinhTrang"] = veve.Tinhtrang;
                            Session["payment"] = "Thanh toán vnpay";
                            db.Ve.AddOrUpdate(veve);
                            db.SaveChanges();
                        }
                        var ve = db.Ve.FirstOrDefault(p => p.IDve == orderId);
                        ve.Tinhtrang = "Đã thanh toán";
                        Session["TinhTrang"] = ve.Tinhtrang;
                        Session["payment"] = "Thanh toán vnpay";
                        db.Ve.AddOrUpdate(ve);
                        db.SaveChanges();
                        if (Session["idVeKH"] != null)
                        {
                            string content = System.IO.File.ReadAllText(Server.MapPath("~/Content/template/ChoThanhToanKH.html"));
                            content = content.Replace("{{CustomerName}}", Session["namelh"].ToString());
                            content = content.Replace("{{MaVe}}", Session["idVeMoi"].ToString());
                            content = content.Replace("{{MaVeKH}}", Session["idVeKH"].ToString());
                            content = content.Replace("{{DateHour}}", Session["DateHour1"].ToString());
                            content = content.Replace("{{DateHour2}}", Session["DateHour2"].ToString());
                            content = content.Replace("{{Departure}}", Session["Departure"].ToString());
                            content = content.Replace("{{Destination}}", Session["Destination"].ToString());
                            content = content.Replace("{{Bag}}", Session["KgHanhly"].ToString());
                            content = content.Replace("{{BagKH}}", Session["KgHanhlyHK"].ToString());
                            content = content.Replace("{{idchuyenbay}}", Session["idchuyenbay"].ToString());
                            content = content.Replace("{{TenHang}}", Session["TenHang"].ToString());
                            content = content.Replace("{{TenHangKH}}", Session["TenHangKH"].ToString());
                            var toEmail = ConfigurationManager.AppSettings["ToEmailAddress"].ToString();
                            new MailHelper().SendMail(Session["emaillh"].ToString(), "Bạn đã đặt vé tại AirplaneTicket", content);
                        }
                        else
                        {
                            string content = System.IO.File.ReadAllText(Server.MapPath("~/Content/template/ChoThanhToan.html"));
                            content = content.Replace("{{CustomerName}}", Session["namelh"].ToString());
                            content = content.Replace("{{MaVe}}", Session["idVeMoi"].ToString());
                            content = content.Replace("{{DateHour}}", Session["DateHour1"].ToString());
                            content = content.Replace("{{Departure}}", Session["Departure"].ToString());
                            content = content.Replace("{{Destination}}", Session["Destination"].ToString());
                            content = content.Replace("{{Bag}}", Session["KgHanhly"].ToString());
                            content = content.Replace("{{idchuyenbay}}", Session["idchuyenbay"].ToString());
                            content = content.Replace("{{TenHang}}", Session["TenHang"].ToString());
                            var toEmail = ConfigurationManager.AppSettings["ToEmailAddress"].ToString();
                            new MailHelper().SendMail(Session["emaillh"].ToString(), "Bạn đã đặt vé tại AirplaneTicket", content);
                        }
                        return RedirectToAction("DatThanhCong", "Home", new { id = ve.IDve });
                    }
                    else
                    {
                        return RedirectToAction("DatThanhCong", "Home", new { id = orderId });
                    }
                }
                else
                {
                    ViewBag.Message = "Có lỗi xảy ra trong quá trình xử lý";
                }
            }

            return View();
        }
    }
} 
