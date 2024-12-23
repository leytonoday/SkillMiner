namespace SkillMiner.Application.Shared.Models;

public class PagedResponse<T>
{
    public IEnumerable<T> Items { get; set; }

    public int PageSize { get; set; }

    public int PageNumber { get; set; }

    public int TotalCount { get; set; }

    public bool HasNextPage { get; set; }
}