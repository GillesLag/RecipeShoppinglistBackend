using System.ComponentModel.DataAnnotations;

namespace RecipeShoppingList.Models;

public class Recipe
{
    public int Id { get; set; }

    [Required]
    public string? Name { get; set; }

    [Required]
    public int Servings { get; set; }

    [Required]
    public ICollection<string> CookingInstructions { get; set; } = new List<string>();

    [Required]
    public ICollection<RecipeIngredient> RecipeIngredients { get; set; } = new List<RecipeIngredient>();
}
