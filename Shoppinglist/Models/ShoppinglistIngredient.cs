using RecipeShoppingList.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace RecipeShoppingList.Models;

public class ShoppinglistIngredient
{
    public int Id { get; set; }
    public int ShoppinglistId { get; set; }
    public int IngredientId { get; set; }

    [Required]
    public float Quantity { get; set; }

    [Required]
    public Measurement Measurement { get; set; }


    public Shoppinglist? Shoppinglist { get; set; }
    public Ingredient? Ingredient { get; set; }
}
