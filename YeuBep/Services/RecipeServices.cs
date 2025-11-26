using YeuBep.Data;

namespace YeuBep.Services;

public class RecipeServices
{
    private readonly YeuBepDbContext _dbContext;
    private readonly ILogger<RecipeServices> _logger;

    public RecipeServices(YeuBepDbContext dbContext, ILogger<RecipeServices> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }
}