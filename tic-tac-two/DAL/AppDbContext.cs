using Domain;
using Microsoft.EntityFrameworkCore;


namespace DAL;

public class AppDbContext : DbContext
{
    public DbSet<Game> Games { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Configuration> Configurations { get; set; }
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Game>()
            .HasOne(g => g.User1)
            .WithMany(u => u.Games)
            .HasForeignKey(g => g.User1Id)
            .OnDelete(DeleteBehavior.ClientSetNull); 

        modelBuilder.Entity<Game>()
            .HasOne(g => g.User2)
            .WithMany() // empty bc games come through user
            .HasForeignKey(g => g.User2Id)
            .OnDelete(DeleteBehavior.ClientSetNull);
    }

}