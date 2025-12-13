using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YeuBep.Entities;

namespace YeuBep.Data.EntityConfigurations;

public class CategoriesRecipesEntityConfiguration : IEntityTypeConfiguration<CategoriesRecipes>
{
    public void Configure(EntityTypeBuilder<CategoriesRecipes> builder)
    {
        builder.HasKey(e => new { e.CategoryId, e.RecipeId });
    }
}