using System.Text.Json.Serialization;

namespace YeuBep.ViewModels;

public static class PaginationExtension
{
    public static PaginationViewModel<object> CastToObjectType<TType>(this PaginationViewModel<TType> pagination)
    {
        var result = new PaginationViewModel<object>(
            pagination.Items.Cast<object>().ToList(), 
            pagination.PaginationView.PageNumber, 
            pagination.PaginationView.PageSize, 
            pagination.PaginationView.TotalCount)
        {
            DataTypeItem = typeof(TType)
        };

        return result;
    }
}

public class PaginationViewModel<TResponse>
{
    [JsonIgnore]
    public Type DataTypeItem { get; set; }
    public IReadOnlyCollection<TResponse> Items { get; set; }
    public PaginationViewModel PaginationView { get; set; }
    
    public PaginationViewModel(IReadOnlyCollection<TResponse> items, int pageNumber, int pageSize, int totalCount)
    {
        DataTypeItem = typeof(TResponse);
        PaginationView ??= new PaginationViewModel();
        Items = items;
        PaginationView.PageNumber = pageNumber;
        PaginationView.PageSize = pageSize;
        PaginationView.TotalCount = totalCount;
        PaginationView.TotalPages = (int)Math.Ceiling((double)PaginationView.TotalCount / PaginationView.PageSize);
    }
}

public class PaginationViewModel
{
    public int PageNumber { get; set; }
    public int TotalPages { get; set; }
    public int TotalCount { get; set; }
    public int PageSize { get; set; }
    public bool HasPreviousPage => PageNumber > 1;  
    public bool HasNextPage => PageNumber < TotalPages;
}