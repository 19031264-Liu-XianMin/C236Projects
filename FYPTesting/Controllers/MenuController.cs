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
using OfficeOpenXml;
using System.IO;

namespace FYPTesting.Controllers
{
    public class MenuController : Controller
    {
        [HttpPost]
        public ActionResult UploadFile(IFormFile file)
        {
            if (file.Length <= 0)
                return RedirectToAction("ListMenu");
            List<Menu> menuList = new List<Menu>();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var memoryStream = new MemoryStream())
            {
                file.OpenReadStream().CopyTo(memoryStream);
                using (var package = new ExcelPackage(memoryStream))
                {
                    // get the first worksheet in the workbook
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                    int col = 1;

                    for (int row = 2; worksheet.Cells[row, col].Value != null; row++, col = 1)
                    {
                        // do something with worksheet.Cells[row, col].Value
                        Menu menu = new Menu();
                        menu.Number = Convert.ToInt32(worksheet.Cells[row, col].Value);
                        col++;
                        menu.OrderDate = Convert.ToDateTime(worksheet.Cells[row, col].Value);
                        col++;
                        menu.DueDate = Convert.ToDateTime(worksheet.Cells[row, col].Value);
                        col++;
                        menu.RevisedDelDate = Convert.ToDateTime(worksheet.Cells[row, col].Value);
                        col++;
                        menu.PONo = Convert.ToString(worksheet.Cells[row, col].Value);
                        col++;
                        menu.PRNo = Convert.ToString(worksheet.Cells[row, col].Value);
                        col++;
                        menu.SupplierName = Convert.ToString(worksheet.Cells[row, col].Value);
                        col++;
                        menu.PaymentTerms = Convert.ToString(worksheet.Cells[row, col].Value);
                        col++;
                        menu.PartNo = Convert.ToString(worksheet.Cells[row, col].Value);
                        col++;
                        menu.Description = Convert.ToString(worksheet.Cells[row, col].Value);
                        col++;
                        menu.JobNum = Convert.ToString(worksheet.Cells[row, col].Value);
                        col++;
                        menu.Currency = Convert.ToString(worksheet.Cells[row, col].Value);
                        col++;
                        menu.UOM = Convert.ToDouble(worksheet.Cells[row, col].Value);
                        col++;
                        menu.Quantity = Convert.ToDouble(worksheet.Cells[row, col].Value);
                        col++;
                        menu.UnitPrice = Convert.ToDouble(worksheet.Cells[row, col].Value);
                        col++;
                        menu.OrigAmt = Convert.ToDouble(worksheet.Cells[row, col].Value);
                        col++;
                        menu.Request = Convert.ToString(worksheet.Cells[row, col].Value);
                        col++;
                        menu.Purchaser = Convert.ToString(worksheet.Cells[row, col].Value);
                        col++;
                        menuList.Add(menu);
                    }
                } // the using 
            }
            foreach (var mn in menuList)
            {
                string insert = @"INSERT INTO Menu(OrderDate, DueDate, PONo, PRNo, PartNo, Currency, Quantity, OrigAmt, Description, SupplierName, PaymentTerms, JobNum, Purchaser, Request, TSHPONO, RevisedDelDate, UOM,UnitPrice) VALUES
                 ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', {6}, {7}, '{8}','{9}','{10}','{11}','{12}', '{13}', '{14}','{15}', '{16}','{17}')";
                if (DBUtl.ExecSQL(insert, mn.OrderDate.Date, mn.DueDate.Date, mn.PONo, mn.PRNo, mn.PartNo, mn.Currency, mn.Quantity, mn.OrigAmt, mn.Description, mn.SupplierName, mn.PaymentTerms, mn.JobNum, mn.Purchaser, mn.Request, mn.TSHPONO, mn.RevisedDelDate, mn.UOM, mn.UnitPrice) == 1)
                {
                    TempData["Message"] = "Purchase Order Inserted";
                    TempData["MsgType"] = "success";
                }
                else
                {
                    TempData["Message"] = DBUtl.DB_Message;
                    TempData["MsgType"] = "danger";
                }
                //return RedirectToAction("ListMenu");
            }
            return RedirectToAction("PurchaserView");
        }
        public ActionResult Login()
        {
            return View();
        }
        public ActionResult POTable()
        {
            return View();
        }
        [Authorize(Roles = "admin")]
        public ActionResult AdminView()
        {
            List<Menu> list = DBUtl.GetList<Menu>("SELECT * FROM Menu");
            return View(list);
        }
        [Authorize(Roles = "supplier")]
        public ActionResult SupplierView()
        {
            List<Menu> list = DBUtl.GetList<Menu>("SELECT * FROM Menu");
            return View(list);
        }
        [Authorize(Roles = "purchaser")]
        public ActionResult PurchaserView()
        {
            List<Menu> list = DBUtl.GetList<Menu>("SELECT * FROM Menu");
            return View(list);
        }
        //[Authorize(Roles = "supplier")]
        /*public IActionResult SupplierMenu()
        {
            List<Menu> supplierList = DBUtl.GetList<Menu>("SELECT * FROM Menu WHERE SupplierName LIKE '%GLOBAL TECHNOLOGY INTEGRATOR PTE LTD%'");
            return View(supplierList);
        }

        [Authorize(Roles = "admin")]
        public IActionResult ListMenu()
        {
            List<Menu> list = DBUtl.GetList<Menu>("SELECT * FROM Menu");
            return View(list);
        }

        //[Authorize(Roles = "purchaser")]
        public IActionResult PurchaserMenu()
        {
            List<Menu> Purchaserlist = DBUtl.GetList<Menu>("SELECT * FROM Menu");
            return View(Purchaserlist);
        }*/

        [Authorize(Roles = "salesman")]
        public IActionResult SalesManMenu()
        {
            List<Menu> list = DBUtl.GetList<Menu>("SELECT * FROM Menu");
            return View(list);
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
                return RedirectToAction("AdminView");
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
