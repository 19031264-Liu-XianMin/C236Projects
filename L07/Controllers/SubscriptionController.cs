using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Dynamic;
using Microsoft.AspNetCore.Http;
using L07.Models;

namespace L07.Controllers
{
    public class SubscriptionController : Controller
    {
        #region "Subscriptions" 
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Subscriptions()
        {
            string select =
              @"SELECT *
                FROM Subscription, Subscriber, Provider
                WHERE Subscription.subscriber_id = Subscriber.subscriber_id
                AND Subscription.provider_id = Provider.provider_id
                ORDER BY subscription_id";

            DataTable dt = DBUtl.GetTable(select);
            return View(dt.Rows);
        }
        #endregion

        #region Subscription Add
        private void PopulateViewData()
        {
            DataTable dt1 = DBUtl.GetTable("SELECT * FROM Provider");
            ViewData["Providers"] = dt1.Rows;

            DataTable dt2 = DBUtl.GetTable("SELECT * FROM Subscriber");
            ViewData["Subscribers"] = dt2.Rows;
        }

        public IActionResult SubscriptionAdd()
        {
            PopulateViewData();
            return View();
        }

        public IActionResult SubscriptionAddPost()
        {
            IFormCollection form = HttpContext.Request.Form;

            string sid = form["subscriber"].ToString().Trim();
            string pid = form["provider"].ToString().Trim();
            string subDate = form["datesub"].ToString().Trim();

            // Validate User Enters and Selects all Fields
            if (ValidUtl.CheckIfEmpty(sid, pid))
            {
                PopulateViewData();
                ViewData["Message"] = "Please enter all fields";
                ViewData["MsgType"] = "warning";
                return View("SubscriptionAdd");
            }

            // Validate Subscription date values
            if (!subDate.IsDate("yyyy-MM-dd"))
            {
                PopulateViewData();
                ViewData["Message"] = "Please enter a valid date (YYYY-MM-DD)";
                ViewData["MsgType"] = "warning";
                return View("SubscriberAdd");
            }

            // Add Record to Database  
            string sql = @"INSERT INTO Subscription
                          (subscriber_id, provider_id, date_subscribed)
                         VALUES('{0}','{1}','{2}')";

            string insert = String.Format(sql, sid, pid, subDate);

            int count = DBUtl.ExecSQL(insert);
            if (count == 1)
            {
                TempData["Message"] = "Subscription Successfully Added.";
                TempData["MsgType"] = "success";
                return RedirectToAction("Subscriptions");
            }
            else
            {
                PopulateViewData();
                ViewData["Message"] = DBUtl.DB_Message;
                ViewData["MsgType"] = "danger";
                return View("SubscriptionAdd");
            }
        }
        #endregion

        #region "Subscription Delete"
        // TODO: L07 Task 6 - Complete the SubscriptionDelete(string id) action
        public IActionResult SubscriptionDelete(string id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }

            string sql = @"SELECT * FROM Subscription WHERE subscription_id = '{0}'";
            string select = String.Format(sql, id);
            DataTable dt = DBUtl.GetTable(select);
            if (dt.Rows.Count == 0)
            {
                TempData["Message"] = "Subscription Not Found!";
                TempData["MsgType"] = "warning";
                return View("Subscriptions");
            }

            sql = @"DELETE Subscription WHERE subscription_id = {0}";
            string delete = String.Format(sql, id);

            int count = DBUtl.ExecSQL(delete);
            if (count == 1)
            {
                TempData["Message"] = "Subscription Deleted!";
                TempData["MsgType"] = "success";
            }
            else
            {
                TempData["Message"] = DBUtl.DB_Message;
                TempData["MsgType"] = "danger";
            }
            return RedirectToAction("Subscriptions");
        }

        #endregion

        #region "Subscription Edit"
        public IActionResult SubscriptionEdit(String id)
        {
            // TODO: L07 Task 7 - Complete the SubscriptionEdit(string id) action
            string select = $"SELECT * FROM Subscription WHERE subscription_id={id}";
            DataTable dt = DBUtl.GetTable(select);
            if (dt.Rows.Count == 0)
            {
                TempData["Message"] = "Subscription Not Found";
                TempData["MsgType"] = "warning";
                return RedirectToAction("Subscriptions");
            }
            else
            {
                PopulateViewData();
                Subscription model = new Subscription
                {
                    SubscriptionId = (int)dt.Rows[0]["subscription_id"],
                    SubscriberId = (int)dt.Rows[0]["subscriber_id"],
                    ProviderId = (int)dt.Rows[0]["provider_id"],
                    DateSubscribed = (DateTime)dt.Rows[0]["date_subscribed"],
                };
                return View(model);
            }
        }

        // TODO: L07 Task 8 - Create the SubscriptionEditPost() action
        public IActionResult SubscriptionEditPost()
        {
            IFormCollection form = HttpContext.Request.Form;
            string subscriptionId = form["subscriptionId"].ToString().Trim();
            string subscriberId = form["subscriberId"].ToString().Trim();
            string providerId = form["providerId"].ToString().Trim();
            string dateSubscribed = form["dateSubscribed"].ToString().Trim();

            // Validate User Enters and Selects all Fields
            if (ValidUtl.CheckIfEmpty(subscriptionId, subscriberId, providerId, dateSubscribed))
            {
                ViewData["Message"] = "Please enter all fields";
                ViewData["MsgType"] = "warning";
                return View("SubscriptionAdd");
            }

            // Validate Date of Birth
            if (!dateSubscribed.IsDate("yyyy-MM-dd"))
            {
                ViewData["Message"] = "Please enter a valid date YYYY-MM-DD";
                ViewData["MsgType"] = "warning";
                return View("SubscriptionAdd");
            }

            // Update Record in Database  
            string sql = @"UPDATE Subscription
                           SET subscriber_id = '{1}',
                               provider_id = '{2}', 
                               date_subscribed = '{3}' 
                           WHERE subscription_id = '{0}'";

            string update = String.Format(sql, subscriptionId, subscriberId, providerId, dateSubscribed);


            int count = DBUtl.ExecSQL(update);
            if (count == 1)
            {
                TempData["Message"] = "Subscription Updated";
                TempData["MsgType"] = "success";
            }
            else
            {
                TempData["Message"] = DBUtl.DB_Message;
                TempData["MsgType"] = "danger";
            }
            return RedirectToAction("Subscriptions");
        }
        #endregion
    }
}
// 19031264 Liu Xian Min 