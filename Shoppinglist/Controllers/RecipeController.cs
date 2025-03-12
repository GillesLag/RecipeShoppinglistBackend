using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RecipeShoppinglist.DTOs;
using RecipeShoppingList.Models;
using RecipeShoppingList.Repostories;
using System.ComponentModel;

namespace RecipeShoppinglist.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class RecipeController : ControllerBase
    {
        private readonly ILogger<RecipeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public RecipeController(ILogger<RecipeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Recipe>> GetAll()
        {
            var recipes = _unitOfWork.RecipeRepo.GetAll();

            if (recipes is null)
            {
                return NotFound();
            }

            return Ok(recipes);
        }

        [HttpGet("{id}")]
        public ActionResult<Recipe> GetById(int id)
        {
            var recipe = _unitOfWork.RecipeRepo.GetById(id);

            if (recipe is null)
            {
                return NotFound();
            }

            return Ok(recipe);
        }

        [HttpPost]
       //[ValidateAntiForgeryToken]
        public ActionResult CreateRecipe([FromBody] CreateRecipeDto recipeDto)
        {
            var newRecipe = new Recipe()
            {
                CookingInstructions = recipeDto.CookingInstructions,
                Name = recipeDto.Name,
                Servings = recipeDto.Servings,
            };

            foreach (var recipeIngredient in recipeDto.RecipeIngredients)
            {
                var ingredient = _unitOfWork.IngredientRepo.GetById(recipeIngredient.IngredientId);

                if (ingredient is null)
                {
                    ingredient = new Ingredient()
                    {
                        Name = recipeIngredient.Ingredient?.Name,
                    };
                }

                var newRecipeIngredient = new RecipeIngredient()
                {
                    Ingredient = ingredient,
                    Recipe = newRecipe,
                    Quantity = recipeIngredient.Quantity,
                    Measurement = recipeIngredient.Measurement,
                    IngredientId = ingredient.Id
                };

                newRecipe.RecipeIngredients.Add(newRecipeIngredient);
            }

            _unitOfWork.RecipeRepo.Add(newRecipe);
            _unitOfWork.Save();

            return Ok();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var recipe = _unitOfWork.RecipeRepo.GetById(id);

            if (recipe is null)
            {
                return NotFound();
            }

            _unitOfWork.RecipeRepo.Delete(id);
            _unitOfWork.Save();

            return NoContent();
        }
    }
}
