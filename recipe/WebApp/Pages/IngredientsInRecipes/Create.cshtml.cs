using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using DAL;
using Domain;

namespace WebApp.Pages.IngredientsInRecipes
{
    public class CreateModel : PageModel
    {
        private readonly DAL.AppDbContext _context;

        public CreateModel(DAL.AppDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["IngredientId"] = new SelectList(_context.Ingredients, "Id", "IngredientName");
        ViewData["RecipeId"] = new SelectList(_context.Recipes, "Id", "RecipeName");
            return Page();
        }

        [BindProperty]
        public IngredientInRecipe IngredientInRecipe { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.IngredientsInRecipes.Add(IngredientInRecipe);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
