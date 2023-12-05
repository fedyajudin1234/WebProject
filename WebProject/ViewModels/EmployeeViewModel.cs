using System.ComponentModel.DataAnnotations;
using WebProject.Data.Models;

namespace WebProject.ViewModels
{
    public class EmployeeViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
        [Required]
        public string Patronymic { get; set; }
        [Required]
        public string Email { get; set; }
        public ICollection<Project> Projects { get; set; }
        public List<EmployeeProject> EmployeeProjects { get; set; }
        public List<Employee> EmployeeList { get; set; }
    }
}
