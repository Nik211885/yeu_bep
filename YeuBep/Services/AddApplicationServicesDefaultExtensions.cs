namespace YeuBep.Services;

public  static class AddApplicationServicesDefaultExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddApplicationServicesDefault()
        {
            services.AddScoped<RecipeServices>();
            services.AddScoped<RatingServices>();
            services.AddScoped<FavoriteServices>();
            services.AddScoped<CommentServices>();
            return services;
        }
    }
}