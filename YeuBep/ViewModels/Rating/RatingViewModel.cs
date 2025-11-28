using YeuBep.ViewModels.Account;

namespace YeuBep.ViewModels.Rating;

public class RatingViewModel
{
    public AccountInfo CreatedBy { get; }
    public DateTimeOffset CreateDate { get; }
    public DateTimeOffset UpdateDate { get; }
    public string RecipeId { get; set; }
    public int RatingPoint { get; set; }
}