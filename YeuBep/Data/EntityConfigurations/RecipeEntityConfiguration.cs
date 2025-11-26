using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YeuBep.Entities;

namespace YeuBep.Data.EntityConfigurations;

public class RecipeEntityConfiguration : IEntityTypeConfiguration<Recipe>
{
    public void Configure(EntityTypeBuilder<Recipe> builder)
    {
        builder.ToTable("Recipes");
        builder.HasKey(x=>x.Id);
        builder.Property(x => x.Avatar)
            .HasMaxLength(500)
            .IsUnicode();
        builder.Property(x=>x.Title)
            .HasMaxLength(200)
            .IsRequired();
        builder.Property(x => x.Description)
            .HasMaxLength(500);
        builder.Property(x => x.PortionCount)
            .HasMaxLength(50);
        builder.Property(x => x.TimeToCook)
            .HasMaxLength(50);
        builder.Property(x=>x.Slug)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(x => x.RecipeStatus)
            .IsRequired()
            .HasMaxLength(30)
            .HasConversion<string>();
        builder.OwnsMany(x => x.IngredientPart, y =>
        {
            y.ToJson();
            y.Property(x => x.Title);
            y.Property(x=>x.Ingredients);
        });
        
        builder.OwnsMany(x=>x.DetailInstructionSteps, y=>
        {
            y.ToJson();
            y.Property(x => x.ImageDescription);
            y.Property(x => x.Instructions);
        });
        
        builder.HasOne(x=>x.CreatedBy)
            .WithMany()
            .HasForeignKey(x=>x.CreatedById);
        builder.HasOne(x=>x.ModifiedBy)
            .WithMany()    
            .HasForeignKey(x=>x.ModifiedById);
        
        builder.HasMany(x=>x.Comments)
            .WithOne(x=>x.Recipe)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasMany(x=>x.Ratings)
            .WithOne(x=>x.Recipe)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasMany(x=>x.Favorites)
            .WithOne(x=>x.Recipe)
            .OnDelete(DeleteBehavior.Cascade);
        
    }
    
}