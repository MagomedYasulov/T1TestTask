using Microsoft.EntityFrameworkCore;
using T1TestTask.Data.Entites;

namespace T1TestTask.Data
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Course> Courses => Set<Course>();
        public DbSet<Student> Students => Set<Student>();


        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Course>().
                HasMany(g => g.Students)
                .WithOne(m => m.Course).
                OnDelete(DeleteBehavior.Cascade);

        }
    }
}
