using RecipeShoppinglist.Repostories.Interfaces;
using RecipeShoppingList.Repostories.Interfaces;

namespace RecipeShoppingList.Repostories;

public interface IUnitOfWork
{
    public IRecipeRepository RecipeRepo { get; }
    public IIngredientRepository IngredientRepo { get; }
    public IShoppinglistRepository ShoppinglistRepo { get; }
    public IShoppinglistIngredientRepository ShoppinglistIngredientRepo { get; }
    public void Save();
}
