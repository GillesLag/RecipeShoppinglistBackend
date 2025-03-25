using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using RecipeShoppinglist.Controllers;
using RecipeShoppinglist.DTOs;
using RecipeShoppingList.Models;
using RecipeShoppingList.Repostories;

namespace RecipeShoppinglist.Test;

[TestFixture]
public class ShoppinglistControllerTests
{
    private Mock<ILogger<ShoppinglistController>> _mockLogger;
    private Mock<IUnitOfWork> _mockRepo;
    private ShoppinglistController _controller;

    [SetUp]
    public void SetUp()
    {
        _mockLogger = new Mock<ILogger<ShoppinglistController>>();
        _mockRepo = new Mock<IUnitOfWork>();
        _controller = new ShoppinglistController(_mockLogger.Object, _mockRepo.Object);
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
        var shoppinglists = new List<Shoppinglist>
        {
            new() { Id = 1, Name = "Kerst" },
            new() { Id = 2, Name = "Pasen" }
        };

        _mockRepo.Setup(repo => repo.ShoppinglistRepo.GetAll())
            .Returns(shoppinglists);

        // Act
        var result = _controller.GetAll();

        // Assert
        Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());

        var okResult = result.Result as OkObjectResult;
        Assert.That(okResult, Is.Not.Null);
        Assert.That(shoppinglists, Is.EqualTo(okResult.Value));
    }

    [Test]
    public void GetAll_ReturnsNotFound_WhenIngredientsIsNull()
    {
        // Arrange
        _mockRepo.Setup(repo => repo.ShoppinglistRepo.GetAll())
            .Returns((List<Shoppinglist>)null!);

        // Act
        var result = _controller.GetAll();

        // Assert
        Assert.That(result.Result, Is.InstanceOf<NotFoundResult>());
    }

    [Test]
    public void GetById_ReturnsOkActionResult_WithOneIngredient([Values(1, 2, 3)] int id)
    {
        // Arrange
        var shoppinglists = new List<Shoppinglist>
        {
            new() { Id = 1, Name = "Kerst" },
            new() { Id = 2, Name = "Pasen" },
            new() { Id = 3, Name = "valentijn" }
        };

        _mockRepo.Setup(repo => repo.ShoppinglistRepo.GetById(It.Is<int>(x => x == id)))
            .Returns((int x) => shoppinglists.First(i => i.Id == x));

        // Act
        var result = _controller.GetById(id);

        // Assert
        Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());

        var okResult = result.Result as OkObjectResult;
        Assert.That(okResult, Is.Not.Null);
        Assert.That(id, Is.EqualTo(((Shoppinglist)okResult.Value!).Id));
    }

    [Test]
    public void GetById_ReturnsNotFound_WhenIdDoesNotExist()
    {
        // Arrange
        _mockRepo.Setup(repo => repo.ShoppinglistRepo.GetById(It.IsAny<int>()))
            .Returns((Shoppinglist)null!);

        // Act
        var noneExistingId = 999;
        var result = _controller.GetById(noneExistingId);

        // Assert
        Assert.That(result.Result, Is.InstanceOf<NotFoundResult>());
    }

    [Test]
    public void Create_ReturnsCreatedAtActionResult_WithTheIngredient()
    {
        // Arrange
        var shoppinglistDto = new CreateShoppinglistDto { Name = "Kerst" };
        var shoppinglist = new Shoppinglist { Id = 1, Name = "Kerst" };

        _mockRepo.Setup(repo => repo.ShoppinglistRepo.Add(It.IsAny<Shoppinglist>()))
            .Callback<Shoppinglist>(i => i.Id = 1);

        // Act
        var result = _controller.Add(shoppinglistDto);

        // Assert
        Assert.That(result.Result, Is.InstanceOf<CreatedAtActionResult>());

        var createdResult = result.Result as CreatedAtActionResult;
        Assert.That(createdResult, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(createdResult.ActionName, Is.EqualTo("GetById"));
            Assert.That(((Shoppinglist)createdResult.Value!).Id, Is.EqualTo(shoppinglist.Id));
        });

        _mockRepo.Verify(repo => repo.ShoppinglistRepo.Add(It.IsAny<Shoppinglist>()), Times.Once);
        _mockRepo.Verify(repo => repo.Save(), Times.Once);
    }
}
