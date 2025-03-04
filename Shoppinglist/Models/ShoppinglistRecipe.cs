using RecipeShoppingList.Models;
using System.ComponentModel.DataAnnotations;

namespace RecipeShoppinglist.Models
{
    public class ShoppinglistRecipe
    {
        public int Id { get; set; }

        [Required]
        public int ShoppinglistId { get; set; }

        [Required]
        public int RecipeId { get; set; }
        public Recipe? Recipe { get; set; }
        public Shoppinglist? Shoppinglist { get; set; }
    }
}
