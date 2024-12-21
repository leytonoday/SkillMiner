namespace SkillMiner.Domain.Entities.WebScrapingTaskEntity;

public sealed class WebScrapingException(string Message) : Exception($"WebScraping Error - {Message}");