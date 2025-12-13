using Microsoft.AspNetCore.Mvc;
using YeuBep.Queries;

namespace YeuBep.Controllers.Apis;
[ApiController]
[Route("api/categories")]
public class CategoriesApiController : ControllerBase
{
    private readonly ILogger<CategoriesApiController> _logger;
    private readonly CategoryQueries _categoryQueries;

    public CategoriesApiController(ILogger<CategoriesApiController> logger, CategoryQueries categoryQueries)
    {
        _logger = logger;
        _categoryQueries = categoryQueries;
    }

    [HttpGet("list")]
    public async Task<IActionResult> GetCategories()
    {
        var categories = await _categoryQueries.GetCategoriesAsync();
        return Ok(categories);
    }

    [HttpGet("recipe")]
    public async Task<IActionResult> GetByRecipeId(string recipeId)
    {
        var categories = await _categoryQueries.GetCategoryByRecipeAsync(recipeId);
        return Ok(categories);
    }
}