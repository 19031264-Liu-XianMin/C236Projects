﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using L07.Models;
using System.Data;

namespace L07.Controllers
{
    public class SubscriberController : Controller
    {
        #region "Subscribers"
        public IActionResult Index()
        {
            return View("Index");
        }

        public IActionResult Subscribers()
        {
            string select = "SELECT * FROM Subscriber";
            DataTable dt = DBUtl.GetTable(select);
            return View(dt.Rows);
        }
        #endregion

        #region "Subscriber Add"
        // TODO: L07 Task 3 - Create the SubscriberAdd() action
        public IActionResult SubscriberAdd()
        {
            return View();
        }

        // TODO: L07 Task 4 - Create the SubscriberAddPost() action
        public IActionResult SubscriberAddPost()
        {
            // Retrieve the text data from the form
            IFormCollection form = HttpContext.Request.Form;

            // Validate the user has entered all text fields
            string pp = "0";
            string aaf = "0";
            string bp = "0";

            string username = form["uname"].ToString().Trim();
            string firstName = form["name1"].ToString().Trim();
            string familyName = form["name2"].ToString().Trim();
            string dob = form["datebirth"].ToString().Trim();

            pp = form["publicprofile"].ToString().Trim();
            aaf = form["autofriends"].ToString().Trim();
            bp = form["broadcast"].ToString().Trim();

            // Write the SQL to insert the record into the database
            if (ValidUtl.CheckIfEmpty(username, firstName, familyName, dob))
            {
                ViewData["Message"] = "Please enter all fields";
                ViewData["MsgType"] = "warning";
                return View("SubscriberAdd");
            }

            // Execute the SQL

            // Check if the SQL was successful. 
            //     If successful redirect to the 'Providers' action
            //     If failure show the DB_Message and return to the 'ProviderAdd' view

            string sql = "INSERT INTO Subscriber(username, first_name, family_name, dob, public_profile_flag, auto_accept_friends_flag, broadcast_posts_flag) VALUES('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}')";
            string insert = String.Format(sql, username.EscQuote(), firstName.EscQuote(), familyName.EscQuote(), dob, pp, aaf, bp);

            int count = DBUtl.ExecSQL(insert);
            if (count == 1)
            {
                TempData["Message"] = "Subscriber Successfully Added!";
                TempData["MsgType"] = "success";
                return RedirectToAction("Subscribers");
            }
            else
            {
                ViewData["Message"] = DBUtl.DB_Message;
                ViewData["MsgType"] = "danger";
                return View("SubscriberAdd");
            }
        }
        #endregion

        #region "Subscriber Delete"
        public IActionResult SubscriberDelete(string id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }

            string sql = @"SELECT * FROM Subscriber WHERE subscriber_id={0}";
            string select = String.Format(sql, id);
            DataTable dt = DBUtl.GetTable(select);
            if (dt.Rows.Count == 0)
            {
                TempData["Message"] = "Subscriber Not Found";
                TempData["MsgType"] = "warning";
                return RedirectToAction("Subscribers");
            }

            // Check the selected subscriber doesn't have any subscriptions
            sql = @"SELECT * FROM Subscription WHERE subscriber_id={0}";
            select = String.Format(sql, id);
            DataTable dt2 = DBUtl.GetTable(select);
            if (dt2.Rows.Count > 0)
            {
                TempData["Message"] = "Subscriber cannot be deleted. " +
                                      "All of its subscriptions must be deleted first.";
                TempData["MsgType"] = "warning";
                return RedirectToAction("Subscribers");
            }

            sql = @"DELETE Subscriber WHERE subscriber_id={0}";
            string delete = String.Format(sql, id);

            int count = DBUtl.ExecSQL(delete);
            if (count == 1)
            {
                TempData["Message"] = "Subscriber Deleted";
                TempData["MsgType"] = "success";
            }
            else
            {
                TempData["Message"] = DBUtl.DB_Message;
                TempData["MsgType"] = "danger";
            }
            return RedirectToAction("Subscribers");
        }
        #endregion

        #region "Subscriber Edit"
        public IActionResult SubscriberEdit(String id)
        {
            string sql = "SELECT * FROM Subscriber WHERE subscriber_id={0}";
            string select = String.Format(sql, id);
            DataTable dt = DBUtl.GetTable(select);
            if (dt.Rows.Count == 1)
            {
                Subscriber sub = new Subscriber
                {
                    SubscriberId = (int)dt.Rows[0]["subscriber_id"],
                    UserName = dt.Rows[0]["username"].ToString(),
                    FirstName = dt.Rows[0]["first_name"].ToString(),
                    FamilyName = dt.Rows[0]["family_name"].ToString(),
                    DateOfBirth = (DateTime)dt.Rows[0]["dob"],
                    PublicProfile = (bool)dt.Rows[0]["public_profile_flag"],
                    AutoAcceptFriends = (bool)dt.Rows[0]["auto_accept_friends_flag"],
                    BroadcastPosts = (bool)dt.Rows[0]["broadcast_posts_flag"]
                };
                return View(sub);
            }
            else
            {
                TempData["Message"] = "Subscriber Not Found";
                TempData["MsgType"] = "warning";
                return RedirectToAction("Subscribers");
            }
        }

        public IActionResult SubscriberEditPost()
        {
            IFormCollection form = HttpContext.Request.Form;
            string subId = form["subid"].ToString().Trim();
            string userName = form["uname"].ToString().Trim();
            string firstName = form["name1"].ToString().Trim();
            string familyName = form["name2"].ToString().Trim();
            string dateOfBirth = form["datebirth"].ToString().Trim();

            // Validate User Enters and Selects all Fields
            if (ValidUtl.CheckIfEmpty(userName, firstName, familyName, dateOfBirth))
            {
                ViewData["Message"] = "Please enter all fields";
                ViewData["MsgType"] = "warning";
                return View("SubscriberAdd");
            }

            // Validate Date of Birth
            if (!dateOfBirth.IsDate("yyyy-MM-dd"))
            {
                ViewData["Message"] = "Please enter a valid date YYYY-MM-DD";
                ViewData["MsgType"] = "warning";
                return View("SubscriberAdd");
            }

            // Read CheckBoxes  
            string publicProfile = form["publicprofile"].ToString();
            string autoAcceptFriends = form["autofriends"].ToString();
            string broadcastPosts = form["broadcast"].ToString();

            // Update Record in Database  
            string sql = @"UPDATE Subscriber
                           SET username = '{1}',
                               first_name = '{2}', 
                               family_name = '{3}', 
                               dob = '{4}', 
                               public_profile_flag = '{5}', 
                               auto_accept_friends_flag = '{6}',
                               broadcast_posts_flag = '{7}'
                           WHERE subscriber_id = '{0}'";

            string update = String.Format(sql, subId,
                                               userName,
                                               firstName,
                                               familyName,
                                               dateOfBirth,
                                               (publicProfile.Equals("Yes") ? 1 : 0),
                                               (autoAcceptFriends.Equals("Yes") ? 1 : 0),
                                               (broadcastPosts.Equals("Yes") ? 1 : 0));

            int count = DBUtl.ExecSQL(update);
            if (count == 1)
            {
                TempData["Message"] = "Subscriber Updated";
                TempData["MsgType"] = "success";
            }
            else
            {
                TempData["Message"] = DBUtl.DB_Message;
                TempData["MsgType"] = "danger";
            }
            return RedirectToAction("Subscribers");
        }
        #endregion
    }
}
// 19031264 Liu Xian Min 