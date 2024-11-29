using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DAL;
using Domain;

namespace WebApp.Pages.IngredientsInRecipes
{
    public class DetailsModel : PageModel
    {
        private readonly DAL.AppDbContext _context;

        public DetailsModel(DAL.AppDbContext context)
        {
            _context = context;
        }

        public IngredientInRecipe IngredientInRecipe { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ingredientinrecipe = await _context.IngredientsInRecipes.FirstOrDefaultAsync(m => m.Id == id);

            if (ingredientinrecipe is not null)
            {
                IngredientInRecipe = ingredientinrecipe;

                return Page();
            }

            return NotFound();
        }
    }
}
