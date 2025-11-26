using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YeuBep.Entities;

namespace YeuBep.Data.EntityConfigurations;

public class RatingEntityConfiguration : IEntityTypeConfiguration<Rating>
{
    public void Configure(EntityTypeBuilder<Rating> builder)
    {
        builder.ToTable("Ratings");
        builder.HasKey(x=>x.Id);
        builder.Property(x => x.RatingPoint).IsRequired();
        builder.Property(x => x.RecipeId)
            .IsRequired()
            .HasMaxLength(20);
        
        builder.HasOne(x=>x.Recipe)
            .WithMany(x=>x.Ratings)
            .HasForeignKey(x=>x.RecipeId);
        
        builder.HasOne(x=>x.CreatedBy)
            .WithMany()
            .HasForeignKey(x=>x.CreatedById);
        builder.HasOne(x=>x.ModifiedBy)
            .WithMany()    
            .HasForeignKey(x=>x.ModifiedById);
    }
}