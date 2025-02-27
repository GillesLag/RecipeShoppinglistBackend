using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecipeShoppingList.Models;
using RecipeShoppingList.Repostories;

namespace RecipeShoppingList.Data;

public class RecipeContext : DbContext
{
    public DbSet<Recipe> Recipes { get; set; }
    public DbSet<Ingredient> Ingredients { get; set; }
    public DbSet<Models.Shoppinglist> Shoppinglists { get; set; }
    public DbSet<RecipeIngredient> RecipeIngredients { get; set; }
    public DbSet<ShoppinglistIngredient> ShoppinglistIngredients { get; set; }

    public RecipeContext(DbContextOptions<RecipeContext> options)
        :base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Recipe>()
            .HasMany(r => r.RecipeIngredients)
            .WithOne(i => i.Recipe);

        modelBuilder.Entity<Models.Shoppinglist>()
            .HasMany(s => s.ShoppinglistIngredients)
            .WithOne(i => i.Shoppinglist);

        modelBuilder.Entity<Ingredient>()
            .HasMany(i => i.RecipeIngredients)
            .WithOne(r => r.Ingredient);

        modelBuilder.Entity<Ingredient>()
            .HasMany(i => i.ShoppinglistIngredients)
            .WithOne(s => s.Ingredient);
    }
}
