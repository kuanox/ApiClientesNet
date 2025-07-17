using Microsoft.EntityFrameworkCore;
using UserApi.Entities;

namespace UserApi.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {        
        modelBuilder.Entity<User>()
            .HasIndex(u => u.FirstName)
            .IsUnique();

        modelBuilder.Entity<User>()
            .HasIndex(u => u.LastName)
            .IsUnique();

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();
    }
}