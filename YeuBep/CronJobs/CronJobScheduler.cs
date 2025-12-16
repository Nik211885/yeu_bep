using Hangfire;

namespace YeuBep.CronJobs;

public static class CronJobScheduler
{
    public static void RegisterRecurringJobs()
    {
        RecurringJob.AddOrUpdate<SyncRecipeToMilvusCronJob>(
            "reindex-every-1-hour",
            job => job.Run(),
            Cron.Hourly
        );
        RecurringJob.AddOrUpdate<DeleteRecipeFromMilvusCronJob>(
            "reindex-every-1-hour",
            job => job.Run(),
            Cron.Hourly
        );
    }
}