using Microsoft.AspNetCore.Mvc;
using YeuBep.ViewModels.Recipe;

namespace YeuBep.ViewComponents;

public class RecipeCardSearchComponent : ViewComponent
{
    public IViewComponentResult Invoke(RecipeViewModel model)
    {
        return View("~/Views/Shared/Components/RecipeCardSearch.cshtml",model);
    }
}