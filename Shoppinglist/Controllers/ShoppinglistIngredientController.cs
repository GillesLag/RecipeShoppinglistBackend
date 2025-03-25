using Microsoft.AspNetCore.Mvc;
using RecipeShoppinglist.DTOs;
using RecipeShoppingList.Repostories;

namespace RecipeShoppinglist.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class ShoppinglistIngredientController : Controller
    {
        private readonly ILogger<ShoppinglistIngredientController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        public ShoppinglistIngredientController(ILogger<ShoppinglistIngredientController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        [HttpPut]
        [Route("{id}")]
        public ActionResult Update(int id, [FromBody]UpdateShoppinglistIngredientDto ingredientDto)
        {
            if (ingredientDto is null)
            {
                return BadRequest();
            }

            var ingredient = _unitOfWork.ShoppinglistIngredientRepo.GetById(id);

            if (ingredient == null)
            {
                return NotFound();
            }

            ingredient.Measurement = ingredientDto.Measurement;
            ingredient.IsChecked = ingredientDto.IsChecked;
            ingredient.Quantity = ingredientDto.Quantity;

            _unitOfWork.ShoppinglistIngredientRepo.Update(ingredient);
            _unitOfWork.Save();

            return Ok();
        }
    }
}
