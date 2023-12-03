using CallAppTask.DTO;
using Microsoft.EntityFrameworkCore;

namespace CallAppTask.Repositories
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        public DbSet<User> User { get; set; }
        public DbSet<UserProfile> UserProfile { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(b =>

            {
                b.HasData(
                    new User
                    {
                        Id = 1,
                        UserName = "Luke",
                        Password = "Luke123"
                    });

                b.OwnsOne(u => u.UserProfile).HasData
                (
                    new UserProfile
                    {
                        Id = 1,
                        PersonalNumber = "01010039867"
                    });
            });
            
        }
    }
}
