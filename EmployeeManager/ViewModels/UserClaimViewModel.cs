using EmployeeManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManager.ViewModels
{
    public class UserClaimViewModel
    {
        public UserClaimViewModel()
        {
            UserClaims = new List<UserClaim>();
        }
        public string UserId { get; set; }
        public List<UserClaim> UserClaims { get; set; }
    }
}
