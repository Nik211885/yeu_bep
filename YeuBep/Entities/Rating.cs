namespace YeuBep.Entities;

public class Rating : AuditEntity
{
    public string RecipeId { get; set; }
    public int RatingPoint { get; set; }
    
    public virtual Recipe Recipe { get; set; }
}