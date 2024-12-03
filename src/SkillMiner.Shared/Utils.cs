namespace SkillMiner.Shared;

/// <summary>
/// Provides utility methods used throughout the application.
/// </summary>
public static class Utils
{
    /// <summary>
    /// Determines whether the current environment is development.
    /// </summary>
    /// <returns><c>true</c> if the current environment is Development; otherwise, <c>false</c>.</returns>
    public static bool IsDevelopment()
    {
        return string.Equals(Environment.GetEnvironmentVariable("SKILL_MINER_ENVIRONMENT"), "development", StringComparison.InvariantCultureIgnoreCase);
    }
}