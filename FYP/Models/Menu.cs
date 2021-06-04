using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace FYP.Models
{
    public class Menu
    {
        [Required(ErrorMessage = "Please enter Status No")]
        public string Status { get; set; }

        [Required(ErrorMessage = "Please enter Date/Time")]
        [DataType(DataType.DateTime)]
        public string OrderDate { get; set; }

        [Required(ErrorMessage = "Please enter Date/Time")]
        [DataType(DataType.DateTime)]
        public string DueDate { get; set; }

        [Required(ErrorMessage = "Please enter Date/Time")]
        [DataType(DataType.DateTime)]
        public int ReviseDueDate { get; set; }

        [Required(ErrorMessage = "Please enter PO No")]
        [StringLength(11, MinimumLength = 1, ErrorMessage = "PO No 1-11 chars")]
        public int PONo { get; set; }

        [Required(ErrorMessage = "Please enter PR No")]
        [StringLength(11, MinimumLength = 1, ErrorMessage = "PR No 1-11 chars")]
        public int PRNo { get; set; }

        [Required(ErrorMessage = "Please enter Supplier Name")]
        [StringLength(200, MinimumLength = 1, ErrorMessage = "Supplier Name 1-200 chars")]
        public int SupplierName { get; set; }

        [Required(ErrorMessage = "Please enter Payment Terms")]
        [StringLength(10, MinimumLength = 1, ErrorMessage = "Payment Terms 1-10 chars")]
        public int PaymentTerms { get; set; }

        [Required(ErrorMessage = "Please enter Part No")]
        public int PartNo { get; set; }

        [Required(ErrorMessage = "Please enter Description")]
        public int Description { get; set; }

        [Required(ErrorMessage = "Please enter Currency")]
        public int Currency { get; set; }

        [Required(ErrorMessage = "Please enter Quantity No")]
        [Range(0.000, 9999999.000, ErrorMessage = "0-9999999")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Please enter Original Amount")]
        [Range(0.00, 9999999.00, ErrorMessage = "0-9999999 marks")]
        public int OrigAmt { get; set; }


    }
}
