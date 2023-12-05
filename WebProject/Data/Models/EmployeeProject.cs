using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebProject.Data.Models
{
    public class EmployeeProject
    {
        [Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int? EmployeeId { get; set; }
        [Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int? ProjectId { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual Project Project { get; set; }
    }
}
