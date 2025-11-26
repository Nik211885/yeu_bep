using Microsoft.AspNetCore.Mvc;
using YeuBep.ViewModels.Recipe;

namespace YeuBep.ViewComponents;

public class RecipeComponent : ViewComponent
{
    public IViewComponentResult Invoke(RecipeViewModel model)
    {
        return View("~/Views/Shared/Components/RecipeCard.cshtml", model);
    }
}