using RecipeShoppingList.Models;

namespace RecipeShoppinglist.DTOs;

public class CreateRecipeDto
{
    public string[]? CookingInstruction { get; set; }
    public string? Name { get; set; }
    public int Servings { get; set; }
    public ICollection<CreateRecipeIngredientDto> RecipeIngredients { get; set; } = new List<CreateRecipeIngredientDto>();
}
