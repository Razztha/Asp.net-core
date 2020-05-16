using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManager.ViewModels
{
    public class EditRoleViewModel
    {
        public EditRoleViewModel()
        {
            Users = new List<string>();
        }
        public List<string> Users { get; set; }
        public string Id { get; set; }

        [Required]
        [Display(Name = "Role")]
        public string RoleName { get; set; }
    }
}
