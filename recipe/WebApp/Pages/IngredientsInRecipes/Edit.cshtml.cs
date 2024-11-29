using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DAL;
using Domain;

namespace WebApp.Pages.IngredientsInRecipes
{
    public class EditModel : PageModel
    {
        private readonly DAL.AppDbContext _context;

        public EditModel(DAL.AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public IngredientInRecipe IngredientInRecipe { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ingredientinrecipe =  await _context.IngredientsInRecipes.FirstOrDefaultAsync(m => m.Id == id);
            if (ingredientinrecipe == null)
            {
                return NotFound();
            }
            IngredientInRecipe = ingredientinrecipe;
           ViewData["IngredientId"] = new SelectList(_context.Ingredients, "Id", "IngredientName");
           ViewData["RecipeId"] = new SelectList(_context.Recipes, "Id", "Description");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(IngredientInRecipe).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IngredientInRecipeExists(IngredientInRecipe.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool IngredientInRecipeExists(int id)
        {
            return _context.IngredientsInRecipes.Any(e => e.Id == id);
        }
    }
}
