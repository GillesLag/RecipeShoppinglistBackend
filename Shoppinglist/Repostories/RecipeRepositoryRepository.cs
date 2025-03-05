using Microsoft.EntityFrameworkCore;
using RecipeShoppingList.Data;
using RecipeShoppingList.Models;
using RecipeShoppingList.Repostories.Interfaces;

namespace RecipeShoppingList.Repostories;

public class RecipeRepositoryRepository : GenericRepository<Recipe>, IRecipeRepository
{
    public RecipeRepositoryRepository(RecipeContext context)
        : base(context)
    {
        
    }

    public override IEnumerable<Recipe> GetAll()
    {
        return _context.Recipes
            .Include(r => r.RecipeIngredients)
            .ThenInclude(ri => ri.Ingredient);
    }

    public override Recipe? GetById(int id)
    {
        return _context.Recipes
            .Include(r => r.RecipeIngredients)
            .ThenInclude(i => i.Ingredient)
            .SingleOrDefault(r => r.Id == id);
    }
}
