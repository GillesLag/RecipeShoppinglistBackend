using RecipeShoppingList.Models;

namespace RecipeShoppinglist.DTOs;

public class CreateRecipeDto
{
    public ICollection<string> CookingInstruction { get; set; } = new List<string>();
    public string? Name { get; set; }
    public int Servings { get; set; }
    public ICollection<CreateRecipeIngredientDto> RecipeIngredients { get; set; } = new List<CreateRecipeIngredientDto>();
}
