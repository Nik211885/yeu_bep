using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YeuBep.Entities;

namespace YeuBep.Data.EntityConfigurations;

public class FavoriteEntityConfiguration : IEntityTypeConfiguration<Favorite>
{
    public void Configure(EntityTypeBuilder<Favorite> builder)
    {
        builder.ToTable("Favorites");
        builder.HasKey(k => k.Id);
        builder.HasOne(x=>x.Recipe)
            .WithMany(x=>x.Favorites)
            .HasForeignKey(x=>x.RecipeId);
        builder.HasOne(x=>x.CreatedBy)
            .WithMany()
            .HasForeignKey(x=>x.CreatedById);
        builder.HasOne(x=>x.ModifiedBy)
            .WithMany()    
            .HasForeignKey(x=>x.ModifiedById);
    }
}