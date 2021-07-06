using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FYPTesting.Models;
using System;
using System.Collections.Generic;
using Microsoft.Azure.Documents;
using System.Web.Providers.Entities;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Security.Claims;

namespace FYPTesting.Controllers
{
    public class MenuController : Controller
    {
        [Authorize(Roles = "supplier")]
        public IActionResult SupplierMenu()
        {
            List<Menu> supplierList = DBUtl.GetList<Menu>("SELECT * FROM Menu WHERE SupplierName LIKE '%GLOBAL TECHNOLOGY INTEGRATOR PTE LTD%'");
            return View(supplierList);
        }

        [Authorize(Roles ="admin, supplier, salesman, purchaser")]
        public IActionResult ListMenu()
        {
            List<Menu> list = DBUtl.GetList<Menu>("SELECT * FROM Menu");
            return View(list);
        }


        [Authorize(Roles = "admin, purchaser")]
        public IActionResult AddPO()
        {
            return View("AddPO");
        }

        [Authorize(Roles = "admin, purchaser")]
        [HttpPost]
        public IActionResult AddPO(Menu mn)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Message"] = "Invalid Input";
                ViewData["MsgType"] = "warning";
                return View("AddPO");
            }
            else
            {
                string insert = @"INSERT INTO Menu(OrderDate, DueDate, PONo, PRNo, PartNo, Currency, Quantity, OrigAmt, Description) VALUES
                 ('{0:YYYY-MM-DD}', '{1:YYYY-MM-DD}', '{2}', '{3}', '{4}', '{5}', {6}, {7}, '{8}')";
                if (DBUtl.ExecSQL(insert, mn.OrderDate, mn.DueDate, mn.RevisedDelDate, mn.PONo, mn.PRNo, mn.Currency, mn.Quantity, mn.OrigAmt, mn.Description) == 1)
                {
                    TempData["Message"] = "Purchase Order Updated";
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

    }
}
