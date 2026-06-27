namespace Library.Application.Common.Models;

/// <summary>Pagination metadata included with paged responses.</summary>
public sealed record PaginationMeta(int Page, int PageSize, int TotalCount)
{
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    public bool HasPreviousPage => Page > 1;
    public bool HasNextPage => Page < TotalPages;
}

