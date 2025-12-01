using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YeuBep.Entities;

namespace YeuBep.Data.EntityConfigurations;

public class NotificationEntityConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.ToTable("Notifications");
        builder.HasKey(x=>x.Id);
        builder.Property(x => x.Body)
            .IsRequired();
        builder.Property(x => x.Title)
            .IsRequired()
            .HasMaxLength(100);
        builder.Property(x => x.Link)
            .HasMaxLength(200);
        builder.HasOne(x=>x.CreatedBy)
            .WithMany()
            .HasForeignKey(x=>x.CreatedById);
        builder.HasOne(x=>x.ModifiedBy)
            .WithMany()    
            .HasForeignKey(x=>x.ModifiedById);
        builder.HasOne(x => x.SendForUser)
            .WithMany()
            .HasForeignKey(x => x.SendForUserId);
    }
}