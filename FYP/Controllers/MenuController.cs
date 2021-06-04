using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FYP.Models;
using System;
using System.Collections.Generic;

namespace FYP.Controllers
{
    public class MenuController : Controller
    {
        public IActionResult ListMenu()
        {
            List<Menu> list = DBUtl.GetList<Menu>("SELECT * FROM Menu");
            return View(list);
        }

        [HttpGet]
        public IActionResult EditMenu(string id)
        {
            string select = "SELECT * FROM Menu WHERE Status='{0}'";
            List<Menu> list = DBUtl.GetList<Menu>(select, id);
            if (list.Count == 1)
            {
                return View(list[0]);
            }
            else
            {
                TempData["Message"] = "Status not found";
                TempData["MsgType"] = "warning";
                return RedirectToAction("ListMenu");
            }
        }

        [HttpPost]
        public IActionResult EditMenu(Menu cdt)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Message"] = "Invalid Input";
                ViewData["MsgType"] = "warning";
                return View("EditMenu");
            }
            else
            {
                string update =
                   @"UPDATE Menu
                    SET 
                        ReviseDueDate='{3}',  Quantity={11}, 
                  WHERE Status='{0}'";
                int res = DBUtl.ExecSQL(update, cdt.ReviseDueDate,cdt.Quantity);
                if (res == 1)
                {
                    TempData["Message"] = "Menu Updated";
                    TempData["MsgType"] = "success";
                }
                else
                {
                    TempData["Message"] = DBUtl.DB_Message;
                    TempData["MsgType"] = "danger";
                }
                return RedirectToAction("ListMenu");
            }
        }

        public IActionResult Employees()
        {
            return View();
        }
    }
}
