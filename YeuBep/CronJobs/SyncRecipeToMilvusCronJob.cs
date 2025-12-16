using Hangfire;
using Microsoft.EntityFrameworkCore;
using Milvus.Client;
using YeuBep.Data;
using YeuBep.Entities;

namespace YeuBep.CronJobs;

[DisableConcurrentExecution(3600)]
public class SyncRecipeToMilvusCronJob
{
    private readonly ILogger<SyncRecipeToMilvusCronJob> _logger;
    private readonly YeuBepDbContext _dbContext;
    private readonly MilvusClient _milvusClient;
    public SyncRecipeToMilvusCronJob(ILogger<SyncRecipeToMilvusCronJob> logger, YeuBepDbContext dbContext, MilvusClient milvusClient)
    {
        _logger = logger;
        _dbContext = dbContext;
        _milvusClient = milvusClient;
    }

    public async Task Run()
    {
        var recipeToSync = await _dbContext.Recipes
            .Where(x => x.RecipeStatus == RecipeStatus.Accept)
            .Where(x => !x.IsSyncToMilvus)
            .ToListAsync();
        
    }
}