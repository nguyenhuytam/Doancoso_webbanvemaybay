﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using web_banvemaybay.Models;
using static System.Net.WebRequestMethods;

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

        public ActionResult Checkve(string OTP)
        {
            string email = Session["email"].ToString();
            int mave =int.Parse(@Session["mavemuontra"].ToString());
            web_banvemaybayEntities db = new web_banvemaybayEntities();
            if (Session["otp"].ToString() == OTP)
            {
                    var checkve = db.Ve.Where(c => c.TTlienhe.Email == email);
                    if (checkve != null)
                    {
                        var checkma = db.Ve.Where(c => c.IDve == mave);
                        if (checkma != null)
                        {
                        return View(checkma.OrderBy(c => c.IDve));
                    }
                    }
            }
            else
            {
                ViewBag.Message = "Bạn nhập sai mã vui lòng kiểm tra lại ";
                return RedirectToAction("xacnhan", "Check", new {thongbao="Bạn nhập sai mã otp vui lòng kiểm tra lại "});
            }
            return View();
        }
        Random random = new Random();
        int otp;
        public ActionResult SendOTP()
        {
            return View();
        }
            [HttpPost]
        public ActionResult SendOTP(FormCollection form)
        {
            string mavemuontra = form["search"];
            string email = form["email"];
            otp = random.Next(100000, 10000000);

            var fromAddress = ConfigurationManager.AppSettings["FromEmailAddress"].ToString();
            var toAddress = new MailAddress(email).ToString();
            string frompass = ConfigurationManager.AppSettings["FromEmailPassword"].ToString();
            const string subject = "OTP code";
            string body = "Mã OTP: " + otp.ToString();

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress, frompass),
                Timeout = 200000
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body,
            })
            {
                smtp.Send(message);
            }
            Session["otp"] = otp.ToString();
            Session["email"] = email;
            Session["mavemuontra"] = mavemuontra;
            ViewBag.Message = "OTP đã được gửi";
            return RedirectToAction("xacnhan", "Check");
        }
        public ActionResult xacnhan()
        {
            return View();
        }


    }
}