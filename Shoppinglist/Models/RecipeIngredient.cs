using RecipeShoppingList.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace RecipeShoppingList.Models;

public class RecipeIngredient
{
    public int Id { get; set; }
    public int RecipeId { get; set; }
    public int IngredientId { get; set; }

    [Required]
    public float Quantity { get; set; }

    [Required]
    public Measurement Measurement { get; set; }

    public Recipe? Recipe { get; set; }
    public Ingredient? Ingredient { get; set; }
}
