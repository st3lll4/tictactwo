using System.ComponentModel.DataAnnotations;

namespace Domain;

public class Recipe : BaseEntity
{
    [MaxLength(128)] public string RecipeName { get; set; } = default!;
    public string Description { get; set; } = default!;
    public int Portion { get; set; } 
    public int PrepTime { get; set; } // min
    public int CookTime { get; set; } // min

    public ICollection<IngredientInRecipe>? IngredientsInRecipe { get; set; }
}