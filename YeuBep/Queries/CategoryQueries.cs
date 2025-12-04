using YeuBep.Controllers;
using YeuBep.Data;

namespace YeuBep.Queries;

public class CategoryQueries
{
    private readonly ILogger<CategoryController> _logger;
    private readonly YeuBepDbContext _yeuBepDbContext;

    public CategoryQueries(ILogger<CategoryController> logger, YeuBepDbContext yeuBepDbContext)
    {
        _logger = logger;
        _yeuBepDbContext = yeuBepDbContext;
    }
}