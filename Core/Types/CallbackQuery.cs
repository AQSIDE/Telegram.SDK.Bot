using System.Text.Json.Serialization;

namespace Telegram.SDK.Bot.Core.Types;

public sealed class CallbackQuery
{
    [JsonPropertyName("id")] 
    public string Id { get; init; }

    [JsonPropertyName("from")] 
    public User From { get; init; }
    
    [JsonPropertyName("message")] 
    public Message? Message { get; init; }

    [JsonPropertyName("data")] 
    public string? Data { get; init; }
}