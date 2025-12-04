using Microsoft.AspNetCore.Mvc;
using YeuBep.Queries;

namespace YeuBep.Controllers;

public class CategoryController : Controller
{
    private readonly ILogger<CategoryController> _logger;
    private readonly CategoryQueries _categoryQueries;

    public CategoryController(ILogger<CategoryController> logger, CategoryQueries categoryQueries)
    {
        _logger = logger;
        _categoryQueries = categoryQueries;
    }

    [HttpGet]
    public async Task<IActionResult> GetCategories()
    {
        return View("~/Views/Categorie/Category.cshtml");
    }

    [HttpGet]
    public async Task<IActionResult> GetTrending()
    {
        return View("~/Views/Categorie/Trending.cshtml");
    }

    [HttpGet]
    public async Task<IActionResult> GetChefs()
    {
        return View("~/Views/Categorie/Chef.cshtml");
    }
}