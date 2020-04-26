using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManager.Models
{
    public class MockEmployeeRepository : IEmployeeRepository
    {
        private List<Employee> _employeeList;

        public MockEmployeeRepository()
        {
            _employeeList = new List<Employee>(){
                new Employee() {Id = 1, Name = "Rasitha", Email = "rasitha@gmail.com", Department = Dept.IT},
                new Employee() {Id = 2, Name = "Jayani", Email = "jayani@gmail.com", Department = Dept.IS},
                new Employee() {Id = 3, Name = "Kamal", Email = "kamal@gmail.com", Department = Dept.HR}
            };
        }
        public Employee GetEmployee(int id)
        {
            return _employeeList.FirstOrDefault(e => e.Id == id);
        }

        public IEnumerable<Employee> GetAllEmployees()
        {
            return _employeeList;
        }

        public Employee Add(Employee employee)
        {
            employee.Id = _employeeList.Max(e => e.Id) + 1; 
            _employeeList.Add(employee);
            return employee;
        }

        public Employee Delete(int id)
        {
            var employee = _employeeList.FirstOrDefault(e => e.Id == id);
            _employeeList.Remove(employee);

            return employee;
        }

        public Employee Edit(Employee editEmployee)
        {
            var employee = _employeeList.FirstOrDefault(e => e.Id == editEmployee.Id);
            if (employee != null)
            {
                editEmployee.Name = editEmployee.Name;
                editEmployee.Email = editEmployee.Email;
                editEmployee.Department = editEmployee.Department;
            }
            return employee;
        }
    }
}