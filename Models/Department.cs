using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProgrammerTest_ThaiAgriFood.Models
{
    public class Department
    {
        [Key]
        [Column("Department_ID")]
        public string Department_ID { get; set; }
        public string Department_Name { get; set; }
        public string Department_Address { get; set; }
    }
}
