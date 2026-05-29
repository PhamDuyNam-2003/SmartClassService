using auth_service.Models;
using Microsoft.EntityFrameworkCore;

namespace auth_service.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
   


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>()
     .HasIndex(x => x.Email)
     .IsUnique();

        modelBuilder.Entity<User>()
            .HasIndex(x => x.UserName)
            .IsUnique();

        modelBuilder.Entity<User>()
            .HasIndex(x => x.NormalizedEmail)
            .IsUnique();

        modelBuilder.Entity<User>()
            .HasIndex(x => x.NormalizedUserName)
            .IsUnique();
              
    }
}