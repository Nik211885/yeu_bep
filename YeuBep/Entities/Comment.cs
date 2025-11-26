namespace YeuBep.Entities;

public class Comment : AuditEntity
{
    public string RecipeId { get; set; }
    public string CommentText { get; set; }
    
    public virtual Recipe Recipe { get; set; }
}