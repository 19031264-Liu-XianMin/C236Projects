﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace L10.Models
{
    public class Performance
    {
        // TODO: L10 Task 3 - Write Validation Attributes for all fields
        [Required(ErrorMessage = "Please enter Title")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Title must be 1-50 characters")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Please enter Artist")]
        [StringLength(40, MinimumLength = 1, ErrorMessage = "Artist must be 1-40 characters")]
        public string Artist { get; set; }

        [Required(ErrorMessage = "Please enter Date/Time")]
        [DataType(DataType.DateTime)]
        [Remote(action: "VerifyDate", controller: "Performance")]
        public DateTime PerformDT { get; set; }

        [Required(ErrorMessage = "Please enter Duration")]
        [Range(0.5, 4.0, ErrorMessage = "Duration 0.5-4.0 hours")]
        public float Duration { get; set; }

        [Required(ErrorMessage = "Please enter Price")]
        [Range(0.0, 1000.0, ErrorMessage = "Price 0-1000")]
        public float Price { get; set; }

        [Required(ErrorMessage = "Please enter Chamber")]
        [RegularExpression("C[1-3][0-9]", ErrorMessage = "Invalid Chamber")]
        public string Chamber { get; set; }
    }
}
// 19031264 Liu Xian Min