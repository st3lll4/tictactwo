using Domain;
using Microsoft.EntityFrameworkCore;

namespace DAL;

public class AppDbContext : DbContext
{
    public DbSet<Recipe> Recipes { get; set; }
    public DbSet<Ingredient> Ingredients { get; set; }
    public DbSet<IngredientInRecipe> IngredientsInRecipes { get; set; }
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
}