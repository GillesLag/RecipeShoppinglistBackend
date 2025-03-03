using RecipeShoppinglist.Repostories.Interfaces;
using RecipeShoppingList.Data;
using RecipeShoppingList.Models;
using RecipeShoppingList.Repostories;

namespace RecipeShoppinglist.Repostories
{
    public class ShoppinglistIngredientRepository : GenericRepository<ShoppinglistIngredient>, IShoppinglistIngredientRepository
    {
        public ShoppinglistIngredientRepository(RecipeContext context) : base(context)
        {
        }

    }
}
