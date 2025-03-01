using RecipeShoppingList.Models;

namespace RecipeShoppingList.Repostories.Interfaces;

public interface IShoppinglistRepository : IGenericRepository<Shoppinglist>
{
    public Shoppinglist GetLatestShoppinglist();
}
