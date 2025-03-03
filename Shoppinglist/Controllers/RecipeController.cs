using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        [HttpGet]
        public Recipe GetById()
        {
            return new Recipe();
        }
    }
}
