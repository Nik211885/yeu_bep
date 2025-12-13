using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YeuBep.Entities;

namespace YeuBep.Data.EntityConfigurations;

public class CategoryEntityConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("Categories");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Title).IsRequired()
            .HasMaxLength(100);
        builder.Property(x => x.Description).IsRequired();
        builder.Property(x=>x.Slug).IsRequired()
            .HasMaxLength(110);
        builder.Property(x => x.Avatar)
            .IsRequired().HasMaxLength(500);
        builder.HasOne(x=>x.CreatedBy)
            .WithMany()
            .HasForeignKey(x=>x.CreatedById);
        builder.HasOne(x=>x.ModifiedBy)
            .WithMany()    
            .HasForeignKey(x=>x.ModifiedById);
        builder.Property(x => x.CountRecipe)
            .HasDefaultValue(0);
        builder.HasMany(x=>x.CategoriesRecipes)
            .WithOne()
            .HasForeignKey(x=>x.CategoryId);
            
            
    }
}