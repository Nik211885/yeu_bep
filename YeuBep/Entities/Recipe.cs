namespace YeuBep.Entities;

public class Recipe : AuditEntity
{   
    public string Avatar { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string PortionCount { get; set; }
    public string Slug { get; set; }
    public string TimeToCook { get; set; }
    public ICollection<IngredientPart> IngredientPart { get; set; }
    public ICollection<DetailInstructionStep> DetailInstructionSteps { get; set; }
    public int CountFavorite { get; set; }
    public int CountRatingPoint {get; set;}
    public int TotalRatingPoint { get; set; }
    public RecipeStatus RecipeStatus { get; set; }
    public virtual ICollection<Comment> Comments { get; set; }
    public virtual ICollection<Favorite> Favorites { get; set; }
    public virtual ICollection<Rating> Ratings { get; set; }
}

public class IngredientPart
{
    public string Title { get; set; }
    public ICollection<string> Ingredients { get; set; }
}

public class DetailInstructionStep
{
    public string Instructions { get; set; }
    public string ImageDescription { get; set; }
}