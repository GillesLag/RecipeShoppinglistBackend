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
        public IEnumerable<Recipe> GetAll()
        {
            return _unitOfWork.RecipeRepo.GetAll();
        }

        [HttpGet]
        public Recipe GetById()
        {
            return new Recipe();
        }
    }
}
