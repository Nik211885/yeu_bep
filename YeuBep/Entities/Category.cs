namespace YeuBep.Entities;

public class Category : AuditEntity
{
    public string Title { get; set; }
    public string Slug { get; set; }
    public string Description { get; set; }
    public string Avatar { get; set; }
    public bool IsActive { get; set; }
    public int CountRecipe { get; set; }
    public virtual ICollection<CategoriesRecipes> CategoriesRecipes { get; set; }
}