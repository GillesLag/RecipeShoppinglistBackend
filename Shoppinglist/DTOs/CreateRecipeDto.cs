using RecipeShoppingList.Models;
using System.ComponentModel.DataAnnotations;

namespace RecipeShoppinglist.DTOs;

public class CreateRecipeDto
{
    public string? Name { get; set; }
    public int Servings { get; set; }
    public ICollection<string> CookingInstructions { get; set; } = new List<string>();
    public ICollection<CreateRecipeIngredientDto> RecipeIngredients { get; set; } = new List<CreateRecipeIngredientDto>();
}
