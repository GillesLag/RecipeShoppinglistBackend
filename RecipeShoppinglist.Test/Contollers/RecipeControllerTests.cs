using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using RecipeShoppinglist.Controllers;
using RecipeShoppinglist.DTOs;
using RecipeShoppingList.Models;
using RecipeShoppingList.Models.Enums;
using RecipeShoppingList.Repostories;

namespace RecipeShoppinglist.Test.Contollers;

[TestFixture]
public class RecipeControllerTests
{
    private Mock<ILogger<RecipeController>> _mockLogger;
    private Mock<IUnitOfWork> _mockRepo;
    private RecipeController _controller;

    [SetUp]
    public void SetUp()
    {
        _mockLogger = new Mock<ILogger<RecipeController>>();
        _mockRepo = new Mock<IUnitOfWork>();
        _controller = new RecipeController(_mockLogger.Object, _mockRepo.Object);
    }

    [TearDown]
    public void TearDown()
    {
        _controller.Dispose();
    }

    [Test]
    public void GetAll_ReturnsOkActionResult_WithAllTheRecipes()
    {
        // Arrange
        var recipes = new List<Recipe>()
        {
            new Recipe() { Id = 1, Name = "Spaghetti", Servings = 4 },
            new Recipe() { Id = 2, Name = "Pannekoeken", Servings = 4 }
        };

        _mockRepo.Setup(repo => repo.RecipeRepo.GetAll())
            .Returns(recipes);

        // Act
        var result = _controller.GetAll();

        // Assert
        Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());

        var okResult = result.Result as OkObjectResult;
        Assert.That(okResult, Is.Not.Null);
        Assert.That(recipes, Is.EqualTo(okResult.Value));
    }

    [Test]
    public void GetAll_ReturnsNotFound_WhenRecipesIsNull()
    {
        // Arrange
        _mockRepo.Setup(repo => repo.RecipeRepo.GetAll())
            .Returns((List<Recipe>)null!);

        // Act
        var result = _controller.GetAll();

        // Assert
        Assert.That(result.Result, Is.InstanceOf<NotFoundResult>());
    }

    [Test]
    public void GetById_ReturnsOkActionResult_WithOneRecipe([Values(1, 2, 3)] int id)
    {
        // Arrange
        var recipes = new List<Recipe>
        {
            new() { Id = 1, Name = "Spaghetti", Servings = 4 },
            new() { Id = 2, Name = "Pannekoeken", Servings = 4 },
            new() { Id = 3, Name = "Lasagne", Servings = 4 }
        };

        _mockRepo.Setup(repo => repo.RecipeRepo.GetById(It.Is<int>(x => x == id)))
            .Returns((int x) => recipes.FirstOrDefault(i => i.Id == x));

        // Act
        var result = _controller.GetById(id);

        // Assert
        Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());

        var okResult = result.Result as OkObjectResult;
        Assert.That(okResult, Is.Not.Null);
        Assert.That(id, Is.EqualTo(((Recipe)okResult.Value!).Id));
    }

    [Test]
    public void GetById_ReturnsNotFound_WhenIdDoesNotExist()
    {
        // Arrange
        _mockRepo.Setup(repo => repo.RecipeRepo.GetById(It.IsAny<int>()))
            .Returns((Recipe)null!);

        // Act
        var noneExistingId = 999;
        var result = _controller.GetById(noneExistingId);

        // Assert
        Assert.That(result.Result, Is.InstanceOf<NotFoundResult>());
    }

    [Test]
    public void Create_ReturnsOkResult()
    {
        // Arrange
        var recipeDto = new CreateRecipeDto
        {
            Name = "Steak",
            Servings = 4,
            CookingInstructions = ["Cook on medium heat."],
            RecipeIngredients = new List<CreateRecipeIngredientDto>
            {
                new() { IngredientId = 1, Quantity = 2, Measurement = Measurement.Liter, Ingredient = null },
                new() { IngredientId = 0, Quantity = 1, Measurement = Measurement.Pieces, Ingredient = new CreateIngredientDto { Name = "Garlic" } }
            }
        };

        var existingIngredient = new Ingredient { Id = 1, Name = "Salt" };
        var newIngredient = new Ingredient { Id = 2, Name = "Garlic" };

        _mockRepo.Setup(repo => repo.IngredientRepo.GetById(1))
            .Returns(existingIngredient);

        _mockRepo.Setup(repo => repo.IngredientRepo.GetById(0))
            .Returns((Ingredient)null!);

        _mockRepo.Setup(repo => repo.RecipeRepo.Add(It.IsAny<Recipe>()))
            .Callback<Recipe>(r =>
            {
                r.Id = 10;
                r.RecipeIngredients.ElementAt(0).Ingredient = existingIngredient;
                r.RecipeIngredients.ElementAt(1).Ingredient = newIngredient;

            });

        // Act
        var result = _controller.CreateRecipe(recipeDto);

        // Assert
        Assert.That(result, Is.InstanceOf<OkResult>());
        _mockRepo.Verify(repo => repo.IngredientRepo.GetById(1), Times.Once);
        _mockRepo.Verify(repo => repo.IngredientRepo.GetById(0), Times.Once);
        _mockRepo.Verify(repo => repo.RecipeRepo.Add(It.IsAny<Recipe>()), Times.Once);
        _mockRepo.Verify(repo => repo.Save(), Times.Once);
    }

    [Test]
    public void Delete_ReturnNoContentResult()
    {
        // Arrange
        var recipe = new Recipe() { Id = 1, Name = "Spaghetti", Servings = 4 };

        _mockRepo.Setup(repo => repo.RecipeRepo.GetById(1))
            .Returns(recipe);

        // Act
        var result = _controller.Delete(1);

        // Assert
        Assert.That(result, Is.InstanceOf<NoContentResult>());

        _mockRepo.Verify(repo => repo.RecipeRepo.GetById(1), Times.Once);
        _mockRepo.Verify(repo => repo.RecipeRepo.Delete(1), Times.Once);
        _mockRepo.Verify(repo => repo.Save(), Times.Once);
    }
}

