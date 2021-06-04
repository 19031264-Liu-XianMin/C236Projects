using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace L02.Controllers
{
    public class DemoController : Controller
    {
        public IActionResult SayHello1()
        {
            DoGreeting();
            return View();
        }
        private void DoGreeting()
        {
            DateTime current = DateTime.Now;
            string greet = String.Format("It's {0:HHmm}hrs. ", current);
            if (current.Hour < 12)
                greet += "Good Morning";
            else if (current.Hour < 18)
                greet += "Good Afternoon";
            else
                greet += "Good Evening";
            ViewData["Greeting"] = greet;
        }
        public IActionResult SayHello5()
        {
            DoGreeting();
            return View(); // Views/Demo/SayHello5.cshtml
        }

        public IActionResult SayHello5_Post()
        {
            DoGreeting();
            string name = HttpContext.Request.Form["Name"];
            string salute = HttpContext.Request.Form["Gender"].ToString();
            string memship = HttpContext.Request.Form["Membership"];
            string smoke = HttpContext.Request.Form["Smoke"].ToString();

            ViewData["Message"] = $"Hello {salute} {name} ({memship}), Wecome!";
            if (String.Equals(smoke, "Smoking"))
                ViewData["Message"] += " Please smoke at the designated places.";

            return View("SayHello5"); // Views/Demo/SayHello5.cshtml
        }

    }
}
// 19031264 Liu Xian Min