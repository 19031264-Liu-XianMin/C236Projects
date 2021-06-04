using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace SA14.Models
{
    public class Stock
    {

        public int StockNo { get; set; }

        [Required(ErrorMessage = "Please enter item description")]
        public String ItemDesc { get; set; }

        public int CatID { get; set; }

        [Required(ErrorMessage = "Please enter cost price")]
        [Range(0, 850.88, ErrorMessage = "Price 0 - 850.88")]
        public double CostPrice { get; set; }

        [Required(ErrorMessage = "Please enter sell price")]
        [Range(0, 1299.99, ErrorMessage = "Price 0 - 1299.99")]
        public double SellPrice { get; set; }

        [Required(ErrorMessage = "Please enter quality available")]
        [StringLength(500, MinimumLength = 1, ErrorMessage = "Quality Available 1-500 chars")]
        public int QtyAvailable { get; set; }

        [Required(ErrorMessage = "Please enter reorder quality")]
        [StringLength(102, MinimumLength = 1, ErrorMessage = "Reorder Quality 1-50 chars")]
        public int ReOrderQty { get; set; }

        public DateTime Launched { get; set; }

        [RegularExpression([AA|BB|CC] [^\d*[13579]$][A-Z^3])
        public string PCode { get; set; }
    }
}
