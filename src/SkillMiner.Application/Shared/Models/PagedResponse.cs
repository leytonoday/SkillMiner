namespace SkillMiner.Application.Shared.Models;

public class PagedResponse<T>
{
    public IEnumerable<T> Items { get; set; }

    public int PageSize { get; set; }

    public int PageNumber { get; set; }

    public int TotalItemsInPage { get; set; }

    public int TotalItems { get; set; }

    public bool HasNextPage { get; set; }
}