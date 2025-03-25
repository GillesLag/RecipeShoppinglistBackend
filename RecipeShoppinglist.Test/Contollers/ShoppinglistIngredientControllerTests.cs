using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using RecipeShoppinglist.Controllers;
using RecipeShoppinglist.DTOs;
using RecipeShoppingList.Models;
using RecipeShoppingList.Models.Enums;
using RecipeShoppingList.Repostories;

namespace RecipeShoppinglist.Test;

[TestFixture]
public class ShoppinglistIngredientControllerTests
{
    private Mock<ILogger<ShoppinglistIngredientController>> _mockLogger;
    private Mock<IUnitOfWork> _mockRepo;
    private ShoppinglistIngredientController _controller;

    [SetUp]
    public void SetUp()
    {
        _mockLogger = new Mock<ILogger<ShoppinglistIngredientController>>();
        _mockRepo = new Mock<IUnitOfWork>();
        _controller = new ShoppinglistIngredientController(_mockLogger.Object, _mockRepo.Object);
    }

    [TearDown]
    public void TearDown()
    {
        _controller.Dispose();
    }

    [Test]
    public void Update_ReturnOkResult()
    {
        // Arrange
        var shoppinglistIngredient = new ShoppinglistIngredient()
        {
            Id = 1,
            IngredientId = 1,
            IsChecked = false,
            ShoppinglistId = 1,
            Quantity = 100,
            Measurement = Measurement.Gram,
        };

        var shoppinglistIngredientDto = new UpdateShoppinglistIngredientDto()
        {
            Id = 1,
            IngredientId = 1,
            ShoppinglistId = 1,
            Quantity = 150,
            Measurement = Measurement.Liter,
            IsChecked = true,
        };

        _mockRepo.Setup(repo => repo.ShoppinglistIngredientRepo.GetById(1))
            .Returns(shoppinglistIngredient);

        // Act
        var result = _controller.Update(1, shoppinglistIngredientDto);

        // Assert
        Assert.That(result, Is.InstanceOf<OkResult>());

        Assert.Multiple(() =>
        {
            Assert.That(shoppinglistIngredient.Measurement, Is.EqualTo(shoppinglistIngredientDto.Measurement));
            Assert.That(shoppinglistIngredient.Quantity, Is.EqualTo(shoppinglistIngredientDto.Quantity));
            Assert.That(shoppinglistIngredient.IsChecked, Is.EqualTo(shoppinglistIngredientDto.IsChecked));
        });

        _mockRepo.Verify(repo => repo.ShoppinglistIngredientRepo.GetById(1), Times.Once());
        _mockRepo.Verify(repo => repo.ShoppinglistIngredientRepo.Update(shoppinglistIngredient), Times.Once());
        _mockRepo.Verify(repo => repo.Save(), Times.Once());
    }
}
