namespace SkillMiner.Application.Services;

/// <summary>
/// Service interface for interacting with a large language model.
/// </summary>
public interface ILargeLanguageModelService
{
    /// <summary>
    /// Converts the provided text into a series of keywords that represent the text.
    /// </summary>
    /// <param name="textToConvert">The text to convert into keywords.</param>
    /// <param name="prompt">A custom prompt to indicate what the LLM should do when identifying keywords.</param>
    /// <param name="maxKeywords">Maximum amount of keywords to generate.</param>
    /// <param name="cancellationToken">Propagates notification that operations should be cancelled.</param>
    /// <returns>A task representing the asynchronous operation that yields a collection of keywords.</returns>
    public Task<IEnumerable<string>> ConvertToKeywordsAsync(string textToConvert, string prompt, int maxKeywords, CancellationToken cancellationToken);
}
