using Microsoft.AspNetCore.Mvc;
using RecipeShoppingList.Models;
using RecipeShoppingList.Repostories;

namespace RecipeShoppinglist.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class ShoppinglistController : Controller
    {
        private readonly ILogger<ShoppinglistController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        public ShoppinglistController(ILogger<ShoppinglistController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Shoppinglist>> GetAll()
        {
            var shoppinglists =  _unitOfWork.ShoppinglistRepo.GetAll();

            if (shoppinglists == null)
            {
                return NotFound();
            }

            return Ok(shoppinglists);
        }

        [HttpGet("{id}")]
        public ActionResult<Shoppinglist> GetById(int id)
        {
            var shoppinglist = _unitOfWork.ShoppinglistRepo.GetById(id);
            if (shoppinglist == null)
            {
                return NotFound();
            }

            return Ok(shoppinglist);
        }

        [HttpPut]
        [ValidateAntiForgeryToken]
        public ActionResult AddRecipeToShoppinglist(int shoppinglistId, int recipeId)
        {
            var shoppinglist = _unitOfWork.ShoppinglistRepo.GetById(shoppinglistId);
            var recipe = _unitOfWork.RecipeRepo.GetById(recipeId);

            if (shoppinglist == null || recipe == null)
            {
                return NotFound();
            }

            foreach (var item in recipe.RecipeIngredients)
            {
                var shoppinglistIngredient = shoppinglist.ShoppinglistIngredients.SingleOrDefault(x => x.IngredientId == item.IngredientId);
                if (shoppinglistIngredient is not null)
                {
                    shoppinglistIngredient.Quantity += item.Quantity;
                }
                else
                {
                    var newItem = new ShoppinglistIngredient()
                    {
                        ShoppinglistId = shoppinglistId,
                        IngredientId = item.IngredientId,
                        Quantity = item.Quantity,
                        Measurement = item.Measurement,
                    };

                    shoppinglist.ShoppinglistIngredients.Add(newItem);
                }
            }

            _unitOfWork.Save();
            return Ok();
        }
    }
}
