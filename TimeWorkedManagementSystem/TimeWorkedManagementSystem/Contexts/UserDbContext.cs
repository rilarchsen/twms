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

            modelBuilder.Entity<Shift>().HasOne(s => s.Company).WithMany(c => c.Shifts).OnDelete(DeleteBehavior.Cascade);
        }

        public sealed override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var item in ChangeTracker.Entries<OwnedEntityBase>())
            {
                item.Entity.UserId = UserService.UserId;
                switch (item.State)
                {
                    case EntityState.Added:
                        item.Entity.CreatedOn = DateTimeOffset.UtcNow;
                        break;
                    case EntityState.Modified:
                        item.Entity.ModifiedOn = DateTimeOffset.UtcNow;
                        break;
                }
            }
            return await base.SaveChangesAsync(cancellationToken);
        }
        
        public sealed override int SaveChanges()
        {
            foreach (var item in ChangeTracker.Entries<OwnedEntityBase>())
            {
                item.Entity.UserId = UserService.UserId;
                switch (item.State)
                {
                    case EntityState.Added:
                        item.Entity.CreatedOn = DateTimeOffset.UtcNow;
                        break;
                    case EntityState.Modified:
                        item.Entity.ModifiedOn = DateTimeOffset.UtcNow;
                        break;
                }
            }
            return base.SaveChanges();
        }
    }
}
