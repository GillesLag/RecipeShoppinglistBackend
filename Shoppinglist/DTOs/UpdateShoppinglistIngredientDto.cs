using RecipeShoppingList.Models;
using RecipeShoppingList.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace RecipeShoppinglist.DTOs;

public class UpdateShoppinglistIngredientDto
{
    public int Id { get; set; }
    public int ShoppinglistId { get; set; }
    public int IngredientId { get; set; }

    [Required]
    public float Quantity { get; set; }

    [Required]
    public Measurement Measurement { get; set; }
    public bool IsChecked { get; set; }
}
