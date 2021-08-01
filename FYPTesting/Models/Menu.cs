using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace FYPTesting.Models
{
    public class Menu
    {

        public int Number { get; set; }

        [Required(ErrorMessage = "Please enter Supplier Name")]
        [StringLength(200, MinimumLength = 1, ErrorMessage = "Supplier Name 1-200 chars")]
        public string SupplierName { get; set; }

        [Required(ErrorMessage = "Please enter PO No")]
        [StringLength(11, MinimumLength = 1, ErrorMessage = "PO No 1-11 chars")]
        public String PONo { get; set; }

        [Required(ErrorMessage = "Please enter Date/Time")]
        [DataType(DataType.DateTime)]
        public DateTime OrderDate { get; set; }

        [Required(ErrorMessage = "Please enter Payment Terms")]
        [StringLength(10, MinimumLength = 1, ErrorMessage = "Payment Terms 1-10 chars")]
        public string PaymentTerms { get; set; }

        [Required(ErrorMessage = "Please enter Job Number")]
        [StringLength(11, MinimumLength = 1, ErrorMessage = "Job Number 1-11 chars")]
        public string JobNum { get; set; }

        [Required(ErrorMessage = "Please enter Purchaser Name")]
        [StringLength(200, MinimumLength = 1, ErrorMessage = "Supplier Name 1-200 chars")]
        public string Purchaser { get; set; }

        [Required(ErrorMessage = "Please enter Request Name")]
        [StringLength(200, MinimumLength = 1, ErrorMessage = "Supplier Name 1-200 chars")]
        public string Request { get; set; }

        [Required(ErrorMessage = "Please enter PR No")]
        [StringLength(11, MinimumLength = 1, ErrorMessage = "PR No 1-11 chars")]
        public String PRNo { get; set; }

        [Required(ErrorMessage = "Please enter TSH PO Number")]
        [StringLength(11, MinimumLength = 1, ErrorMessage = "TSH PO Number 1-11 chars")]
        public string TSHPONO { get; set; }

        [Required(ErrorMessage = "Please enter Part No")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = "Part No 1-50 chars")]
        public String PartNo { get; set; }

        [Required(ErrorMessage = "Please enter Description")]
        public String Description { get; set; }

        [Required(ErrorMessage = "Please enter Date/Time")]
        [DataType(DataType.DateTime)]
        public DateTime DueDate { get; set; }

        [Required(ErrorMessage = "Please enter Date/Time")]
        [DataType(DataType.DateTime)]
        public DateTime RevisedDelDate { get; set; }

        [Required(ErrorMessage = "Please enter Currency")]
        [StringLength(3, MinimumLength = 1, ErrorMessage = "Currency 1-3 chars")]
        public string Currency { get; set; }

        [Required(ErrorMessage = "Please enter Quantity No")]
        [Range(0.000, 9999999.000, ErrorMessage = "0-9999999")]
        public double Quantity { get; set; }

        [Required(ErrorMessage = "Please enter Units of Measure")]
        [Range(0.000, 9999999.000, ErrorMessage = "0-9999999")]
        public double UOM { get; set; }

        [Required(ErrorMessage = "Please enter Unit Price")]
        [Range(0.000, 9999999.000, ErrorMessage = "0-9999999")]
        public double UnitPrice { get; set; }

        [Required(ErrorMessage = "Please enter Original Amount")]
        [Range(0.00, 9999999.00, ErrorMessage = "0-9999999 marks")]
        public double OrigAmt { get; set; }

        [Required(ErrorMessage = "Please enter Status")]
        [StringLength(8, MinimumLength = 1, ErrorMessage = "Status 1-8 chars")]
        public String Status { get; set; }

    }
}
