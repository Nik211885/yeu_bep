using Microsoft.AspNetCore.Mvc;
using YeuBep.ViewModels;

namespace YeuBep.ViewComponents;

public class TableComponent : ViewComponent
{
    public IViewComponentResult Invoke(PaginationViewModel<object> model, List<ButtonConfig> buttonConfig)
    {
        ViewBag.ButtonConfig = buttonConfig;
        return View("~/Views/Shared/Components/Table.cshtml", model);
    }
}