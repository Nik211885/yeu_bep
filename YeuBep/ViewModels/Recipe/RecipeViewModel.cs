using YeuBep.Attributes.Table;
using YeuBep.Entities;
using YeuBep.ViewModels.Account;
using YeuBep.ViewModels.Comment;

namespace YeuBep.ViewModels.Recipe;

public class RecipeViewModel
{
    [KeyTable]
    [NameColumn("recipeId")]
    public string Id { get; set; }
    [NameColumn("Tiêu đề")]
    public string Title { get; set; }
    [NameColumn("Mô tả")]
    public string Description { get; set; }
    [IgnoreColumn]
    public string PortionCount { get; set; }
    [IgnoreColumn]
    public string Slug { get; set; }
    [IgnoreColumn]
    public string TimeToCook { get; set; }
    [IgnoreColumn]
    public int CountFavorite { get; set; }
    [IgnoreColumn]
    public int CountRatingPoint {get; set;}
    [IgnoreColumn]
    public int TotalRatingPoint { get; set; }
    [NameColumn("Ngày tạo")]
    public DateTimeOffset CreatedDate { get; set; }
    public AccountInfo CreatedBy { get; set; }
    [IgnoreColumn]
    public string Avatar { get; set; }
    [IgnoreColumn]
    public ICollection<IngredientPart> IngredientPart { get; set; }
    [IgnoreColumn]
    public ICollection<CommentViewModel> Comments { get; set; }
    [IgnoreColumn]
    public ICollection<DetailInstructionStep> DetailInstructionSteps { get; set; }
    [NameColumn("Trạng thái")]
    public RecipeStatus RecipeStatus { get; set; }
    [IgnoreColumn]
    public long Views {get; set;}
    
}