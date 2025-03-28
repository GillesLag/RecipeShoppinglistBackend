﻿using RecipeShoppinglist.Repostories;
using RecipeShoppinglist.Repostories.Interfaces;
using RecipeShoppingList.Data;
using RecipeShoppingList.Repostories.Interfaces;

namespace RecipeShoppingList.Repostories;

public class UnitOfWork : IUnitOfWork
{
    private readonly RecipeContext _context;

    public IRecipeRepository RecipeRepo { get; private set; }
    public IIngredientRepository IngredientRepo { get; private set; }
    public IShoppinglistRepository ShoppinglistRepo { get; private set; }
    public IShoppinglistIngredientRepository ShoppinglistIngredientRepo { get; private set; }

    public UnitOfWork(RecipeContext context)
    {
        _context = context;

        RecipeRepo = new RecipeRepositoryRepository(context);
        IngredientRepo = new IngredientRepository(context);
        ShoppinglistRepo = new ShoppinglistRepository(context);
        ShoppinglistIngredientRepo = new ShoppinglistIngredientRepository(context);
    }

    public void Save()
    {
        _context.SaveChanges();
    }
}
