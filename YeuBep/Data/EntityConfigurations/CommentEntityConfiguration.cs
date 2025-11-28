using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YeuBep.Entities;

namespace YeuBep.Data.EntityConfigurations;

public class CommentEntityConfiguration : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder.ToTable("Comments");
        builder.HasKey(k => k.Id);
        builder.Property(x => x.CommentText)
            .IsRequired()
            .HasMaxLength(800);
        builder.HasOne(x=>x.Recipe)
            .WithMany(x=>x.Comments)
            .HasForeignKey(x=>x.RecipeId);
        
        builder.HasOne(x=>x.CreatedBy)
            .WithMany()
            .HasForeignKey(x=>x.CreatedById);
        builder.HasOne(x=>x.ModifiedBy)
            .WithMany()    
            .HasForeignKey(x=>x.ModifiedById);
    }
}