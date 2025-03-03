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
        //[ValidateAntiForgeryToken]
        public ActionResult AddRecipeToShoppinglist([FromBody]Shoppinglist shoppinglist)
        {
            var list = _unitOfWork.ShoppinglistRepo.GetById(shoppinglist.Id);

            if (list is null)
            {
                return NotFound();
            }

            _unitOfWork.ShoppinglistRepo.Update(shoppinglist);
            _unitOfWork.Save();

            return Ok();
        }
    }
}
