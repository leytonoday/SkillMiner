using HtmlAgilityPack;
using PuppeteerSharp;

namespace SkillMiner.Infrastructure.WebScrapers.WebScraperHelper;

public class WebScraperHelper : IWebScraperHelper
{
    public async Task<string?> ReadWebpageToHtmlStringAsync(string url, int renderTimeoutMs, CancellationToken cancellationToken)
    {
        // Only downloads it if it hasn't already been downloaded. Should only download one time.
        await new BrowserFetcher().DownloadAsync();

        using var browser = await Puppeteer.LaunchAsync(new LaunchOptions
        {
            Headless = true, // Run in headless mode (no browser window)
            Args = ["--no-sandbox", "--disable-setuid-sandbox"] // Important for some environments like Docker
        });

        using var page = await browser.NewPageAsync();
        try
        {
            await page.GoToAsync(url, new NavigationOptions { WaitUntil = [WaitUntilNavigation.Networkidle2], Timeout = 10000 }); // Wait for the network to be idle

            await Task.Delay(renderTimeoutMs, cancellationToken);

            return await page.GetContentAsync();
        }
        catch (NavigationException ex)
        {
            // Handle navigation errors (e.g., invalid URL, timeout)
            Console.WriteLine($"Navigation error: {ex.Message}");
            return null; // Or throw the exception if you prefer
        }
        catch (Exception ex)
        {
            // Handle other exceptions (e.g. PuppeteerSharp errors)
            Console.WriteLine($"An error occurred: {ex.Message}");
            return null;
        }
        finally
        {
            await page.CloseAsync();
            await browser.CloseAsync();
        }
    }

    public HtmlDocument StringToHtmlDocument(string stringHtml)
    {
        var document = new HtmlDocument();
        document.LoadHtml(stringHtml);
        return document;
    }
}
