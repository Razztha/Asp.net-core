using EmployeeManager.Models;
using EmployeeManager.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManager.Controllers
{
    public class HomeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;

        public HomeController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }
        public ViewResult Index()
        {
            var model = _employeeRepository.GetAllEmployees();

            return View(model);
        }

        public ViewResult Details(int? id)
        {
            HomeDetailsViewModel EmployeeViewModel = new HomeDetailsViewModel()
            {
                Employee = _employeeRepository.GetEmployee(id??1),
                PageTitle = "Employee Manager"
            };
            
            return View(EmployeeViewModel);
        }

        [HttpGet]
        public ViewResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(EmployeeCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                Employee employee = new Employee
                {
                    Name = model.Name,
                    Email = model.Email,
                    Department = model.Department
                };

                _employeeRepository.Add(employee);
                return RedirectToAction("details", new { id = employee.Id });
            }
            return View();
        }

        [HttpGet]
        public ViewResult Edit(int? id)
        {
            var employee = _employeeRepository.GetEmployee(id??1);
            return View("create", employee);
        }

        [HttpPost]
        public RedirectToActionResult Edit(Employee employee)
        {
            _employeeRepository.Edit(employee);
            return RedirectToAction("details", new {id = employee.Id});
        }
    }
}