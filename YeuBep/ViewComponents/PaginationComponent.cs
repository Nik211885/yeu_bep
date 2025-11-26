using Microsoft.AspNetCore.Mvc;
using YeuBep.ViewModels;

namespace YeuBep.ViewComponents;

public class PaginationComponent : ViewComponent
{
    public IViewComponentResult Invoke(PaginationViewModel model)
    {
        return View("~/Views/Shared/Components/Pagination.cshtml", model);
    }
}
