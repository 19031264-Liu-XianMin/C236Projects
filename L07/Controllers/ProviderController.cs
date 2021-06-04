using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using L07.Models;
using System.Data;

namespace L07.Controllers
{
    public class ProviderController : Controller
    {
        #region "Providers" 
        public IActionResult Index()
        {
            return View("Index");
        }

        public IActionResult Providers()
        {
            string select = "SELECT * FROM Provider";
            DataTable dt = DBUtl.GetTable(select);
            return View(dt.Rows);
        }
        #endregion

        #region "Provider Add" 
        // TODO: L07 Task 1 - Complete the ProviderAdd() action
        public IActionResult ProviderAdd()
        {
            return View();
        }

        // TODO: L07 Task 2 - Complete the ProviderAddPost() action.
        public IActionResult ProviderAddPost()
        {
            // Retrieve the text data from the form
            IFormCollection form = HttpContext.Request.Form;

            // Validate the user has entered all text fields
            string providerName = form["providername"].ToString().Trim();

            // Write the SQL to insert the record into the database
            if (ValidUtl.CheckIfEmpty(providerName))
            {
                ViewData["Message"] = "Please enter all fields";
                ViewData["MsgType"] = "warning";
                return View("ProviderAdd");
            }
            // Execute the SQL

            // Check if the SQL was successful. 
            //     If successful redirect to the 'Providers' action
            //     If failure show the DB_Message and return to the 'ProviderAdd' view
            string sql = "INSERT INTO Provider(name) VALUES('{0}')";
            string insert = String.Format(sql, providerName.EscQuote());

            int count = DBUtl.ExecSQL(insert);
            if(count == 1)
            {
                TempData["Message"] = "Provider Successfully Added!";
                TempData["MsgType"] = "success";
                return RedirectToAction("Providers");
            }
            else
            {
                ViewData["Message"] = DBUtl.DB_Message;
                ViewData["MsgType"] = "danger";
                return View("ProviderAdd");
            }
        }
        #endregion

        #region "Provider Delete"        
        public IActionResult ProviderDelete(string id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }

            string sql = "SELECT * FROM Provider WHERE provider_id={0}";
            string select = String.Format(sql, id);
            DataTable dt = DBUtl.GetTable(select);
            if (dt.Rows.Count == 0)
            {
                TempData["Message"] = "Provider Not Found";
                TempData["MsgType"] = "warning";
                return RedirectToAction("Providers");
            }

            // Check the selected provider doesn't have any subscriptions
            sql = "SELECT * FROM Subscription WHERE provider_id={0}";
            select = String.Format(sql, id);
            DataTable dt2 = DBUtl.GetTable(select);
            if (dt2.Rows.Count > 0)
            {
                TempData["Message"] = "Provider cannot be deleleted. " +
                                      "All of the providers's subscriptions must be deleted first";
                TempData["MsgType"] = "warning";
                return RedirectToAction("Providers");
            }

            sql = @"DELETE Provider WHERE provider_id ={0}";
            string delete = String.Format(sql, id);

            int count = DBUtl.ExecSQL(delete);
            if (count == 1)
            {
                TempData["Message"] = "Provider Deleted";
                TempData["MsgType"] = "success";
            }
            else
            {
                TempData["Message"] = DBUtl.DB_Message;
                TempData["MsgType"] = "danger";
            }
            return RedirectToAction("Providers");
        }
        #endregion

        #region "Provider Edit"
        public IActionResult ProviderEdit(String id)
        {
            string sql = "SELECT * FROM Provider WHERE provider_id={0}";
            string select = String.Format(sql, id);
            DataTable dt = DBUtl.GetTable(select);
            if (dt.Rows.Count == 1)
            {
                Provider prov = new Provider
                {
                    ProviderId = (int)dt.Rows[0]["provider_id"],
                    Name = dt.Rows[0]["name"].ToString(),
                };
                return View(prov);
            }
            else
            {
                TempData["Message"] = "Provider Not Found";
                TempData["MsgType"] = "warning";
                return RedirectToAction("Providers");
            }
        }

        public IActionResult ProviderEditPost()
        {
            IFormCollection form = HttpContext.Request.Form;
            string provId = form["provId"].ToString().Trim();
            string provName = form["provName"].ToString().Trim();

            // Validate User Enters and Selects all Fields
            if (ValidUtl.CheckIfEmpty(provName))
            {
                ViewData["Message"] = "Please enter all fields";
                ViewData["MsgType"] = "warning";
                return View("ProviderEdit");
            }

            // Update Record in Database  
            string sql = @"UPDATE Provider
                           SET name = '{1}'
                           WHERE Provider_id={0}";

            string update = String.Format(sql, provId,
                                               provName);

            int count = DBUtl.ExecSQL(update);
            if (count == 1)
            {
                TempData["Message"] = "Provider Updated";
                TempData["MsgType"] = "success";
            }
            else
            {
                TempData["Message"] = DBUtl.DB_Message;
                TempData["MsgType"] = "danger";
            }
            return RedirectToAction("Providers");
        }
        #endregion
    }
}
// 19031264 Liu Xian Min 