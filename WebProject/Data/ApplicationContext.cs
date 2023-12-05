using Microsoft.EntityFrameworkCore;
using WebProject.Data.Models;

namespace WebProject.Data
{
    public class ApplicationContext: DbContext
    {
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<EmployeeProject> EmployeeProjects { get; set; }
        public ApplicationContext()
        {

        }
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<EmployeeProject>()
            //    .HasKey(ep => new { ep.ProjectId, ep.EmployeeId });
            //modelBuilder.Entity<EmployeeProject>()
            //    .HasOne(ep => ep.Employee)
            //    .WithMany(e => e.EmployeeProjects)
            //    .HasForeignKey(e => e.EmployeeId).IsRequired(true);
            //modelBuilder.Entity<EmployeeProject>()
            //    .HasOne(ep => ep.Project)
            //    .WithMany(p => p.EmployeeProjects)
            //    .HasForeignKey(p => p.ProjectId);
            //modelBuilder.Entity<EmployeeProject>().Property(k => k.ProjectId).ValueGeneratedNever();
            //modelBuilder.Entity<EmployeeProject>().Property(k => k.EmployeeId).ValueGeneratedNever();


            modelBuilder.Entity<Project>()
                .HasMany(e => e.Employees)
                .WithMany(p => p.Projects)
                .UsingEntity<EmployeeProject>(
                 value => value.HasOne(e => e.Employee)
                 .WithMany(ep => ep.EmployeeProjects)
                 .HasForeignKey(efk => efk.EmployeeId).IsRequired(true),
                 value => value.HasOne(p => p.Project)
                 .WithMany(ep => ep.EmployeeProjects)
                 .HasForeignKey(pfk => pfk.ProjectId).IsRequired(true),
                 value =>
                 {
                     value.HasKey(temp => new { temp.EmployeeId, temp.ProjectId });
                 }
                );
            //modelBuilder.Entity<EmployeeProject>().Property(k => k.ProjectId).ValueGeneratedNever();
            //modelBuilder.Entity<EmployeeProject>().Property(k => k.EmployeeId).ValueGeneratedNever();
            //modelBuilder.Entity<EmployeeProject>().HasKey(k => new { k.EmployeeId, k.ProjectId });
            //modelBuilder.Entity<Employee>().HasMany(e => e.EmployeeProjects).With
        }
    }
}
