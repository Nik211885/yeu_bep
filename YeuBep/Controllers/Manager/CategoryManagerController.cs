using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YeuBep.Entities;
using YeuBep.Queries;
using YeuBep.Services;
using YeuBep.ViewModels;
using YeuBep.ViewModels.Category;

namespace YeuBep.Controllers.Manager;

[Authorize(Roles = nameof(Role.Admin))]
public class CategoryManagerController : Controller
{
    private readonly ILogger<CategoryManagerController> _logger;
    private readonly CategoryQueries _categoryQueries;
    private readonly CategoryServices _CategoryServices;

    public CategoryManagerController(ILogger<CategoryManagerController> logger, CategoryQueries categoryQueries, CategoryServices categoryServices)
    {
        _logger = logger;
        _categoryQueries = categoryQueries;
        _CategoryServices = categoryServices;
    }
    [HttpGet]
    public async Task<IActionResult> Categories(int pageNumber = 1, int pageSize = 10,
        Dictionary<string, string>? filterEqualTableViewModel = null)
    {
        var categories =
            await _categoryQueries.GetCategoryPaginationAsync(pageNumber, pageSize, filterEqualTableViewModel);
        return View("~/Views/Categorie/CategoryManager.cshtml", categories.CastToObjectType());
    }

    [HttpGet]
    public IActionResult CreateCategory()
    {
        return View("~/Views/Categorie/CategoryForm.cshtml");
    }

    [HttpPost]
    public async Task<IActionResult> CreateCategory(CreateCategoryViewModel model)
    {
        var category = await _CategoryServices.CreateCategory(model);
        ViewData["Success"] = "Xử lý thành công";
        return RedirectToAction(nameof(UpdateCategory), new { id = category.Value.Id });
    }
    [HttpGet]
    public async Task<IActionResult> UpdateCategory(string id)
    {
        var categories = await _categoryQueries.GetCategoryByIdAsync(id);
        var categoriesMapping =  categories.Adapt<CreateCategoryViewModel>();
        return View("~/Views/Categorie/CategoryForm.cshtml",categoriesMapping);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateCategory(string id, CreateCategoryViewModel model)
    {
        var category = await _CategoryServices.UpdateCategory(id, model);
        if (category.IsFailed)
        {
            return Redirect($"/Error/NotFoundPage");
        }
        var categoriesMapping = category.Value.Adapt<CreateCategoryViewModel>();
        return View("~/Views/Categorie/CategoryForm.cshtml",categoriesMapping); 
    }
}