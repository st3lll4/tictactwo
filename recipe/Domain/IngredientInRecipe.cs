namespace Domain;

public class IngredientInRecipe : BaseEntity
{
    public decimal Amount { get; set; } // vb pole taisarv
    
    //fk
    public int RecipeId { get; set; }
    public Recipe? Recipe { get; set; }

    public int IngredientId { get; set; }
    public Ingredient? Ingredient { get; set; }
}