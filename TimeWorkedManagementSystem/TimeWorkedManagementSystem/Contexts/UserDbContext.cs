using Microsoft.EntityFrameworkCore;
using TimeWorkedManagementSystem.Interfaces;
using TimeWorkedManagementSystem.Models;

namespace TimeWorkedManagementSystem.Contexts
{
    public class UserDbContext : DbContext
    {
        public IUserService UserService { get; }
        public DbSet<User> Users { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Shift> Shifts { get; set; }
        public DbSet<Break> Breaks { get; set; }

        public UserDbContext(DbContextOptions<UserDbContext> options, IUserService userService) : base(options)
        {
            UserService = userService;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Company>().HasQueryFilter(c => c.UserId == UserService.UserId);
            modelBuilder.Entity<Shift>().HasQueryFilter(s => s.UserId == UserService.UserId);
            modelBuilder.Entity<Break>().HasQueryFilter(b => b.UserId == UserService.UserId);

            modelBuilder.Entity<Shift>().HasOne(s => s.Company).WithMany(c => c.Shifts).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
