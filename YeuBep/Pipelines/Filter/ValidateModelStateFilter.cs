using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace YeuBep.Pipelines.Filter;

public class ValidateModelStateFilter : IActionFilter
{
    private readonly ITempDataDictionaryFactory _tempDataFactory;

    public ValidateModelStateFilter(ITempDataDictionaryFactory tempDataFactory)
    {
        _tempDataFactory = tempDataFactory;
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            var allErrors = context.ModelState
                .SelectMany(x => x.Value?.Errors ?? [])
                .Select(e => e.ErrorMessage)
                .ToList();

            var tempData = _tempDataFactory.GetTempData(context.HttpContext);
            tempData["Error"] = string.Join("<br/>", allErrors);
        }
    }

    public void OnActionExecuted(ActionExecutedContext context) { }
}