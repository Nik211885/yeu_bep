using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace YeuBep.Data;

public class YeBepDbContextDesignTime : IDesignTimeDbContextFactory<YeuBepDbContext>
{
    public YeuBepDbContext CreateDbContext(string[] args)
    {
        var basePath = Path.Combine(Directory.GetCurrentDirectory());
        var configuration = new ConfigurationBuilder()
            .SetBasePath(basePath)
            .AddJsonFile("appsettings.json", optional: false)
            .Build();
        var connectionString = configuration.GetConnectionString("Postgres");
        var optionsBuilder = new DbContextOptionsBuilder<YeuBepDbContext>();
        optionsBuilder.UseNpgsql(connectionString);
        return new YeuBepDbContext(optionsBuilder.Options);
    }
}