using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebProject.Data.Models
{
    public class Employee
    {
        //[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        //[DefaultValue("0")]
        [Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.None)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Patronymic { get; set; }
        public string Email { get; set; }
        public List<Project>? Projects { get; set; }
        public List<EmployeeProject> EmployeeProjects { get; set; }
    }
}
