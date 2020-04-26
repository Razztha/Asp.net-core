using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManager.Models
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>().HasData(
                new Employee
                {
                    Id = 1,
                    Name = "Rasitha",
                    Email = "rasitha@gmail.com",
                    Department = Dept.IS
                },
                new Employee
                {
                    Id = 2,
                    Name = "Jayani",
                    Email = "jayani@gmail.com",
                    Department = Dept.IT
                }
                );
        }
    }
}
