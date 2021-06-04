using System.ComponentModel.DataAnnotations;

namespace FYPTesting.Models
{
    public class MemberLogin
    {
        [Required(ErrorMessage = "Please enter User ID")]
        public string UserID { get; set; }

        //[Required(ErrorMessage = "Please enter Email")]
        //[EmailAddress(ErrorMessage = "Invalid Email")]
        //public string Email { get; set; }

        [Required(ErrorMessage = "Please enter Password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
