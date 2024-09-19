using Domain.Entity;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public DbSet<UserData> UserData { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserData>()
            .Property(u => u.Salary)
            .HasPrecision(18, 2);
    }
}

