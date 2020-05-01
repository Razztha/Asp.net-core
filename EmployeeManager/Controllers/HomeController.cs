using EmployeeManager.Models;
using EmployeeManager.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Hosting.Internal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;

namespace EmployeeManager.Controllers
{
    public class HomeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IWebHostEnvironment webHostEnvironment;

        public HomeController(IEmployeeRepository employeeRepository, IWebHostEnvironment webHostEnvironment)
        {
            _employeeRepository = employeeRepository;
            this.webHostEnvironment = webHostEnvironment;
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
                string uniqueFileName = null;
                if (model.Photo != null)
                {
                    string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images");
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Photo.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    model.Photo.CopyTo(new FileStream(filePath, FileMode.Create));
                }

                Employee employee = new Employee
                {
                    Name = model.Name,
                    Email = model.Email,
                    Department = model.Department,
                    PhotoPath = uniqueFileName
                };

                _employeeRepository.Add(employee);
                return RedirectToAction("details", new { id = employee.Id });
            }
            return View();
        }

        [HttpGet]
        public ViewResult Edit(int id)
        {
            var employee = _employeeRepository.GetEmployee(id);

            if (employee == null)
            {
                return View("index");
            }

            var editEmployee = new EmployeeEditViewModel
            {
                Id = employee.Id,
                Name = employee.Name,
                Email = employee.Email,
                Department = employee.Department,
                ExistingPhotoPath = employee.PhotoPath
            };

            return View("edit", editEmployee);
        }

        [HttpPost]
        public IActionResult Edit(EmployeeEditViewModel editEmployee)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = null;
                if (editEmployee.Photo != null)
                {
                    string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images");
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + editEmployee.Photo.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    editEmployee.Photo.CopyTo(new FileStream(filePath, FileMode.Create));
                }

                Employee employee = new Employee
                {
                    Id = editEmployee.Id,
                    Name = editEmployee.Name,
                    Email = editEmployee.Email,
                    Department = editEmployee.Department,
                    PhotoPath = uniqueFileName
                };

                _employeeRepository.Edit(employee);
                return RedirectToAction("details", new { id = employee.Id });
            }

            return View(editEmployee);
        }

        [HttpGet]
        public RedirectToActionResult Delete(int id)
        {
            _employeeRepository.Delete(id);
            return RedirectToAction("index");
        }
    }
}