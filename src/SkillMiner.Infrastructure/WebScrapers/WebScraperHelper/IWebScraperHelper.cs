using HtmlAgilityPack;

namespace SkillMiner.Infrastructure.WebScrapers.WebScraperHelper;

/// <summary>
/// Generic WebScraper. Doesn't parse the HTML it scrapes, only retrieves it.
/// </summary>
public interface IWebScraperHelper
{

    /// <summary>
    /// Goes to a Webpage, renders the content, and returns it's HTML as a string.
    /// </summary>
    /// <param name="url">The URL of the website to read.</param>
    /// <param name="renderTimeoutMs">An amount of time in milliseconds to wait for the page to render fully before reading it's content.</param>
    /// <param name="cancellationToken">Cancels the operation.</param>
    /// <returns>The HTML content of the page.</returns>
    public Task<string?> ReadWebpageToHtmlStringAsync(string url, int renderTimeoutMs, CancellationToken cancellationToken);

    /// <summary>
    /// Parses HTML into a tree of nodes.
    /// </summary>
    /// <param name="stringHtml">The HTML string to parse.</param>
    /// <returns>Parsed HTML in a navigatable tree structure..</returns>
    public HtmlDocument StringToHtmlDocument(string stringHtml);
}
