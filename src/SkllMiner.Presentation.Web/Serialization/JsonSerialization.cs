using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;

namespace SkillMiner.Presentation.Web.Serialization;

public static class JsonSerialization
{
    /// <summary>
    /// Specifies whether the JSON serializer should write indented JSON. This is <c>true</c> in debug builds, and <c>false</c> in release builds.
    /// </summary>
    public const bool WriteIndented =
#if DEBUG
            true
#else
            false
#endif
        ;

    /// <summary>
    /// Gets the naming strategy for JSON serialization.
    /// </summary>
    public static NamingStrategy NamingStrategy { get; } = new CamelCaseNamingStrategy();

    /// <summary>
    /// Gets the JSON serializer settings.
    /// </summary>
    public static JsonSerializerSettings Options { get; } = new JsonSerializerSettings
    {
        // Use camelCase property names
        ContractResolver = new DefaultContractResolver
        {
            NamingStrategy = NamingStrategy
        },

        // Write indented JSON based on the current configuration
        Formatting = WriteIndented ? Formatting.Indented : Formatting.None
    };

    /// <summary>
    /// JSON writer options are directly controlled using the serializer's formatting.
    /// </summary>
    public static Formatting WriterOptions => WriteIndented ? Formatting.Indented : Formatting.None;
}