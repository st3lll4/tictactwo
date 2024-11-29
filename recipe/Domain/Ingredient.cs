namespace Domain;

public class Ingredient : BaseEntity
{
    public string IngredientName { get; set; } = default!;
    public string Unit { get; set; } = default!;
    
    public ICollection<IngredientInRecipe>? IngredientsInRecipe { get; set; }
}