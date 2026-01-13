using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProgrammerTest_ThaiAgriFood.Models
{
    public class Employee
    {
        [Key]
        [Column("Employee_ID")]
        public string Employee_ID { get; set; }
        public string Employee_First_name { get; set; }
        public string Employee_Last_name { get; set; }
        public string Gender { get; set; }
        public DateTime Date_of_Birth { get; set; }
        public DateTime Date_Joined { get; set; }
        public string? Employee_Address { get; set; }
        public string? Photo { get; set; }
        public string Department_ID { get; set; }
    }
}
