using System.ComponentModel.DataAnnotations;

namespace RecipeShoppingList.Models;

public class Shoppinglist
{
    public int Id { get; set; }

    [Required]
    public string? Name { get; set; }
    public ICollection<ShoppinglistIngredient> ShoppinglistIngredients { get; set; } = new List<ShoppinglistIngredient>();
}
