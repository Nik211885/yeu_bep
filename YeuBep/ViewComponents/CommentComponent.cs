using Microsoft.AspNetCore.Mvc;
using YeuBep.ViewModels.Comment;

namespace YeuBep.ViewComponents;

public class CommentComponent : ViewComponent
{
    public IViewComponentResult Invoke(CommentViewModel comment)
    {
        return View("~/Views/Shared/Components/Comment.cshtml", comment);
    }
}