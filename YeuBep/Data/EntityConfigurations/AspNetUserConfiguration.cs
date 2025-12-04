using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YeuBep.Entities;

namespace YeuBep.Data.EntityConfigurations;

public class AspNetUserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(u => u.Majors)
            .HasColumnType("jsonb")  
            .HasConversion(
                v => JsonSerializer.Serialize(v.ToList(), (JsonSerializerOptions?)null),
                v => JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions?)null)
                     ?? new List<string>()
            );
    }
}