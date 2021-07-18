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

        [Authorize(Roles ="admin")]
        public IActionResult ListMenu()
        {
            List<Menu> list = DBUtl.GetList<Menu>("SELECT * FROM Menu");
            return View(list);
        }

        [Authorize(Roles = "purchaser")]
        public IActionResult PurchaserMenu()
        {
            List<Menu> Purchaserlist = DBUtl.GetList<Menu>("SELECT * FROM Menu");
            return View(Purchaserlist);
        }

        [Authorize(Roles = "salesman")]
        public IActionResult SalesManMenu()
        {
            List<Menu> SalesManlist = DBUtl.GetList<Menu>("SELECT * FROM Menu");
            return View(SalesManlist);
        }

        [Authorize(Roles = "purchaser, admin")]
        public IActionResult EditPO(int id)
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
                return RedirectToAction("ListMenu");
            }
        }

        [Authorize(Roles = "purchaser, admin")]
        [HttpPost]
        public IActionResult EditPO(Menu mn)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Message"] = "Invalid Input";
                ViewData["MsgType"] = "warning";
                return View("EditPO");
            }
            else
            {
                string update =
                   @"UPDATE Menu
                    SET RevisedDelDate='{13:YYYY-MM-DD}'
                  WHERE Number={0}";
                int res = DBUtl.ExecSQL(update, mn.RevisedDelDate);
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
                return RedirectToAction("ListMenu");
            }
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
                string insert = @"INSERT INTO Menu(OrderDate, DueDate, Purchaser, Requestor, SupplierName, PONo, TSHPONO, PRNo, PartNo, JobNum, Currency, Quantity, PaymentTerms, UOM, UnitPrice, OrigAmt, Description) VALUES
                 ('{0:YYYY-MM-DD}', '{1:YYYY-MM-DD}', '{2}', '{3}', '{4}', '{5}', {6}, {7}, '{8}', '{9}', '{10}', '{11}', '{12}', '{13}', '{14}', '{15}', '{16}')";
                if (DBUtl.ExecSQL(insert, mn.OrderDate, mn.DueDate, mn.RevisedDelDate, mn.Purchaser,mn.Request,mn.SupplierName, mn.PONo, mn.TSHPONO, mn.PRNo, mn.PartNo, mn.JobNum, mn.Currency, mn.Quantity, mn.PaymentTerms, mn.UOM, mn.OrigAmt, mn.Description) == 1)
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
