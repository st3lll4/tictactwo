using DAL;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly AppDbContext _context;

    [BindProperty] public string Search { get; set; } = default!;

    public List<Recipe> Recipes = [];

    public IndexModel(ILogger<IndexModel> logger, AppDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public void OnGet()
    {
    }

    public IActionResult OnPost()
    {
        _logger.LogInformation(Search);
        Search = Search.Trim().ToLower();
        Recipes = _context.Recipes
            .Include(i => i.IngredientsInRecipe!) // vt teema 6
            .ThenInclude(j => j.Ingredient)
            .Where(r =>
                r.RecipeName.ToLower().Contains(Search) ||
                r.Description.ToLower().Contains(Search) ||
                r.IngredientsInRecipe!.Any(i => i.Ingredient!.IngredientName.ToLower().Contains(Search))
            )
            .ToList();

        // List<String> random = ["hello", "tere"];
        // var x =string.Join(", ", random);
        
        return Page();
    }
}