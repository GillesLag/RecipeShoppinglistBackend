using Microsoft.EntityFrameworkCore;
using RecipeShoppingList.Data;
using RecipeShoppingList.Models;
using RecipeShoppingList.Repostories.Interfaces;

namespace RecipeShoppingList.Repostories;

public class ShoppinglistRepository : GenericRepository<Shoppinglist>, IShoppinglistRepository
{
    public ShoppinglistRepository(RecipeContext context)
        : base(context)
    {
        
    }

    public override IEnumerable<Shoppinglist> GetAll()
    {
        return _context.Shoppinglists
            .Include(s => s.ShoppinglistIngredients)
            .ThenInclude(si => si.Ingredient);
    }

    public override Shoppinglist? GetById(int id)
    {
        return _context.Shoppinglists
            .Include(s => s.ShoppinglistIngredients)
            .ThenInclude(si => si.Ingredient)
            .Include(s => s.ShoppinglistRecipes)
            .ThenInclude(sr => sr.Recipe)
            .SingleOrDefault(s => s.Id == id);
    }

    public Shoppinglist GetLatestShoppinglist()
    {
        return _context.Shoppinglists.Last();
    }
}
