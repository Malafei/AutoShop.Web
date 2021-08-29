using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AutoShop.Web.Models
{
    public class RegisterViewModels
    {
        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Incorrect Email")]
        [Required (ErrorMessage = "Required Attribute")]
        public string Email { get; set; }


        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Required Attribute")]
        public string Password { get; set; }


        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords must match.")]
        [Required(ErrorMessage = "Required Attribute")]
        public string ConfirmPassword { get; set; }

    }

    public class LoginViewModels
    {
        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Incorrect Email")]
        [Required(ErrorMessage = "Required Attribute")]
        public string Email { get; set; }


        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Required Attribute")]
        public string Password { get; set; }


        public string ReturnUrl { get; set; }
    }
}
