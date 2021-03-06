using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AutoShop.Web.Models
{
    //public class UserViewModels
    //{
    //    public int Id { get; set; }
    //    public string Login { get; set; }
    //    public string Password { get; set; }
    //    public string ConfirmPassword { get; set; }
    //    public string KeyWord { get; set; }
    //}

    //public class CreateUserViewModel
    //{
    //    [Required]
    //    [MaxLength(28)]
    //    [MinLength(6)]
    //    public string Login { get; set; }


    //    [Required]
    //    [MinLength(8)]
    //    [DataType(DataType.Password)]
    //    public string Password { get; set; }

    //    [Required]
    //    [Compare("Password", ErrorMessage = "Passwords must match.")]
    //    [DataType(DataType.Password)]
    //    [Display(Name = "Confirm Password")]
    //    public string ConfirmPassword { get; set; }

    //    [Required]
    //    [Display(Name = "Keyword")]
    //    public string KeyWord { get; set; }
    //}
    public class UserViewModel
    {
        
        public string Id { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public DateTime DateRegestry { get; set; }
        public string PathImage { get; set; }
    }

    //public class UserViewModel
    //{
    //    public IList<User> Users { get; set;}
    //}

    public class CreateUserViewModel
    {
        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Incorrect Email")]
        [Required(ErrorMessage = "Required Attribute")]
        public string Email { get; set; }

        [Required]
        [MinLength(8)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

    }

    public class EditUserViewModel
    {
        [Required]
        public string Id { get; set; }
        public string Email { get; set; }
        public string NameUser { get; set; }
        public string RoleSelect { get; set; }

        [Display(Name = "Фото")]
        public IFormFile Photo { get; set; }
    }

    //public class UserSiginVievModel
    //{
    //    [Required]
    //    public string Login { get; set; }
    //    [Required]
    //    public string Password { get; set; }
    //    [Required]
    //    public string ReturnUrl { get; set; }

    //}


    //public class SearthUserEditVievModel
    //{
    //    [Required]
    //    public string Login { get; set; }
    //}

    //public class AuditUserEditVievModel
    //{
    //    [Required]
    //    public string Word { get; set; }
    //}


    //public class EditPasswordVievModel
    //{
    //    [Required]
    //    [MinLength(8)]
    //    [DataType(DataType.Password)]
    //    public string NewPassword { get; set; }

    //    [Required]
    //    [Compare("NewPassword", ErrorMessage = "Passwords must match.")]
    //    [DataType(DataType.Password)]
    //    [Display(Name = "Confirm Password")]
    //    public string ConfirmPassword { get; set; }
    //}
}
