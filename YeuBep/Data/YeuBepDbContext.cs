using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using YeuBep.Entities;

namespace YeuBep.Data;

public class YeuBepDbContext
    : IdentityDbContext<User, IdentityRole, string>
{
    public YeuBepDbContext(DbContextOptions<YeuBepDbContext> options) : base(options)
    {
        
    }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Favorite> Favorites { get; set; }
    public DbSet<Recipe> Recipes { get; set; }
    public DbSet<Rating> Ratings { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<CategoriesRecipes> CategoriesRecipes { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(typeof(YeuBepDbContext).Assembly);
        base.OnModelCreating(builder);
    }
}