using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace FYPTesting.Models
{
    public class Edit
    {
        public int Number { get; set; }

        [Required(ErrorMessage = "Please enter Date/Time")]
        [DataType(DataType.DateTime)]
        public DateTime RevisedDelDate { get; set; }

    }
}
