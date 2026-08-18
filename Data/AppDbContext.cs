using Microsoft.EntityFrameworkCore;
using SimpleSIS.Models;

namespace SimpleSIS.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Student> Students => Set<Student>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Student>().HasData(
            new Student
            {
                Id = 1,
                StudentNumber = "2026-001",
                FullName = "Juan Dela Cruz",
                GradeLevel = 12,
                Section = "St. Paul",
                Strand = "ICT"
            },
            new Student
            {
                Id = 2,
                StudentNumber = "2026-002",
                FullName = "Maria Santos",
                GradeLevel = 12,
                Section = "St. Paul",
                Strand = "ICT"
            });
    }
}
