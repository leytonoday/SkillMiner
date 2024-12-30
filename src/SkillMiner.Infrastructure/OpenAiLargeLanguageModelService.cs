using Microsoft.Extensions.Configuration;
using OpenAI.Chat;
using SkillMiner.Application.Services;
using System.Text.Json;

namespace SkillMiner.Infrastructure;

public class OpenAiLargeLanguageModelService
    (IConfiguration configuration)
    : ILargeLanguageModelService
{
    public async Task<IEnumerable<string>> ConvertToKeywordsAsync(string textToConvert, string prompt, int maxKeywords, CancellationToken cancellationToken)
    {
        string openAiApiKey = configuration["ApiKeys:OpenAi"]
            ?? throw new Exception("OpenAI Api Key not set");

        var client = new ChatClient(model: "gpt-4o-mini", apiKey: openAiApiKey);

        ChatCompletion completion = await client.CompleteChatAsync([
            new SystemChatMessage(prompt),
            new UserChatMessage(textToConvert)
            ],
            new ChatCompletionOptions()
            {
                ResponseFormat = ChatResponseFormat.CreateJsonObjectFormat()
            }, 
            cancellationToken);

        string responseString = completion.Content[0].Text;

        int startOfArray = responseString.IndexOf('[');
        int endOfArray = responseString.LastIndexOf(']') + 1;

        string jsonArray = responseString.Substring(startOfArray, endOfArray - startOfArray);

        return (JsonSerializer.Deserialize<IEnumerable<string>>(jsonArray) ?? []).Take(maxKeywords);
    }
}
