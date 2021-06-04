using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace FYP.Models
{
    public class PurchaseOrder : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
