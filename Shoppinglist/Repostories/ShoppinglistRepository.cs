using Microsoft.EntityFrameworkCore;
using RecipeShoppingList.Data;
using RecipeShoppingList.Models;
using RecipeShoppingList.Repostories.Interfaces;

namespace RecipeShoppingList.Repostories;

public class ShoppinglistRepository : GenericRepository<Models.Shoppinglist>, IShoppinglistRepository
{
    public ShoppinglistRepository(RecipeContext context)
        : base(context)
    {
        
    }

    public override Models.Shoppinglist? GetById(int id)
    {
        return _context.Shoppinglists
            .Include(s => s.ShoppinglistIngredients)
            .ThenInclude(si => si.Ingredient)
            .SingleOrDefault(s => s.Id == id);
    }

    public Models.Shoppinglist GetLatestShoppinglist()
    {
        return _context.Shoppinglists.Last();
    }
}
