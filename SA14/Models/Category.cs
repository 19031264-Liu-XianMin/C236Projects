using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace SA14.Models
{
    public class Category 
    {
        public int CatID { get; set; }
        public string CatTitle { get; set; }
        public int Ranking { get; set; }
    }
}
