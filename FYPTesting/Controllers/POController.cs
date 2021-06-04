using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FYPTesting.Models;
using System;
using System.Collections.Generic;

namespace FYPTesting.Controllers
{
    public class POController : Controller
    {
        [Authorize(Roles = "admin, supplier")]
        public IActionResult PurchaseOrder()
        {
            List<Menu> list = DBUtl.GetList<Menu>("SELECT * FROM PurchaseOrder");
            return View(list);
        }
    }
}
