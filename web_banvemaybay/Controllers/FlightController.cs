using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Threading.Tasks;
using System.Web.Helpers;
using System.Web.Mvc;
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
        /*  public async Task<ActionResult> SearchFlight(string to, string from, DateTime dateto, DateTime? datefrom)
          {

              HttpClient client = new HttpClient();
              string apiKey = "b7878674755fde7fab824400c50ce94d"; // API key của AviationStack
              string apiUrl = $"http://api.aviationstack.com/v1/flights?access_key={apiKey}&dep_iata={from}&arr_iata={to}&date={dateto.ToString("yyyy-MM-dd")}";

              HttpResponseMessage response = await client.GetAsync(apiUrl);
              if (response.IsSuccessStatusCode)
              {
                  string jsonString = await response.Content.ReadAsStringAsync();
                  dynamic result = JsonConvert.DeserializeObject(jsonString);

                  // Kiểm tra xem API có trả về kết quả hay không
                  if (result.success && result.data != null)
                  {
                      // Lấy dữ liệu từ API và hiển thị lên View
                      var flights = result.data;
                      return View(flights);
                  }
                  else
                  {
                      // Xử lý khi không tìm thấy chuyến bay nào
                      return RedirectToAction("Home", "Home", new { thongbao = "Không tìm thấy chuyến bay phù hợp" });
                  }
              }
              else
              {
                  // Xử lý khi gọi API không thành công
                  return RedirectToAction("Home", "Home", new { thongbao = "Đã xảy ra lỗi trong quá trình tìm kiếm chuyến bay" });
              }
          }*/

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Sohangve(int idHangve,int idChuyenBay,int? idHangvecu)
        {
            int nguoidat = int.Parse(Session["soluong"].ToString());
            int? idGhecu = idHangve;
            var idHv = db.Hangve.Where(c => c.IDhangve == idHangve).FirstOrDefault();
            var tt = db.Chuyenbay.Where(c => c.IDchuyenbay == idChuyenBay).FirstOrDefault();
            var sl =  new Chuyenbay();
            double giatienidcu = double.Parse(Session["giatien" + tt.IDchuyenbay].ToString());
            Session["idHangve"] = idHangve;
            if ( tt.PhoThong < nguoidat && idHangve == 2)
            {
                double tongtien = (giatienidcu) - (tt.Giatien / 100 * 10 + 70000);
                TempData["ErrorMessage"] = "Hết chỗ trống vui lòng chọn lại  !";
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
                        double tongtien = ((giatienidcu - hanhcu.Gia * nguoidat) + idHv.Gia *nguoidat) - (tt.Giatien / 100 * 10 + 70000);
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
                    flights = flights.Where(c => c.Ngaydi > now && c.Ngaydi > nowPlusOneHour && c.Ngayve == datefromValue && (c.PhoThong > 0 || c.ThuongGia > 0) && (c.PhoThong >= (nl + te + eb) || c.ThuongGia >= (nl + te + eb)));
                }
                else if (datefromValue <= dateto)
                {
                    return RedirectToAction("Home", "Home", new { thongbao = "Vui lòng chọn Ngày về cách ngày đi ít nhất 1 ngày " });
                }
                flights = flights.Where(c => c.Ngaydi != null && SqlFunctions.DateDiff("day", c.Ngaydi, datetoValue) == 0 && SqlFunctions.DateDiff("day", c.Ngayve, datefromValue) == 0 && ( c.PhoThong > 0 || c.ThuongGia > 0) && (c.PhoThong >=( nl + te + eb) || c.ThuongGia >= (nl + te + eb)));
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
                    flights = flights.Where(c => c.Ngaydi > now && c.Ngaydi > nowPlusOneHour && c.Ngayve == null &&( c.PhoThong > 0 || c.ThuongGia > 0) && (c.PhoThong >= (nl + te + eb) || c.ThuongGia >= (nl + te + eb)));
                }
                // Hiển thị chuyến bay có ngày đi và không có ngày về
                flights = flights.Where(c => c.Ngaydi != null && SqlFunctions.DateDiff("day", c.Ngaydi, datetoValue) == 0 && c.Ngayve == null && (c.PhoThong > 0 || c.ThuongGia > 0) && (c.PhoThong >=(nl + te + eb) || c.ThuongGia >= (nl + te + eb)));
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
                else if (valueNL == 0 && valueTE != 0 && valueEB != 0)
                {
                    flight.Giatien = ((0.25 * giaVe) * valueEB) + ((0.75 * giaVe) * valueTE);

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
            return View(flights);
        }

        public ActionResult Information(int id, int? idhanhli, int? idcu)
        {
            string errorMessage = TempData["ErrorMessage"] as string;
            ViewBag.ErrorMessage = errorMessage;
            int? idhanhlicu = idhanhli;
            var tt = db.Chuyenbay.Where(c => c.IDchuyenbay == id).FirstOrDefault();
            var idhl = db.Hanhli.Where(c => c.IDhanhli == idhanhli).FirstOrDefault();
            double giatienidcu = double.Parse(Session["giatien" + tt.IDchuyenbay].ToString());
            if (idcu != null) // Kiểm tra idhanhlicu có tồn tại
            {
                var hanhcu = db.Hanhli.Where(hl => hl.IDhanhli == idcu).FirstOrDefault();
                if (hanhcu != null)
                {
                   double tongtien = (giatienidcu - hanhcu.Giatien) + idhl.Giatien;
                    // Lưu lại tổng tiền vào session
                    Session["giahanhly"] = idhl.Giatien;
                    Session["giatien" + tt.IDchuyenbay] = tongtien;
                    Session["idcu"] = idhanhlicu;
                    Session["giathanhtoan"] = Math.Round(tongtien);
                    Session["giathanhtoanvnpay"] = Math.Round(tongtien * 100);

                    // Chuyển hướng đến action "Information" trong controller "Flight"
                    return View(tt);
                }
            }
            if (idhanhli != null)
            {
                double gia = double.Parse(Session["giatien" + tt.IDchuyenbay].ToString());
                double tong = gia;
                Session["giatien" + tt.IDchuyenbay] = tong;
                double tongtien = tong+ idhl.Giatien ;
                Session["giatien" + tt.IDchuyenbay] = tongtien;
                Session["giathanhtoan"] = Math.Round(tongtien);
                Session["giathanhtoanvnpay"] = Math.Round(tongtien * 100);
                Session["giahanhly"] = idhl.Giatien;
            }
            else
            {
                double gia = double.Parse(Session["giatien" + tt.IDchuyenbay].ToString());
                double tong = gia + tt.Giatien / 100 * 10 + 70000;
                Session["giatien" + tt.IDchuyenbay] = tong;
                Session["giathanhtoan"] = Math.Round(tong);
                Session["giathanhtoanvnpay"] = Math.Round(tong * 100);


            }
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
        public ActionResult tt(FormCollection form, TTlienhe lienhe, Hanhkhach hk , string name, DateTime? birthday, int? idHangve, int? sdtlh, string emaillh, string gioitinh, string namelh, string gioitinhlh, int? idcu, int idchuyenbay, double giatien, string payment)
        {
            var now = DateTime.Now;
            if (birthday > now.AddYears(-18))
            {
                TempData["ErrorMessage"] = "Chưa đủ 18 tuổi vui lòng chọn  lại  !";
                return RedirectToAction("Information", "Flight", new {id = idchuyenbay});
            }
            if (name != null && birthday != null && sdtlh != null && emaillh != null && gioitinh != null && gioitinhlh != null && namelh != null)
            {
                     var ktlh = new TTlienhe();
                    ktlh.FullName = namelh;
                    ktlh.Phone = sdtlh;
                    ktlh.Email = emaillh;
                    db.TTlienhe.Add(ktlh);
                    db.SaveChanges();

                var kt = new Hanhkhach();
                kt.Tenhanhkhach = name;
                kt.NgaySinh = birthday;
                kt.Gioitinh = gioitinh;
                db.Hanhkhach.Add(kt);
                db.SaveChanges();
                string Bag = "Không hành lý!";
                string DateHour = "";
                string Departure = "";
                string Destination = "";
                if (idchuyenbay != null)
                {
                    var ve = new Ve();
                    ve.IDchuyenbay = idchuyenbay;
                    ve.IDhanhkhach = kt.IDhanhkhach;
                    ve.IDlienhe = ktlh.IDlienhe;
                    ve.IDhangve = idHangve;
                    var slhang = db.Chuyenbay.FirstOrDefault(cb => cb.IDchuyenbay == idchuyenbay);
                    if (slhang!=null)
                    {
                        if (idHangve==null)
                        {
                            slhang.PhoThong -= 1;
                            ve.IDhanhli = 2;
                        }
                        if (idHangve==1)
                        {
                            slhang.ThuongGia -= 1;
                            ve.IDhanhli = 1;
                        }
                        if (idHangve == 2)
                        {
                            slhang.PhoThong -= 1;
                            ve.IDhanhli = 2;
                        }

                        db.SaveChanges();
                    }
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
                    Departure = chuyenbay.Diadiemdi.ToString();
                    Destination = chuyenbay.Diadiemden.ToString();
                    db.Ve.Add(ve);
                    db.SaveChanges();
                    int idVeMoi = ve.IDve;
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
                    if (payment == "payment1")
                    {
                        Ve veMomo = db.Ve.Find(idVeMoi);
                        return RedirectToAction("PaymentMomo", "Flight", veMomo);
                    }
                    string content = System.IO.File.ReadAllText(Server.MapPath("~/Content/template/EmailVe.html"));
                    content = content.Replace("{{CustomerName}}", namelh);
                    content = content.Replace("{{MaVe}}", ve.IDve.ToString());
                    content = content.Replace("{{DateHour}}", DateHour);
                    content = content.Replace("{{Departure}}", Departure);
                    content = content.Replace("{{Destination}}", Destination);
                    content = content.Replace("{{Bag}}", Bag);
                    var toEmail = ConfigurationManager.AppSettings["ToEmailAddress"].ToString();

                    new MailHelper().SendMail(emaillh, "Bạn đã đặt vé thành công tại AirplaneTicket", content);
                    new MailHelper().SendMail(toEmail, "Đơn hàng mới từ AirplaneTicket", content);
                    //return View(db.Ve);
                    return RedirectToAction("DatThanhCong", "Home", new { id = idVeMoi });
                }
            }
            return RedirectToAction("Information", "Flight", new { thongbao = "không lấy được dữ liệu chuyến bay  " });
        }
     /*   [HttpPost]*/
     /*   public ActionResult tongtien(int idhanhli, int idChuyenBay, int? idcu, string name, DateTime? birthday, int? sdtlh, string emaillh, string gioitinh, string namelh, string gioitinhlh)
        {
            string giaTriNamelh = Request.Form["namelh"];

            int? idhanhlicu = idhanhli;

            var hanhli = db.Hanhli.Where(hl => hl.IDhanhli == idhanhli).FirstOrDefault();
            double gia = double.Parse(Session["giatien" + idChuyenBay].ToString());
            double giatienidcu = double.Parse(Session["giatien" + idChuyenBay + Session["idhanhli"]].ToString());
            double tongtien;
            if (idcu != null) // Kiểm tra idhanhlicu có tồn tại
            {
                var hanhcu = db.Hanhli.Where(hl => hl.IDhanhli == idcu).FirstOrDefault();
                if (hanhcu != null)
                {
                    tongtien = (giatienidcu - hanhcu.Giatien) + hanhli.Giatien;
                    // Lưu lại tổng tiền vào session
                    Session["giahanhly"] = hanhli.Giatien;
                    Session["giatien" + idChuyenBay] = tongtien;
                    Session["idcu"] = idhanhlicu;
                    Session["giathanhtoan"] = Math.Round(tongtien);
                    Session["giathanhtoanvnpay"] = Math.Round(tongtien * 100);

                    // Chuyển hướng đến action "Information" trong controller "Flight"
                    return RedirectToAction("Information", "Flight", new { id = idChuyenBay });
                }
            }
            // Lấy thông tin từ cơ sở dữ liệu
            if (hanhli != null)
            {
                // Tính tổng tiền mới dựa trên giá trị của hl.Giatien

                tongtien = hanhli.Giatien + gia + (gia / 100 * 10) + 70000;

                // Lưu lại tổng tiền vào session
                Session["giahanhly"] = hanhli.Giatien;
                Session["giatien" + idChuyenBay] = tongtien;
                Session["giathanhtoan"] = Math.Round(tongtien);
                Session["giathanhtoanvnpay"] = Math.Round(tongtien * 100);
                Session["idcu"] = idhanhlicu;

                // Chuyển hướng đến action "Information" trong controller "Flight"
                return RedirectToAction("Information", "Flight", new { id = idChuyenBay });
            }
            else
            {
                // Xử lý lỗi khi không tìm thấy hl.Giatien dựa trên idhanhli
                // Nếu cần, bạn có thể thêm mã lỗi hoặc thông báo lỗi tùy vào yêu cầu của dự án
                return View("Error");
            }
        }*/
        public ActionResult PaymentMomo(Ve veMomo)
        {
            //request params need to request to MoMo system
            string endpoint = "https://test-payment.momo.vn/gw_payment/transactionProcessor";
            string partnerCode = "MOMOOJOI20210710";
            string accessKey = "iPXneGmrJH0G8FOP";
            string serectkey = "sFcbSGRSJjwGxwhhcEktCHWYUuTuPNDB";
            string orderInfo = "Thanh toán momo";
            string returnUrl = RedirectToAction("DatThanhCong", "Home", new { id = Session["idVeMoi"] }).ToString();
            string notifyurl = "https://4c8d-2001-ee0-5045-50-58c1-b2ec-3123-740d.ap.ngrok.io/Home/SavePayment"; //lưu ý: notifyurl không được sử dụng localhost, có thể sử dụng ngrok để public localhost trong quá trình test

            string amount = Session["giathanhtoan"].ToString();
            string orderid = "0000000000000"+ veMomo.IDve.ToString(); //mã đơn hàng
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
            int rOrderId = int.Parse(result.orderId);
            string rErrorCode = result.errorCode; // = 0: thanh toán thành công
            int code = Convert.ToInt32(rErrorCode);

            if (code == 0)
            {
                Ve ve = db.Ve.Where(p => p.IDve == rOrderId).FirstOrDefault();
                if (ve != null)
                {
                    ve.Tinhtrang = "Da thanh toan";
                }
                db.SaveChanges();
                return RedirectToAction("DatThanhCong", "Home", new { id = ve.IDve });
            }

            else
            {
                ViewBag.Message = "Có lỗi xảy ra trong quá trình xử lý";
            }
            return View();
        }
        public ActionResult PaymentVnpay()
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
            pay.AddRequestData("vnp_TxnRef", DateTime.Now.Ticks.ToString()); //mã hóa đơn

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
                        //Thanh toán thành công
                        ViewBag.Message = "Thanh toán thành công hóa đơn " + orderId + " | Mã giao dịch: " + vnpayTranId;
                    }
                    else
                    {
                        //Thanh toán không thành công. Mã lỗi: vnp_ResponseCode
                        ViewBag.Message = "Có lỗi xảy ra trong quá trình xử lý hóa đơn " + orderId + " | Mã giao dịch: " + vnpayTranId + " | Mã lỗi: " + vnp_ResponseCode;
                    }
                }
                else
                {
                    ViewBag.Message = "Có lỗi xảy ra trong quá trình xử lý";
                }
            }

            return View();
        }
        /*      // Action xử lý yêu cầu tìm kiếm chuyến bay
              [HttpPost]
              public async Task<ActionResult> TimKiem(string origin, string destination, DateTime departureDate)
              {
                  try
                  {
                       Kiểm tra giá trị hợp lệ của các tham số
                      if (string.IsNullOrEmpty(origin) || string.IsNullOrEmpty(destination) || departureDate == null)
                      {
                          ViewBag.ErrorMessage = "Vui lòng nhập đầy đủ thông tin.";
                          return View("Index");
                      }

                       Gọi API của Kiwi để lấy dữ liệu chuyến bay
                      string apiKey = "O7OALE3za9k0VlVkIkEy8EggsjU6wMR8";
                      string apiUrl = "https://api.skypicker.com/flights";
                      string searchUrl = $"{apiUrl}?flyFrom={origin}&to={destination}&dateFrom={departureDate.ToString("dd/MM/yyyy")}&dateTo={departureDate.ToString("dd/MM/yyyy")}&apiKey={apiKey}";
                      using (HttpClient client = new HttpClient())
                      {
                          HttpResponseMessage response = await client.GetAsync(searchUrl);
                          string json = await response.Content.ReadAsStringAsync();

                           Kiểm tra dữ liệu trả về từ API của Kiwi
                          dynamic flightData = JsonConvert.DeserializeObject(json);
                          if (flightData == null || flightData.data == null || flightData.data.Length == 0)
                          {
                              ViewBag.ErrorMessage = "Không tìm thấy thông tin chuyến bay.";
                              return View("Index");
                          }

                           Chuyển đổi dữ liệu json thành đối tượng Chuyenbay
                          List<Chuyenbay> flights = new List<Chuyenbay>();
                          foreach (var flight in flightData.data)
                          {
                              flights.Add(new Chuyenbay
                              {
                                  Diadiemdi = flight.cityFrom,
                                  Diadiemden = flight.cityTo,
                                  Batdau = DateTime.Parse(flight.local_departure),
                                  Ketthuc = DateTime.Parse(flight.local_arrival),
                              });
                          }

                           Trả về kết quả cho người dùng
                          return View("Index", flights);
                      }
                  }
                  catch (Exception ex)
                  {
                      ViewBag.ErrorMessage = "Có lỗi xảy ra: " + ex.Message; // Hiển thị thông báo lỗi chi tiết
                      return View("Index");
                  }
              }
      */



    }
}
