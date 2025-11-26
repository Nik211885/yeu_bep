namespace YeuBep.Entities;

public class Favorite : AuditEntity
{
    public string RecipeId { get; set; }
    
    public virtual Recipe Recipe { get; set; }
}