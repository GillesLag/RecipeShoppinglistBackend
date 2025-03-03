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

        [HttpPut("{id}")]
        //[ValidateAntiForgeryToken]
        public ActionResult AddRecipeToShoppinglist(int id, [FromBody]Shoppinglist shoppinglist)
        {
            var list = _unitOfWork.ShoppinglistRepo.GetById(id);

            if (list is null)
            {
                return NotFound();
            }

            list.Name = shoppinglist.Name;
            list.ShoppinglistIngredients.Clear();

            foreach (var item in shoppinglist.ShoppinglistIngredients)
            {
                var shoppinglistIngredient = _unitOfWork.ShoppinglistIngredientRepo.GetById(item.Id);

                if (shoppinglistIngredient is null)
                {
                    shoppinglistIngredient = new ShoppinglistIngredient()
                    {
                        ShoppinglistId = id,
                        IngredientId = item.IngredientId,
                        Quantity = item.Quantity,
                        Measurement = item.Measurement,
                    };

                    _unitOfWork.ShoppinglistIngredientRepo.Add(shoppinglistIngredient);
                }
                else
                {
                    shoppinglistIngredient.Quantity = item.Quantity;
                }

                list.ShoppinglistIngredients.Add(shoppinglistIngredient);
            }

            _unitOfWork.Save();

            return Ok();
        }
    }
}
