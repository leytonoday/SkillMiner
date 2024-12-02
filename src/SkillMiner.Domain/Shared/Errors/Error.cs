namespace SkillMiner.Domain.Shared.Errors;

/// <summary>
/// Represents an error with a code and a message.
/// </summary>
/// <param name="Code">The code of the error.</param>
/// <param name="Message">The message of the error.</param>
public record Error(string Code, string Message)
{
    /// <summary>
    /// A generic unknown error.
    /// </summary>
    public static readonly Error UnknownError = new(nameof(UnknownError), "An unknown error has occurred.");
}