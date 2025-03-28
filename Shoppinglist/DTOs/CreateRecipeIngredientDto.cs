﻿using RecipeShoppingList.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace RecipeShoppinglist.DTOs;

public class CreateRecipeIngredientDto
{
    public int IngredientId { get; set; }
    public int Quantity { get; set; }
    public Measurement Measurement { get; set; }
    public CreateIngredientDto? Ingredient { get; set; }
}
