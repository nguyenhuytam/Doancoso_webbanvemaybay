using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Mvc;
using Newtonsoft.Json;
using web_banvemaybay.Models;

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
            public ActionResult SearchFlight(string to, string from, DateTime? dateto, DateTime? datefrom,string searchBy)
            {
                var datetoValue = dateto.Value.Date;
                // Thực hiện tìm kiếm dựa trên các tham số đầu vào
                var flights = db.Chuyenbay.Where(c => c.Diadiemdi.ToLower().Contains(to.ToLower()) && c.Diadiemden.ToLower().Contains(from.ToLower()));

                if (dateto != null && datefrom != null)
                {
                    var datefromValue = datefrom.Value.Date;
                    if (datetoValue < DateTime.Now.Date)
                    {
                        return RedirectToAction("Home", "Home", new { thongbao = "Vui lòng chọn Ngày đi lớn hơn ngày hiện tại hoặc bằng ngày hiện tại " });
                    }
                    else if(datetoValue == DateTime.Now.Date)
                    {
                        var now = DateTime.Now;

                        // Tính toán giờ hiện tại cộng thêm 1 tiếng 30 p 
                        var nowPlusOneHour = now.AddHours(2).AddMinutes(30);

                        // Kiểm tra nếu khoảng thời gian giữa ngày đi và giờ đi của mỗi chuyến bay lớn hơn giờ hiện tại và không quá 1 tiếng, hiển thị kết quả
                        flights = flights.Where(c => c.Ngaydi > now && c.Ngaydi > nowPlusOneHour && c.Ngayve >=datefromValue);
                    }
                    else if (datefromValue <= dateto)
                        {
                            return RedirectToAction("Home", "Home", new { thongbao = "Vui lòng chọn Ngày về cách ngày đi ít nhất 1 ngày " });
                        }
                     flights = flights.Where(c => c.Ngaydi !=null && SqlFunctions.DateDiff("day", c.Ngaydi, datetoValue) == 0 && c.Ngayve>=datefromValue);
                }
                 else if (dateto != null && datefrom == null)
                {
                    if (datetoValue < DateTime.Now.Date)
                        {
                            return RedirectToAction("Home", "Home", new { thongbao = "Vui lòng chọn Ngày đi lớn hơn ngày hiện tại hoặc bằng ngày hiện tại " });
                        }
                    if (datetoValue == DateTime.Now.Date)
                    {
                         var now = DateTime.Now;

                            // Tính toán giờ hiện tại cộng thêm 1 tiếng 30 p
                         var nowPlusOneHour = now.AddHours(2).AddMinutes(30);

                        // Kiểm tra nếu khoảng thời gian giữa ngày đi và giờ đi của mỗi chuyến bay lớn hơn giờ hiện tại và không quá 1 tiếng, hiển thị kết quả
                         flights = flights.Where(c => c.Ngaydi > now && c.Ngaydi > nowPlusOneHour && c.Ngayve == null);
                     }
                    // Hiển thị chuyến bay có ngày đi và không có ngày về
                    flights = flights.Where(c => c.Ngaydi != null && SqlFunctions.DateDiff("day", c.Ngaydi, datetoValue) == 0 && c.Ngayve ==null);
                  }
            // Lấy giá trị của radio button đã chọn
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
                    flights = flights.OrderByDescending(c=>c.Giatien);
                }
                else
                {
                    // Sắp xếp tăng dần theo thuộc tính Giatien
                    flights = flights.OrderBy(c => c.Giatien);
                }
            }

                return View(flights);
            }

        public ActionResult Information(int id)
        {
            var tt = db.Chuyenbay.Where(c => c.IDchuyenbay == id).FirstOrDefault();
            return View(tt);
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
