using System.ComponentModel.DataAnnotations;

namespace RecipeShoppingList.Models;

public class Ingredient
{
    public int Id { get; set; }

    [Required]
    public string? Name { get; set; }
    public ICollection<RecipeIngredient> RecipeIngredients { get; set; } = new List<RecipeIngredient>();
    public ICollection<ShoppinglistIngredient> ShoppinglistIngredients { get; set; } = new List<ShoppinglistIngredient>();
}
