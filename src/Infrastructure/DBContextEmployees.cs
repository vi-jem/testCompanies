using myrestful.Models;
using Microsoft.EntityFrameworkCore;

namespace myrestful.Infrastructure
{
    public class DBContextEmployees : DbContext
    {

        public DbSet<Company> Companies { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DBContextEmployees(DbContextOptions<DBContextEmployees> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Employee>()
                .Property(s => s.JobTitle)
                .HasConversion<int>();

            modelBuilder
                .Entity<Company>()
                .HasMany(c => c.Employees)
                .WithOne(e => e.Company)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
        }
    }
}