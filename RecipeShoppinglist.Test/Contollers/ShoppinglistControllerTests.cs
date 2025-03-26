using Microsoft.AspNetCore.Http.HttpResults;
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

    [Test]
    public void Create_ReturnsBadRequest_WithWrongPayload()
    {
        // Arrange


        // Act
        var result = _controller.Add(null!);


        // Assert
        Assert.That(result.Result, Is.InstanceOf<BadRequestResult>());
    }

    [Test]
    public void Update_ReturnsCreatedAtActionResult_WithTheShoppinglist()
    {
        // Arrange
        var shoppinglist = new Shoppinglist()
        {
            Id = 1,
            Name = "Kerst",
            ShoppinglistIngredients = new List<ShoppinglistIngredient>()
            {
                new() { Id = 1, IngredientId = 1, IsChecked = false, Measurement = Measurement.Pieces, Quantity = 2, ShoppinglistId = 1 },
                new() { Id = 2, IngredientId = 2, IsChecked = false, Measurement = Measurement.Pieces, Quantity = 2, ShoppinglistId = 1 },
            }
        };

        var shoppinglistDto = new UpdateShoppinglistDto()
        {
            Id = 1,
            Name = "Pasen",
            ShoppinglistIngredients = new List<UpdateShoppinglistIngredientDto>()
            {
                new() { Id = 1, IngredientId = 1, IsChecked = true, Measurement = Measurement.Gram, Quantity = 50, ShoppinglistId = 1 },
                new() { Id = 0, IngredientId = 3, IsChecked = false, Measurement = Measurement.Pieces, Quantity = 2, ShoppinglistId = 1 },
            }
        };

        var shoppinglistIngredientOne = new ShoppinglistIngredient() { Id = 1, IngredientId = 1, IsChecked = false, Measurement = Measurement.Pieces, Quantity = 2, ShoppinglistId = 1 };
        var shoppinglistIngredientTwo = new ShoppinglistIngredient() { Id = 0, IngredientId = 3, IsChecked = false, Measurement = Measurement.Pieces, Quantity = 2, ShoppinglistId = 1 };

        _mockRepo.Setup(repo => repo.ShoppinglistRepo.GetById(1))
            .Returns(shoppinglist);

        _mockRepo.Setup(repo => repo.ShoppinglistIngredientRepo.GetById(1))
            .Returns(shoppinglistIngredientOne);

        _mockRepo.Setup(repo => repo.ShoppinglistIngredientRepo.GetById(0))
            .Returns((ShoppinglistIngredient)null!);

        // Act
        var result = _controller.UpdateShoppinglist(1, shoppinglistDto);

        // Assert
        Assert.That(result.Result, Is.InstanceOf<CreatedAtActionResult>());

        var createdResult = result.Result as CreatedAtActionResult;
        Assert.That(createdResult, Is.Not.Null);

        var value = createdResult.Value as Shoppinglist;
        Assert.That(value, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(value.Name, Is.EqualTo("Pasen"));

            var shoppinglistIngredients = value.ShoppinglistIngredients;
            Assert.That(shoppinglistIngredients.ElementAt(0).Quantity, Is.EqualTo(50));
            Assert.That(shoppinglistIngredients.ElementAt(0).Measurement, Is.EqualTo(Measurement.Gram));
            Assert.That(shoppinglistIngredients.ElementAt(0).IngredientId, Is.EqualTo(1));

            Assert.That(shoppinglistIngredients.ElementAt(1).Quantity, Is.EqualTo(2));
            Assert.That(shoppinglistIngredients.ElementAt(1).Measurement, Is.EqualTo(Measurement.Pieces));
            Assert.That(shoppinglistIngredients.ElementAt(1).IngredientId, Is.EqualTo(3));
        });
    }

    [Test]
    public void Update_ReturnsNotFound_WhenIdIsWrong()
    {
        // Arrange
        var shoppinglistDto = new UpdateShoppinglistDto()
        {
            Id = 99,
            Name = "Kerst",
        };

        _mockRepo.Setup(repo => repo.ShoppinglistRepo.GetById(99))
            .Returns((Shoppinglist)null!);

        // Act
        var result = _controller.UpdateShoppinglist(99, shoppinglistDto);

        // Assert
        Assert.That(result.Result, Is.InstanceOf<NotFoundResult>());

    }

    [Test]
    public void Delete_ReturnsNoContent()
    {
        // Arrange
        var shoppinglist = new Shoppinglist() { Id = 1, Name = "Kerst" };

        _mockRepo.Setup(repo => repo.ShoppinglistRepo.GetById(1))
            .Returns(shoppinglist);

        // Act
        var result = _controller.Delete(1);

        // Assert
        Assert.That(result, Is.InstanceOf<NoContentResult>());

        _mockRepo.Verify(repo => repo.ShoppinglistRepo.GetById(1), Times.Once);
        _mockRepo.Verify(repo => repo.ShoppinglistRepo.Delete(1), Times.Once);
        _mockRepo.Verify(repo => repo.Save(), Times.Once);
    }

    [Test]
    public void Delete_ReturnsNotFound_WhenIdIsWrong()
    {
        // Arrange
        var shoppinglist = new Shoppinglist() { Id = 99, Name = "Kerst" };

        _mockRepo.Setup(repo => repo.ShoppinglistRepo.GetById(99))
            .Returns((Shoppinglist)null!);

        // Act
        var result = _controller.Delete(1);

        // Assert
        Assert.That(result, Is.InstanceOf<NotFoundResult>());

        _mockRepo.Verify(repo => repo.ShoppinglistRepo.GetById(1), Times.Once);
    }
}
