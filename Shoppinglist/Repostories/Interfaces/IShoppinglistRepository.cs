using RecipeShoppingList.Models;

namespace RecipeShoppingList.Repostories.Interfaces;

public interface IShoppinglistRepository : IGenericRepository<Models.Shoppinglist>
{
    public Models.Shoppinglist GetLatestShoppinglist();
}
