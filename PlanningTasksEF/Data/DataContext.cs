using Microsoft.EntityFrameworkCore;
using PlanningTasksEF.Models;

namespace PlanningTasksEF.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer("Data Source=127.0.0.1;Initial Catalog=Planning;User ID=sa;Password=Password.1;Connect Timeout=60;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        }

        public DbSet<Tasks> Tasks { get; set; }
    }
}
