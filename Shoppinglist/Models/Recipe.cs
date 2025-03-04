using RecipeShoppinglist.Models;
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
    public string? CookingInstructions { get; set; }

    [Required]
    public ICollection<RecipeIngredient> RecipeIngredients { get; set; } = new List<RecipeIngredient>();
    public ICollection<ShoppinglistRecipe> ShoppinglistRecipes { get; set; } = new List<ShoppinglistRecipe>();
}
