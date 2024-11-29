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
    public class IndexModel : PageModel
    {
        private readonly DAL.AppDbContext _context;

        public IndexModel(DAL.AppDbContext context)
        {
            _context = context;
        }

        public IList<IngredientInRecipe> IngredientInRecipe { get;set; } = default!;

        public async Task OnGetAsync()
        {
            IngredientInRecipe = await _context.IngredientsInRecipes
                .Include(i => i.Ingredient)
                .Include(i => i.Recipe).ToListAsync();
        }
    }
}
