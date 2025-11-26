using YeuBep.ViewModels.Account;

namespace YeuBep.ViewModels.Comment;

public class CommentViewModel
{
    public string Id { get; set; }
    public string CreatedDate { get; set; }
    public string RecipeId { get; set; }
    public string CommentText { get; set; }
    public AccountInfo CreatedBy { get; set; }
}