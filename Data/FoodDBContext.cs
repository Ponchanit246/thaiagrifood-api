using Microsoft.EntityFrameworkCore;
using ProgrammerTest_ThaiAgriFood.Models;

namespace ProgrammerTest_ThaiAgriFood.Data
{
    public class FoodDBContext : DbContext
    {

        public FoodDBContext(DbContextOptions<FoodDBContext> options) : base(options) { }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }
    }
}
