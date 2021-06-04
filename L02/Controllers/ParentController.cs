using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace L02.Controllers
{
    public class ParentController : Controller
    {

        private int CalcCreditPoint(string activity, int days)
        {
            int credits = 0;
            if (activity.Equals("Story Telling") ||
                activity.Equals("Art and Craft"))
            {
                credits = days * 10;
            }
            else if (activity.Equals("Traffic Control"))
            {
                credits = days * 5;
            }
            else if (activity.Equals("Music Appreciation"))
            {
                credits = days * 15;
            }
            return credits;
        }
        public IActionResult Volunteer()
        {
            return View();
        }
        public IActionResult Submit()
        {

            // Use IFormCollection for shorter coding
            IFormCollection form = HttpContext.Request.Form;

            // Read the TextFields and Dropdown List
            // string name = form["Name"].ToString().Trim();
            // ....
            
            String name = form["Name"].ToString().Trim();
            String postal = form["Postal"].ToString().Trim();
            String mobile = form["Mobile"].ToString().Trim();
            String activity = form["Activity"].ToString().Trim();
            // Read the RadioButtons 
            // string title = form["Title"].ToString();
            String title = form["Title"].ToString();
            


            // Read the CheckBoxes 
            // string mon = ......
            // ....
            String mon = form["Mon"].ToString();
            String wed = form["Wed"].ToString();
            String fri= form["Fri"].ToString();


            // Determine Number of Days Checked
            int days = 0;
            string daysSelected = "";
            if (mon.Equals("Mon"))
            {
                days++;
                daysSelected += "Monday, ";
            }
            if (wed.Equals("Wed"))
            {
                days++;
                daysSelected += "Wednesday, ";
            }
            if (fri.Equals("Fri"))
            {
                days++;
                daysSelected += "Friday, ";
            }
            
            if (days == 0)
            {
                ViewData["Message"] = "Please check days";
                return View("Volunteer");
            }


            // Passing Data to the View
            //ViewData["________"] = title + " " + name;
            //ViewData["________"] = activity;
            //ViewData["________"] = daysSelected;
            //ViewData["________"] = CalcCreditPoint(activity, days);
            ViewData["title"] = title + " " + name;
            ViewData["activity"] = activity;
            ViewData["days"] = daysSelected;
            ViewData["points"] = CalcCreditPoint(activity, days);

            return View(); // Submit.cshtml
        }

    }
}
// 19031264 Liu Xian Min