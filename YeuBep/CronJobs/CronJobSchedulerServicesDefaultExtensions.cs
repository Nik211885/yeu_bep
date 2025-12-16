namespace YeuBep.CronJobs;

public static class CronJobSchedulerServicesDefaultExtensions
{
    public static IServiceCollection AddCronJobSchedulerServicesDefault(this IServiceCollection services)
    {
        services.AddScoped<DeleteRecipeFromMilvusCronJob>();
        services.AddScoped<SyncRecipeToMilvusCronJob>();
        return services;
    }
}