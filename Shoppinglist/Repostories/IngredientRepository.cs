using RecipeShoppingList.Data;
using RecipeShoppingList.Models;
using RecipeShoppingList.Repostories.Interfaces;

namespace RecipeShoppingList.Repostories;

public class IngredientRepository : GenericRepository<Ingredient>, IIngredientRepository
{
    public IngredientRepository(RecipeContext context)
        : base(context)
    {
        
    }
}
