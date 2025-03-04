using Microsoft.EntityFrameworkCore;
using RecipeShoppinglist.Models;
using RecipeShoppingList.Models;
using RecipeShoppingList.Models.Enums;

namespace RecipeShoppingList.Data;

public static class SeedData
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using (var context = new RecipeContext(serviceProvider.GetRequiredService<DbContextOptions<RecipeContext>>()))
        {
            if (!context.Recipes.Any())
            {
                context.Recipes.AddRange
                (
                    new Recipe
                    {
                        Id = 1,
                        Name = "Pannekoeken",
                        Servings = 2,
                        CookingInstructions = "blablalbalbalbl"
                    },

                    new Recipe
                    {
                        Id = 2,
                        Name = "Spaghetti",
                        Servings = 4,
                        CookingInstructions = "blablalblablalblablalb"
                    },

                    new Recipe
                    {
                        Id = 3,
                        Name = "Steak met frieten",
                        Servings = 4,
                        CookingInstructions = "bakken in lekkere bad"
                    }
                );

                context.SaveChanges();
            }

            if (!context.Ingredients.Any())
            {
                context.Ingredients.AddRange
                (
                    new Ingredient
                    {
                        Id = 1,
                        Name = "Bloem"
                    },

                    new Ingredient
                    {
                        Id = 2,
                        Name = "Suiker"
                    },

                    new Ingredient
                    {
                        Id = 3,
                        Name = "Gehakt"
                    },

                    new Ingredient
                    {
                        Id = 4,
                        Name = "Tomaat"
                    },

                    new Ingredient
                    {
                        Id = 5,
                        Name = "Aardappel"
                    },

                    new Ingredient
                    {
                        Id = 6,
                        Name = "Steak"
                    }
                );

                context.SaveChanges();
            }

            if (!context.RecipeIngredients.Any())
            {
                context.RecipeIngredients.AddRange
                (
                    new RecipeIngredient
                    {
                        Id = 1,
                        Quantity = 200,
                        Measurement = Measurement.Gram,
                        IngredientId = 1,
                        RecipeId = 1,
                    },

                    new RecipeIngredient
                    {
                        Id = 2,
                        Quantity = 50,
                        Measurement = Measurement.Gram,
                        IngredientId = 2,
                        RecipeId = 1,
                    },

                    new RecipeIngredient
                    {
                        Id = 3,
                        Quantity = 250,
                        Measurement = Measurement.Gram,
                        IngredientId = 3,
                        RecipeId = 2,
                    },

                    new RecipeIngredient
                    {
                        Id = 4,
                        Quantity = 100,
                        Measurement = Measurement.Gram,
                        IngredientId = 4,
                        RecipeId = 2,
                    },

                    new RecipeIngredient
                    {
                        Id = 5,
                        Quantity = 250,
                        Measurement = Measurement.Gram,
                        IngredientId = 5,
                        RecipeId = 3,
                    },

                    new RecipeIngredient
                    {
                        Id = 6,
                        Quantity = 500,
                        Measurement = Measurement.Gram,
                        IngredientId = 6,
                        RecipeId = 3,
                    }
                );

                context.SaveChanges();
            }

            if (!context.Shoppinglists.Any())
            {
                context.Shoppinglists.AddRange
                (
                    new Shoppinglist
                    {
                        Id = 1,
                        Name = "Test",
                    },

                    new Shoppinglist
                    {
                        Id = 2,
                        Name = "Alina",
                    }
                );

                context.SaveChanges();
            }

            if (!context.ShoppinglistIngredients.Any())
            {
                context.ShoppinglistIngredients.AddRange
                (
                    new ShoppinglistIngredient
                    {
                        Id = 1,
                        Quantity = 200,
                        Measurement = Measurement.Gram,
                        IngredientId = 1,
                        ShoppinglistId = 1
                    },

                    new ShoppinglistIngredient
                    {
                        Id = 2,
                        Quantity = 50,
                        Measurement = Measurement.Gram,
                        IngredientId = 2,
                        ShoppinglistId = 1
                    },

                    new ShoppinglistIngredient
                    {
                        Id = 3,
                        Quantity = 250,
                        Measurement = Measurement.Gram,
                        IngredientId = 3,
                        ShoppinglistId = 2
                    },

                    new ShoppinglistIngredient
                    {
                        Id = 4,
                        Quantity = 100,
                        Measurement = Measurement.Gram,
                        IngredientId = 4,
                        ShoppinglistId = 2
                    }
                );

                context.SaveChanges();
            }

            if (!context.ShoppinglistRecipes.Any())
            {
                context.ShoppinglistRecipes.AddRange
                (
                    new ShoppinglistRecipe
                    {
                        Id = 1,
                        ShoppinglistId = 1,
                        RecipeId = 1
                    },

                    new ShoppinglistRecipe
                    {
                        Id = 2,
                        ShoppinglistId = 2,
                        RecipeId = 2
                    }
                );

                context.SaveChanges();
            }
        }
    }
}
