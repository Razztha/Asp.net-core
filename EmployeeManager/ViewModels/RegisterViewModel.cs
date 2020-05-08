using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManager.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [Display(Name ="Confirm Passoword")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password and confirmation password doesn't match")]
        public string ConfirmPassword { get; set; }
    }
}
