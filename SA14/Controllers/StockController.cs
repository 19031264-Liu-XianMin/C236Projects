using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SA14.Models;

namespace SA14.Controllers
{
    public class StockController : Controller
    {
        [AllowAnonymous]
        public IActionResult Index()
        {
            List<dynamic> list = DBUtl.GetList("SELECT * FROM Stock INNER JOIN Category ON Category.CatID = Stock.CatID");
            return View(list);
        }
        
        public IActionResult Update(int id)
        {
            string select = "SELECT * FROM Stock WHERE StockNo='{0}'";
            List<Stock> list = DBUtl.GetList<Stock>(select);
            if (list.Count == 1)
            {
                var cList = DBUtl.GetList("SELECT CatID, CatTitile FROM Category");
                ViewData["cList"] = new SelectList(cList, "CatID", "CatTitle");
                Stock stock = list[0];
                return View(stock);
            }
            else
            {
                TempData["Message"] = "Stock Record does not exist";
                TempData["MsgType"] = "warning";
                return RedirectToAction("Index");
            }
        }
        [HttpPost]
        public IActionResult Update(Stock stock)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Message"] = "Invalid Input";
                ViewData["MsgType"] = "warning";
                return RedirectToAction("Index");
            }
            else
            {
                string update =
                   @"UPDATE Stock
                    SET ItemDesc='{1}', CatID={2}, CostPrice={3}, SellPrice={4}, QtyAvailable={5}, ReOrderQty={6}, Launched='{7:yyyy-MM-dd}'
                  WHERE StockNo={0}";
               
                if (DBUtl.ExecSQL(update, stock.StockNo, stock.ItemDesc, stock.CatID, stock.CostPrice, stock.SellPrice, stock.QtyAvailable, stock.ReOrderQty, stock.Launched) == 1)
                {
                    TempData["Message"] = "Stock Record Updated";
                    TempData["MsgType"] = "success";
                }
                else
                {
                    TempData["Message"] = DBUtl.DB_Message;
                    TempData["MsgType"] = "danger";
                }
                return RedirectToAction("Index");
            }
        }
    }
}
