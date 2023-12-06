﻿using Facebook;
using System;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Web.Helpers;
using System.Web.Mvc;
using web_banvemaybay.Models;


namespace web_banvemaybay.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Register(string thongbao)
        {
            ViewBag.thongbao = thongbao;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(TaiKhoan user)
        {
            if (ModelState.IsValid)
            {
                if (user.Password != user.ConfirmPassword)
                {
                    return RedirectToAction("Register", new { thongbao = "Mật khẩu không khớp" });
                }
                web_banvemaybayEntities db = new web_banvemaybayEntities();
                var existingUser = db.TaiKhoan.FirstOrDefault(c => c.Email == user.Email);
                if (existingUser == null)
                {
                    // Mã hóa mật khẩu trước khi lưu vào CSDL\
                    user.Password = MD5Hash(user.Password);
                    db.TaiKhoan.Add(user);
                    db.SaveChanges();
                    return RedirectToAction("Login", "Login");
                }
                else
                {
                    return RedirectToAction("Register", new { thongbao = "Email đã tồn tại " });

                }
            }
            else
            {
                return RedirectToAction("Register", new { thongbao = "Email đã tồn tại " });
            }
        }
        public ActionResult Login(string thongbao)
        {
            ViewBag.thongbao = thongbao;
            return View();
        }
        [HttpPost]
        public ActionResult Login(string tenDN, string mkDN)
        {
            Session["EmailTaiKhoan"] = tenDN;
            web_banvemaybayEntities db = new web_banvemaybayEntities();
            mkDN = MD5Hash(mkDN);
            var kt = db.TaiKhoan.Where(c => c.Email.Equals(tenDN) && c.Password.Equals(mkDN)).ToList();
            if (kt.Count > 0)
            {
                Session["HovaTen"] = kt.FirstOrDefault().HovaTen;
                Session["Email"] = kt.FirstOrDefault().Email;
                Session["IDtaikhoan"] = kt.FirstOrDefault().IDtaikhoan;
                Session["Password"] = kt.FirstOrDefault().Password;
                Session["SDT"] = kt.FirstOrDefault().SDT;
                Session["IDchucvu"] = kt.FirstOrDefault().IDchucvu;
                if (kt.FirstOrDefault().IDchucvu == 1)
                {
                    Session["isAdmin"] = true;
                }
                else
                {
                    Session["isAdmin"] = false;
                }
                if (Session["ReturnUrl"] != null)
                {
                    string returnUrl = Session["ReturnUrl"].ToString();
                    Session["ReturnUrl"] = null;
                    return Redirect(returnUrl);
                }
                else
                {
                    return RedirectToAction("Home", "Home", new { id = Session["Hovaten"] });
                }

            }
            else
            {
                return RedirectToAction("Login", new { thongbao = "Tên đăng nhập hoặc mật khẩu không chính xác!" });
            }
        }
        // Hàm mã hóa MD5
        public string MD5Hash(string text)
        {
            MD5 md5 = new MD5CryptoServiceProvider();

            // Chuyển đổi chuỗi thành mảng byte
            byte[] originalBytes = ASCIIEncoding.Default.GetBytes(text);

            // Mã hóa mảng byte
            byte[] encodedBytes = md5.ComputeHash(originalBytes);

            // Chuyển đổi mảng byte đã mã hóa thành chuỗi hexa
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < encodedBytes.Length; i++)
            {
                sb.Append(encodedBytes[i].ToString("x2"));
            }

            return sb.ToString();
        }
        public Uri RedirectUri
        {
            get
            {
                var uriBuilder = new UriBuilder(Request.Url);
                uriBuilder.Query = null;
                uriBuilder.Fragment = null;
                uriBuilder.Path = Url.Action("FacebookCallback");
                return uriBuilder.Uri;
            }
        }
        public ActionResult LoginFacebook()
        {
            var fb = new FacebookClient();
            var loginUrl = fb.GetLoginUrl(new
            {
                client_id = "165338759455700",
                client_secret = "16b78e319110b6099f1921305f828365",
                redirect_uri = RedirectUri.AbsoluteUri,
                response_type = "code",
                scope = "email",
            });
            return Redirect(loginUrl.AbsoluteUri);
        }
        public ActionResult FacebookCallback(string code, TaiKhoan user)
        {
            var fb = new FacebookClient();
            dynamic result = fb.Post("oauth/access_token", new
            {
                client_id = "165338759455700",
                client_secret = "16b78e319110b6099f1921305f828365",
                redirect_uri = RedirectUri.AbsoluteUri,
                code = code
            });

            var accessToken = result.access_token;


            if (accessToken != null)
            {
                fb.AccessToken = accessToken;

                dynamic me = fb.Get("me?fields=first_name,middle_name,last_name,id,email");
                string email = me.email;
                string firstName = me.first_name;
                string middleName = me.middle_name;
                string lastName = me.last_name;
                string phone = me.phone;
                string password = me.password;
                string emailVerified = me.emailVerified;
                string phoneVerified = me.phoneVerified;
                string passwordVerified = me.passwordVerified;

                if (ModelState.IsValid)
                {
                    web_banvemaybayEntities db = new web_banvemaybayEntities();
                    var existingUser = db.TaiKhoan.FirstOrDefault(c => c.Email == email);
                    if (existingUser == null)
                    {
                        // Mã hóa mật khẩu trước khi lưu vào CSDL\
                        user.Email = email;
                        user.HovaTen = lastName + middleName + firstName;
                        db.TaiKhoan.Add(user);
                        db.SaveChanges();
                        Session["HovaTen"] = user.HovaTen;
                        Session["Email"] = user.Email;
                        Session["IDtaikhoan"] = user.IDtaikhoan;
                        Session["Password"] = user.Password;
                        Session["IDchucvu"] = user.IDchucvu;
                    }
                    else
                    {
                        user.Email = email;
                        user.HovaTen = lastName + middleName + firstName;
                        Session["HovaTen"] = user.HovaTen;
                        Session["Email"] = user.Email;
                        Session["IDtaikhoan"] = user.IDtaikhoan;
                        Session["Password"] = user.Password;
                        Session["IDchucvu"] = user.IDchucvu;
                        return RedirectToAction("Home", "Home", new { ten = Session["HovaTen"] });

                    }
                }
                else
                {
                    return RedirectToAction("Home", "Home", new { ten = Session["HovaTen"] });
                }
            }

            return RedirectToAction("Home", "Home");
        }
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Home", "Home");
        }
        public ActionResult Edit()
        {
            web_banvemaybayEntities db = new web_banvemaybayEntities();
            int idNguoidung = (int)Session["IDtaikhoan"];
            TaiKhoan user = db.TaiKhoan.Where(c => c.IDtaikhoan == idNguoidung).FirstOrDefault();
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: DN/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TaiKhoan user)
        {
            if (ModelState.IsValid)
            {
                if (user.Password != user.ConfirmPassword)
                {
                    return RedirectToAction("Edit", new { thongbao = "Mật khẩu không khớp" });
                }

                web_banvemaybayEntities db = new web_banvemaybayEntities();
                int idNguoidung = (int)Session["IDtaikhoan"];
                var edit = db.TaiKhoan.FirstOrDefault(c => c.IDtaikhoan == idNguoidung);
                if (edit != null)
                {
                    edit.HovaTen = user.HovaTen;
                    edit.SDT = user.SDT;
                    edit.Password = MD5Hash(edit.Password);
                    edit.ConfirmPassword = user.ConfirmPassword;
                    db.SaveChanges();
                    return RedirectToAction("Home", "Home", new { id = Session["IDtaikhoan"] });
                }
                else
                {
                    return HttpNotFound();
                }
            }
            else
            {
                ModelState.AddModelError("", "Lỗi");
                return View(user);
            }
        }
        public ActionResult quenmk(string email, string message)
        {
            ViewBag.Message = message;
            return View();

        }
        Random random = new Random();
        int otp;
        [HttpPost]
        public ActionResult SendOTPMk(FormCollection form)
        {
            string email = form["email"];
            if (email != null)
            {
                web_banvemaybayEntities db = new web_banvemaybayEntities();
                var checkma = db.TaiKhoan.Where(c => c.Email == email).FirstOrDefault();
                if (checkma == null)
                {
                    return RedirectToAction("quenmk", "Login", new { message = "Email này chưa được đăng ký" });
                }

            }
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
            ViewBag.Message = "OTP đã được gửi";
            return RedirectToAction("xacnhan", "Login");
        }
        public ActionResult xacnhan(string thongbao)
        {
            ViewBag.thongbao = thongbao;
            return View();
        }
        public ActionResult doimoi(string passwordmoi, string passwordxn)
        {
                web_banvemaybayEntities db = new web_banvemaybayEntities();
                string email = Session["email"].ToString();
                var checkma = db.TaiKhoan.Where(c => c.Email == email).FirstOrDefault();
                if (passwordmoi == passwordxn)
                {
                    checkma.Password = MD5Hash(passwordmoi);
                    db.SaveChanges();
                return RedirectToAction("Login", "Login");
            }
                else
                {
                    return RedirectToAction("Checkmk", "Login", new { message = "Vui lòng nhập đúng mật khẩu đã nhập", passwordmoi = passwordmoi });
                }
            
        }

        public ActionResult Checkmk(string message)
        {
            ViewBag.Message = message;
            return View();
        }

    }
}