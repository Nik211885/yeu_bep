namespace YeuBep.ViewModels;

public class PaginationViewModel<TResponse>
{
    public IReadOnlyCollection<TResponse> Items { get; set; }
    public PaginationViewModel PaginationView { get; set; }
    public PaginationViewModel(IReadOnlyCollection<TResponse> items, int pageNumber, int pageSize, int totalCount)
    {
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