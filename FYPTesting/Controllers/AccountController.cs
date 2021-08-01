using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Data;
using System.Security.Claims;
using FYPTesting.Models;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Net.Mail;
using System.Net;

namespace FYPTesting.Controllers
{

    public class AccountController : Controller
    {
        private const string LOGIN_SQL =
           @"SELECT * FROM AdminLogin 
            WHERE UserId = '{0}' 
              AND UserPw = HASHBYTES('SHA1', '{1}')";

        private const string LASTLOGIN_SQL =
           @"UPDATE AdminLogin SET LastLogin=GETDATE() WHERE UserId='{0}'";

        private const string StartWeek_SQL =
           @"UPDATE AdminLogin SET StartWeek=GETDATE('Today') WHERE UserId='{0}'";

        private const string EndWeek_SQL =
            @"UPDATE AdminLogin SET EndWeek=GETDATE() + 6 WHERE UserId='{0}'";


        private const string ROLE_COL = "UserRole";
        private const string NAME_COL = "FullName";

        private const string REDIRECT_CNTR = "Menu";
        private const string REDIRECT_ACTN = "AdminView";
        private const string REDIRECT_ACTNA = "SupplierView";
        private const string REDIRECT_ACTNB = "PurchaserView";
        private const string REDIRECT_ACTNC = "SalesManMenu";

        private const string LOGIN_VIEW = "MemberLogin";


        [Authorize(Roles = "salesman, purchaser")]
        public IActionResult About()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult Login(string returnUrl = null)
        {
            TempData["ReturnUrl"] = returnUrl;
            return View(LOGIN_VIEW);
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login(MemberLogin member)
        {
            if (User.IsInRole("admin"))
            {
                if (!AuthenticateUser(member.UserID, member.Password, out ClaimsPrincipal principal))
                {
                    ViewData["Message"] = "Incorrect User ID or Password";
                    ViewData["MsgType"] = "warning";
                    return View(LOGIN_VIEW);
                }
                else
                {

                    HttpContext.SignInAsync(
                       CookieAuthenticationDefaults.AuthenticationScheme,
                       principal,
                       new AuthenticationProperties
                       {
                           IsPersistent = member.RememberMe
                       });

                    DBUtl.ExecSQL(LASTLOGIN_SQL, member.UserID);

                    if (TempData["returnUrl"] != null)
                    {
                        string returnUrl = TempData["returnUrl"].ToString();
                        if (Url.IsLocalUrl(returnUrl))
                            return Redirect(returnUrl);
                    }

                    return RedirectToAction("AdminView", "Account");
                }

            }


            else if (User.IsInRole("supplier"))
            {
                if (!AuthenticateUser(member.UserID, member.Password, out ClaimsPrincipal principal))
                {
                    ViewData["Message"] = "Incorrect User ID or Password";
                    ViewData["MsgType"] = "warning";
                    return View(LOGIN_VIEW);
                }
                else
                {

                    HttpContext.SignInAsync(
                       CookieAuthenticationDefaults.AuthenticationScheme,
                       principal,
                       new AuthenticationProperties
                       {
                           IsPersistent = member.RememberMe
                       });

                    DBUtl.ExecSQL(LASTLOGIN_SQL, member.UserID);

                    if (TempData["returnUrl"] != null)
                    {
                        string returnUrl = TempData["returnUrl"].ToString();
                        if (Url.IsLocalUrl(returnUrl))
                            return Redirect(returnUrl);
                    }

                    return RedirectToAction("SupplierView", "Menu");
                }

            }
            else if (User.IsInRole("purchaser"))
            {
                if (!AuthenticateUser(member.UserID, member.Password, out ClaimsPrincipal principal))
                {
                    ViewData["Message"] = "Incorrect User ID or Password";
                    ViewData["MsgType"] = "warning";
                    return View(LOGIN_VIEW);
                }
                else
                {

                    HttpContext.SignInAsync(
                       CookieAuthenticationDefaults.AuthenticationScheme,
                       principal,
                       new AuthenticationProperties
                       {
                           IsPersistent = member.RememberMe
                       });

                    DBUtl.ExecSQL(LASTLOGIN_SQL, member.UserID);

                    if (TempData["returnUrl"] != null)
                    {
                        string returnUrl = TempData["returnUrl"].ToString();
                        if (Url.IsLocalUrl(returnUrl))
                            return Redirect(returnUrl);
                    }

                    return RedirectToAction("PurchaserView", "Menu");
                }

            }
            else
            {
                if (!AuthenticateUser(member.UserID, member.Password, out ClaimsPrincipal principal))
                {
                    ViewData["Message"] = "Incorrect User ID or Password";
                    ViewData["MsgType"] = "warning";
                    return View(LOGIN_VIEW);
                }
                else
                {

                    HttpContext.SignInAsync(
                       CookieAuthenticationDefaults.AuthenticationScheme,
                       principal,
                       new AuthenticationProperties
                       {
                           IsPersistent = member.RememberMe
                       });

                    DBUtl.ExecSQL(LASTLOGIN_SQL, member.UserID);

                    if (TempData["returnUrl"] != null)
                    {
                        string returnUrl = TempData["returnUrl"].ToString();
                        if (Url.IsLocalUrl(returnUrl))
                            return Redirect(returnUrl);
                    }

                    return RedirectToAction("SalesManView", "Menu");
                }

            }

        }

        [Authorize]
        public IActionResult Logoff(string returnUrl = null)
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            if (Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            return RedirectToAction("Login", "Account");

        }


        //[Authorize(Roles = "admin")]
        public IActionResult Members()
        {
            List<AdminLogin> list = DBUtl.GetList<AdminLogin>("SELECT * FROM AdminLogin WHERE UserRole='purchaser' OR UserRole='supplier' OR UserRole='salesman'");
            return View(list);
        }


        [Authorize(Roles = "admin")]
        public IActionResult Delete(string id)
        {
            string delete = "DELETE FROM AdminLogin WHERE UserId='{0}'";
            int res = DBUtl.ExecSQL(delete, id);
            if (res == 1)
            {
                TempData["Message"] = "User Deleted";
                TempData["MsgType"] = "success";
            }
            else
            {
                TempData["Message"] = DBUtl.DB_Message;
                TempData["MsgType"] = "danger";
            }

            return RedirectToAction("Members");
        }

        [AllowAnonymous]
        public IActionResult Create()
        {
            return View("UserCreate");
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Create(AdminLogin usr)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Message"] = "Invalid Input";
                ViewData["MsgType"] = "warning";
                return View("UserCreate");
            }
            else
            {
                string insert =
                   @"INSERT INTO AdminLogin(UserId, UserPw, FullName, Email, UserRole) VALUES
                 ('{0}', HASHBYTES('SHA1', '{1}'), '{2}', '{3}', '{4}')";
                if (DBUtl.ExecSQL(insert, usr.UserId, usr.UserPw, usr.FullName, usr.Email, usr.UserRole) == 1)
                {
                    string template = @"Hi {0},<br/><br/>
                               Welcome to TSH Synergy Company!
                               Your userid is <b>{1}</b> and password is <b>{2}</b>.
                               <br/><br/>admin";
                    string title = "Created Successul - Welcome";
                    string message = String.Format(template, usr.FullName, usr.UserId, usr.UserPw);
                    string result = "";
                    if (EmailUtl.SendEmail(usr.Email, title, message, out result))
                    {
                        ViewData["Message"] = "User Successfully Created";
                        ViewData["MsgType"] = "success";
                    }
                    else
                    {
                        ViewData["Message"] = result;
                        ViewData["MsgType"] = "warning";
                    }
                }
                else
                {
                    ViewData["Message"] = DBUtl.DB_Message;
                    ViewData["MsgType"] = "danger";
                }
                return View("UserCreate");
            }
        }

        [AllowAnonymous]
        public IActionResult VerifyUserID(string userId)
        {
            string select = $"SELECT * FROM AdminLogin WHERE Userid='{userId}'";
            if (DBUtl.GetTable(select).Rows.Count > 0)
            {
                return Json($"[{userId}] already in use");
            }
            return Json(true);
        }

        private bool AuthenticateUser(string uid, string pw, out ClaimsPrincipal principal)
        {
            principal = null;

            DataTable ds = DBUtl.GetTable(LOGIN_SQL, uid, pw);
            if (ds.Rows.Count == 1)
            {
                principal =
                   new ClaimsPrincipal(
                      new ClaimsIdentity(
                         new Claim[] {
                        new Claim(ClaimTypes.NameIdentifier, uid),
                        new Claim(ClaimTypes.Name, ds.Rows[0][NAME_COL].ToString()),
                        new Claim(ClaimTypes.Role, ds.Rows[0][ROLE_COL].ToString())
                         }, "Basic"
                      )
                   );
                return true;
            }
            return false;
        }
        [Authorize(Roles = "admin")]
        public ActionResult AdminView()
        {
            List<Menu> list = DBUtl.GetList<Menu>("SELECT * FROM Menu");
            return View(list);
        }
        //Edit PO Admin
        [Authorize(Roles = "admin")]
        public IActionResult EditPOAdmin(int id)
        {
            string select = "SELECT * FROM Menu WHERE Number={0}";
            List<Menu> list = DBUtl.GetList<Menu>(select, id);
            if (list.Count == 1)
            {
                return View(list[0]);
            }
            else
            {
                TempData["Message"] = "Number is not found";
                TempData["MsgType"] = "warning";
                return RedirectToAction("AdminView");
            }
        }
        [Authorize(Roles = "admin")]
        [HttpPost]
        public IActionResult EditPOAdmin(Edit mn)
        {

            if (!ModelState.IsValid)
            {
                ViewData["Message"] = "Invalid Input";
                ViewData["MsgType"] = "warning";
                return View("EditPOAdmin");
            }
            else
            {
                string update =
                   @"UPDATE Menu
                    SET RevisedDelDate='{0:yyyy-MM-dd}'
                  WHERE Number={1}";
                int res = DBUtl.ExecSQL(update, mn.RevisedDelDate, mn.Number);
                if (res == 1)
                {
                    TempData["Message"] = "PO Updated";
                    TempData["MsgType"] = "success";
                }
                else
                {
                    TempData["Message"] = DBUtl.DB_Message;
                    TempData["MsgType"] = "danger";
                }
                return RedirectToAction("AdminView");
            }
        }

        /*public static void Email(string htmlString)
        {
            try
            {
                MailMessage message = new MailMessage();
                SmtpClient smtp = new SmtpClient();
                message.From = new MailAddress("liuxianminxm@gmail.com");
                message.To.Add(new MailAddress("master98oh@gmail.com"));
                message.Subject = "Test";
                message.IsBodyHtml = true;
                message.Body = htmlString;
                smtp.Port = 587;
                smtp.Host = "smtp.gmail.com";
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential();
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Send(message);

            }
            catch (Exception) { }
        }*/

    }
}