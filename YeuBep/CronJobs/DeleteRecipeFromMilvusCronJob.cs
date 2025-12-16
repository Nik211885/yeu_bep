using Milvus.Client;
using YeuBep.Data;

namespace YeuBep.CronJobs;

public class DeleteRecipeFromMilvusCronJob
{
    private readonly ILogger<DeleteRecipeFromMilvusCronJob> _logger;
    private readonly YeuBepDbContext _dbContext;
    private readonly MilvusClient _milvusClient;
    public DeleteRecipeFromMilvusCronJob(ILogger<DeleteRecipeFromMilvusCronJob> logger, YeuBepDbContext dbContext, MilvusClient milvusClient)
    {
        _logger = logger;
        _dbContext = dbContext;
        _milvusClient = milvusClient;
    }

    public async Task Run()
    {
        throw new NotImplementedException();
    }
}