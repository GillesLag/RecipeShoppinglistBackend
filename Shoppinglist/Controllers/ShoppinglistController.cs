using Microsoft.AspNetCore.Mvc;
using RecipeShoppinglist.DTOs;
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

        [HttpPost]
        public ActionResult<Shoppinglist> Add([FromBody]CreateShoppinglistDto shoppinglistDto)
        {
            if (shoppinglistDto is null)
            {
                return BadRequest();
            }

            var shoppinglist = new Shoppinglist()
            {
                Name = shoppinglistDto.Name
            };

            _unitOfWork.ShoppinglistRepo.Add(shoppinglist);
            _unitOfWork.Save();

            return CreatedAtAction(nameof(GetById), new { id = shoppinglist.Id }, shoppinglist);
        }

        [HttpPut("{id}")]
        public ActionResult<Shoppinglist> UpdateShoppinglist(int id, [FromBody]UpdateShoppinglistDto shoppinglistDto)
        {
            var shoppinglist = _unitOfWork.ShoppinglistRepo.GetById(id);

            if (shoppinglist is null)
            {
                return NotFound();
            }

            shoppinglist.Name = shoppinglistDto.Name;
            shoppinglist.ShoppinglistIngredients.Clear();

            //Add ingredients to shoppinglist
            foreach (var item in shoppinglistDto.ShoppinglistIngredients)
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
                }
                else
                {
                    shoppinglistIngredient.Quantity = item.Quantity;
                    shoppinglistIngredient.Measurement = item.Measurement;
                }

                shoppinglist.ShoppinglistIngredients.Add(shoppinglistIngredient);
            }

            _unitOfWork.Save();

            var test = CreatedAtAction(nameof(GetById), new { id = shoppinglist.Id }, shoppinglist);
            return test;
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var shoppinglist = _unitOfWork.ShoppinglistRepo.GetById(id);

            if (shoppinglist is null)
            {
                return NotFound();
            }

            _unitOfWork.ShoppinglistRepo.Delete(id);
            _unitOfWork.Save();

            return NoContent();
        }
    }
}
