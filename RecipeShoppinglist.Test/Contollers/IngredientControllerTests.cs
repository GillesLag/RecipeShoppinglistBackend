using System.Linq;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using RecipeShoppinglist.Controllers;
using RecipeShoppinglist.DTOs;
using RecipeShoppingList.Models;
using RecipeShoppingList.Repostories;
using RecipeShoppingList.Repostories.Interfaces;

namespace RecipeShoppinglist.Test.Contollers;

[TestFixture]
public class IngredientControllerTests
{
    private Mock<ILogger<IngredientController>> _mockLogger;
    private Mock<IUnitOfWork> _mockRepo;
    private IngredientController _controller;

    [SetUp]
    public void SetUp()
    {
        _mockLogger = new Mock<ILogger<IngredientController>>();
        _mockRepo = new Mock<IUnitOfWork>();
        _controller = new IngredientController(_mockLogger.Object, _mockRepo.Object);
    }

    [TearDown]
    public void TearDown()
    {
        _controller.Dispose();
    }

    [Test]
    public void GetAll_ReturnsOkActionResult_WithAllTheIngredients()
    {
        // Arrange
        var ingredients = new List<Ingredient>
        {
            new() { Id = 1, Name = "Salt" },
            new() { Id = 2, Name = "Sugar" }
        };

        _mockRepo.Setup(repo => repo.IngredientRepo.GetAll())
            .Returns(ingredients);

        // Act
        var result = _controller.GetAll();

        // Assert
        Assert.IsInstanceOf<OkObjectResult>(result.Result);

        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.That(ingredients, Is.EqualTo(okResult.Value));
    }

    [Test]
    public void GetAll_ReturnsNotFound_WhenIngredientsIsNull()
    {
        // Arrange
        _mockRepo.Setup(repo => repo.IngredientRepo.GetAll())
            .Returns((List<Ingredient>)null!);

        // Act
        var result = _controller.GetAll();

        // Assert
        Assert.IsInstanceOf<NotFoundResult>(result.Result);
    }

    [Test]
    public void GetById_ReturnsOkActionResult_WithOneIngredient([Values(1,2,3)] int id)
    {
        // Arrange
        var ingredients = new List<Ingredient>
        {
            new() { Id = 1, Name = "Salt" },
            new() { Id = 2, Name = "Sugar" },
            new() { Id = 3, Name = "Tomaat" }
        };

        _mockRepo.Setup(repo => repo.IngredientRepo.GetById(It.Is<int>(x => x == id)))
            .Returns((int x) => ingredients.First(i => i.Id == x));

        // Act
        var result = _controller.GetById(id);

        // Assert
        Assert.IsInstanceOf<OkObjectResult>(result.Result);

        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.That(id, Is.EqualTo(((Ingredient)okResult.Value!).Id));
    }

    [Test]
    public void GetById_ReturnsNotFound_WhenIdDoesNotExist()
    {
        // Arrange
        _mockRepo.Setup(repo => repo.IngredientRepo.GetById(It.IsAny<int>()))
            .Returns((Ingredient)null!);

        // Act
        var noneExistingId = 999;
        var result = _controller.GetById(noneExistingId);

        // Assert
        Assert.IsInstanceOf<NotFoundResult>(result.Result);
    }

    [Test]
    public void Create_ReturnsCreatedAtActionResult_WithTheIngredient()
    {
        // Arrange
        var ingredientDto = new CreateIngredientDto { Name = "Bloem" };
        var ingredient = new Ingredient { Id = 1, Name = "Bloem" };

        _mockRepo.Setup(repo => repo.IngredientRepo.Add(It.IsAny<Ingredient>()))
            .Callback<Ingredient>(i => i.Id = 1);

        // Act
        var result = _controller.Create(ingredientDto);

        // Assert
        Assert.IsInstanceOf<CreatedAtActionResult>(result.Result);

        var createdResult = result.Result as CreatedAtActionResult;
        Assert.IsNotNull(createdResult);
        Assert.AreEqual("GetById", createdResult.ActionName);
        Assert.AreEqual(ingredient.Id, ((Ingredient)createdResult.Value).Id);

        _mockRepo.Verify(repo => repo.IngredientRepo.Add(It.IsAny<Ingredient>()), Times.Once);
        _mockRepo.Verify(repo => repo.Save(), Times.Once);
    }
}