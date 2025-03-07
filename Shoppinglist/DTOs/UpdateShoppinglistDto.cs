using RecipeShoppingList.Models;
using System.ComponentModel.DataAnnotations;

namespace RecipeShoppinglist.DTOs;

public class UpdateShoppinglistDto
{
    public int Id { get; set; }

    [Required]
    public string? Name { get; set; }
    public ICollection<UpdateShoppinglistIngredientDto> ShoppinglistIngredients { get; set; } = new List<UpdateShoppinglistIngredientDto>();
}
