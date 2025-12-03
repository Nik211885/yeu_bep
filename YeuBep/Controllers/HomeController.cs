using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using YeuBep.Queries;
using YeuBep.ViewModels.Errors;

namespace YeuBep.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly RecipeQueries _recipeQueries;

        public HomeController(ILogger<HomeController> logger, RecipeQueries recipeQueries)
        {
            _logger = logger;
            _recipeQueries = recipeQueries;
        }
        public async Task<IActionResult> Index()
        {
            var recipe = await _recipeQueries.GetTopRecipePaginationAsync(1, 25);
            return View(recipe.Value.Items);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
