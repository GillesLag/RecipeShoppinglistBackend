using Microsoft.AspNetCore.Mvc;
using RecipeShoppinglist.DTOs;
using RecipeShoppingList.Models;
using RecipeShoppingList.Repostories;

namespace RecipeShoppinglist.Controllers;

[Route("[controller]/[action]")]
[ApiController]
public class IngredientController : Controller
{
    private readonly ILogger<IngredientController> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public IngredientController(ILogger<IngredientController> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Ingredient>> GetAll()
    {
        var ingredients = _unitOfWork.IngredientRepo.GetAll();

        if (ingredients is null)
        {
            return NotFound();
        }

        return Ok(ingredients);
    }

    [HttpGet]
    [Route("{id}")]
    public ActionResult<Ingredient> GetById(int id)
    {
        var ingredient = _unitOfWork.IngredientRepo.GetById(id);

        if (ingredient is null)
        {
            return NotFound();
        }

        return Ok(ingredient);
    }

    [HttpPost]
    public ActionResult<Ingredient> Create(CreateIngredientDto ingredientDto)
    {
        Ingredient ingredient = new Ingredient
        {
            Name = ingredientDto.Name,
        };

        _unitOfWork.IngredientRepo.Add(ingredient);
        _unitOfWork.Save();

        return CreatedAtAction(nameof(GetById), new {id = ingredient.Id}, ingredient);
    }
}
