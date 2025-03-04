using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecipeShoppinglist.Models;
using RecipeShoppingList.Models;
using RecipeShoppingList.Repostories;

namespace RecipeShoppingList.Data;

public class RecipeContext : DbContext
{
    public DbSet<Recipe> Recipes { get; set; }
    public DbSet<Ingredient> Ingredients { get; set; }
    public DbSet<Shoppinglist> Shoppinglists { get; set; }
    public DbSet<RecipeIngredient> RecipeIngredients { get; set; }
    public DbSet<ShoppinglistIngredient> ShoppinglistIngredients { get; set; }
    public DbSet<ShoppinglistRecipe> ShoppinglistRecipes { get; set; }

    public RecipeContext(DbContextOptions<RecipeContext> options)
        :base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Recipe>()
            .HasMany(r => r.RecipeIngredients)
            .WithOne(i => i.Recipe);

        modelBuilder.Entity<Recipe>()
            .HasMany(r => r.ShoppinglistRecipes)
            .WithOne(sr => sr.Recipe);

        modelBuilder.Entity<Shoppinglist>()
            .HasMany(s => s.ShoppinglistIngredients)
            .WithOne(i => i.Shoppinglist);

        modelBuilder.Entity<Shoppinglist>()
            .HasMany(s => s.ShoppinglistRecipes)
            .WithOne(sr => sr.Shoppinglist);

        modelBuilder.Entity<Ingredient>()
            .HasMany(i => i.RecipeIngredients)
            .WithOne(r => r.Ingredient);

        modelBuilder.Entity<Ingredient>()
            .HasMany(i => i.ShoppinglistIngredients)
            .WithOne(s => s.Ingredient);
    }
}
