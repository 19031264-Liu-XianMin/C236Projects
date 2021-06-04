using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace L08.Models
{
    public class Trip
    {
        // TODO L08 Task 3 - Specify [Required] for some properties

        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string City { get; set; }
        public DateTime TripDate { get; set; }
        public int Duration { get; set; }
        public double Spending { get; set; }
        [Required]
        public string Story { get; set; }
        [Required]
        public IFormFile Photo { get; set; }

        public string Picture { get; set; }

        public string SubmittedBy { get; set; }
    }
}
//19031264 Liu Xian Min
