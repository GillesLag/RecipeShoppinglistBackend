using Microsoft.AspNetCore.Mvc;
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
}
