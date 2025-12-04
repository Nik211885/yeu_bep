namespace YeuBep.Queries;

public static class AddQueriesDefaultServicesExtension
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddQueriesServicesDefault()
        {
            services.AddScoped<RecipeQueries>();
            services.AddScoped<FavoritesQueries>();
            services.AddScoped<RatingQueries>();
            services.AddScoped<NotificationQueries>();
            services.AddScoped<CategoryQueries>();
            return services;
        }
    }
}