﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using web_banvemaybay.Models;
using web_banvemaybay.Others;

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
        public ActionResult SearchFlight(string to, string from, DateTime? dateto, DateTime? datefrom, string searchBy, FormCollection form, float nl, float te, float eb)
        {
            if (dateto == null)
            {
                return RedirectToAction("Home", "Home", new { thongbao = " Chọn Ngày đi! " });
            }
            // Thực hiện tìm kiếm dựa trên các tham số đầu vào
            var flights = db.Chuyenbay.Where(c => c.Diadiemdi.ToLower().Contains(to.ToLower()) && c.Diadiemden.ToLower().Contains(from.ToLower()));
            if (dateto != null && datefrom != null)
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
                    flights = flights.Where(c => c.Ngaydi > now && c.Ngaydi > nowPlusOneHour && c.Ngayve >= datefromValue);
                }
                else if (datefromValue <= dateto)
                {
                    return RedirectToAction("Home", "Home", new { thongbao = "Vui lòng chọn Ngày về cách ngày đi ít nhất 1 ngày " });
                }
                flights = flights.Where(c => c.Ngaydi != null && SqlFunctions.DateDiff("day", c.Ngaydi, datetoValue) == 0 && c.Ngayve >= datefromValue);
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
                    flights = flights.Where(c => c.Ngaydi > now && c.Ngaydi > nowPlusOneHour && c.Ngayve == null);
                }
                // Hiển thị chuyến bay có ngày đi và không có ngày về
                flights = flights.Where(c => c.Ngaydi != null && SqlFunctions.DateDiff("day", c.Ngaydi, datetoValue) == 0 && c.Ngayve == null);
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
            Session["dateto"] = form["dateto"];
            Session["datefrom"] = form["datefrom"];
            // Lưu danh sách flights vào Session
            Session["flights"] = flights;
            Session["searchBy"] = searchBy;
            return View(flights);
        }

        public ActionResult Information(int id)
        {
            var tt = db.Chuyenbay.Where(c => c.IDchuyenbay == id).FirstOrDefault();
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
        public ActionResult tt(FormCollection form, TTlienhe lienhe, Hanhkhach hk, string name, DateTime? birthday, int? sdtlh, string emaillh, string gioitinh, string namelh, string gioitinhlh, int? idcu, int idchuyenbay, double giatien)
        {
            if (name != null && birthday != null && sdtlh != null && emaillh != null && gioitinh != null && gioitinhlh != null && namelh != null)
            {
                if (emaillh != null && namelh != null && sdtlh != null)
                {
                    var ktlh = new TTlienhe();
                    ktlh.FullName = namelh;
                    ktlh.Phone = sdtlh;
                    ktlh.Email = emaillh;
                    db.TTlienhe.Add(ktlh);
                    db.SaveChanges();
                }
                var kt = new Hanhkhach();
                kt.Tenhanhkhach = name;
                kt.NgaySinh = birthday;
                kt.Gioitinh = gioitinh;
                db.Hanhkhach.Add(kt);
                db.SaveChanges();
                if (idchuyenbay != null)
                {
                    var ve = new Ve();
                    ve.IDchuyenbay = idchuyenbay;
                    ve.IDhanhkhach = kt.IDhanhkhach;
                    if (idcu == null)
                    {
                        ve.IDhanhli = 1;
                    }
                    ve.IDhanhli = idcu;
                    ve.Ngaydatve = DateTime.Now;
                    ve.Gia = giatien;
                    db.Ve.Add(ve);
                    db.SaveChanges();
                }
                Session["giatien"] = giatien;
                return View(db.Ve);
            }
            else
            {
                return RedirectToAction("Information", "Flight", new { thongbao = "Vui Nhập đầy đủ thông tin! " });
            }
        }
        [HttpPost]
        public ActionResult tongtien(int idhanhli, int idChuyenBay, int? idcu, string name, DateTime? birthday, int? sdtlh, string emaillh, string gioitinh, string namelh, string gioitinhlh)
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
        }
        public ActionResult Payment()
        {
            //request params need to request to MoMo system
            string endpoint = "https://test-payment.momo.vn/gw_payment/transactionProcessor";
            string partnerCode = "MOMOOJOI20210710";
            string accessKey = "iPXneGmrJH0G8FOP";
            string serectkey = "sFcbSGRSJjwGxwhhcEktCHWYUuTuPNDB";
            string orderInfo = "Thanh toán momo";
            string returnUrl = "https://localhost:44394";
            string notifyurl = "https://4c8d-2001-ee0-5045-50-58c1-b2ec-3123-740d.ap.ngrok.io/Home/SavePayment"; //lưu ý: notifyurl không được sử dụng localhost, có thể sử dụng ngrok để public localhost trong quá trình test

            string amount = Session["giathanhtoan"].ToString();
            string orderid = DateTime.Now.Ticks.ToString(); //mã đơn hàng
            string requestId = DateTime.Now.Ticks.ToString();
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
            string rOrderId = result.orderId;
            string rErrorCode = result.errorCode; // = 0: thanh toán thành công
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
