using Domain;
using GameLogic;
using Microsoft.EntityFrameworkCore;

namespace DAL;

public class AppDbContext : DbContext
{
    public DbSet<Game> Games { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<GameConfiguration> Configurations { get; set; }
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    
}