using Microsoft.AspNetCore.Mvc;
using WebProject.Data;
using WebProject.Data.Models;
using WebProject.ViewModels;

namespace WebProject.Controllers
{
    public class EmployeeController: Controller
    {
        private readonly ApplicationContext applicationContext;
        public EmployeeController(ApplicationContext applicationContext)
        {
            this.applicationContext = applicationContext;
        }

        [HttpGet]
        public ActionResult AddEmployee()
        {
            EmployeeViewModel employeeViewModel = new EmployeeViewModel();
            employeeViewModel.Projects = applicationContext.Projects.ToList();
            return View(employeeViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddEmployeeAsync(EmployeeViewModel employeeViewModel)
        {
            if (employeeViewModel.Name != null && employeeViewModel.Surname != null
                && employeeViewModel.Patronymic != null && employeeViewModel.Email != null)
            {
                Employee employee = new Employee()
                {
                    Name = employeeViewModel.Name,
                    Surname = employeeViewModel.Surname,
                    Patronymic = employeeViewModel.Patronymic,
                    Email = employeeViewModel.Email
                };
                applicationContext.Add(employee);
                await applicationContext.SaveChangesAsync();
            }
            return Redirect("/Employee/ListOfEmployee");
        }

        [HttpGet]
        public ActionResult ListOfEmployee()
        {
            EmployeeViewModel employeeViewModel = new EmployeeViewModel();
            employeeViewModel.EmployeeList = applicationContext.Employees.ToList();
            employeeViewModel.Projects = applicationContext.Projects.ToList();
            return View(employeeViewModel);
        }

        [HttpGet]
        public ActionResult EditEmployee(int id)
        {
            Employee employee = applicationContext.Employees.Where(e => e.Id == id).FirstOrDefault();
            return View(employee);
        }

        [HttpPost]
        public async Task<IActionResult> EditEmployeeAsync(Employee employee)
        {
            if (employee.Name != null && employee.Surname != null
                && employee.Patronymic != null && employee.Email != null
                && employee.Email.Contains("@") && employee.Email.Contains("."))
            {
                applicationContext.Update(employee);
                await applicationContext.SaveChangesAsync();
            }
            return Redirect("/Employee/ListOfEmployee");
        }

        [HttpGet]
        public ActionResult DeleteEmployee(int id)
        {
            Employee employee = applicationContext.Employees.Where(e => e.Id == id).FirstOrDefault();
            return View(employee);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteEmployeeAsync(Employee employee)
        {
            applicationContext.Remove(employee);
            await applicationContext.SaveChangesAsync();
            return Redirect("/Employee/ListOfEmployee");
        }
    }
}
