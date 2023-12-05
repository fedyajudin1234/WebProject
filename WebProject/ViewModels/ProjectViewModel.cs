using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using WebProject.Data;
using WebProject.Data.Models;

namespace WebProject.ViewModels
{
    public class ProjectViewModel
    {
        private ApplicationContext applicationContext;
        public int Id { get; set; }
        [Required]
        public string Project_Name { get; set; }
        [Required]
        public string Company_Customer { get; set; }
        [Required]
        public string Company_Producer { get; set; }
        [Required]
        public DateTime Start_Date { get; set; }
        [Required]
        public DateTime Finish_Date { get; set; }
        [Required]
        public int ManagerId { get; set; }
        [Required]
        public int Priority { get; set; }
        public List<Employee> Employees { get; set; } = new List<Employee>();
        public List<Employee> EmployeeListForDictionary { get; set; } = new List<Employee>();
        public List<Project> ProjectList { get; set; }
        public List<EmployeeProject> EmployeeProjects { get; set; } = new List<EmployeeProject>();
        public List<Employee> ManagerList { get; set; } = new List<Employee>();
        public Dictionary<int, List<Employee>> EmployeeList { get; set; } = new Dictionary<int, List<Employee>>();
        public string Selected_Manager { get; set; }
        public List<SelectListItem> EmployeeSelectedList { get; set; }
        public string Selected_Employee { get; set; }
    }
}
