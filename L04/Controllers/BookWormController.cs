using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace L04.Controllers
{
    public class BookWormController : Controller
    {

        const string SELECT1 = // QUESTION ONE
           @"SELECT * FROM BwBook WHERE Lang != 'English'";

        const string SELECT2 = // QUESTION TWO
           @"SELECT * FROM BwBook WHERE Qty = 0";

        const string SELECT3 = // QUESTION THREE
           @"SELECT P.PubName AS Publisher, COUNT(*) AS Title FROM BwPublisher P INNER JOIN BwBook B ON B.PubID = P.PubID GROUP BY P.PubName";

        const string SELECT4 = // QUESTION FOUR
           @"SELECT Title FROM BwBook GROUP BY Title HAVING COUNT(Lang) > 1 ";

        public IActionResult Query()
        {
            return View();
        }

        public IActionResult Submit()
        {
            IFormCollection form = HttpContext.Request.Form;
            string question = form["Question"].ToString();

            string sql = "";
            if (question.Equals("1"))
            {
                sql = SELECT1;
            }
            else if (question.Equals("2"))
            {
                sql = SELECT2;
            }
            else if (question.Equals("3"))
            {
                sql = SELECT3;
            }
            else if (question.Equals("4"))
            {
                sql = SELECT4;
            }

            DataTable dt = DBUtl.GetTable(sql);
            return View("Query", dt);
        }

    }

}
// 19031264 Liu Xian Min
