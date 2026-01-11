using System.Text.Json.Serialization;

namespace Telegram.SDK.Bot.Core.Types;

public sealed class Message
{
    [JsonPropertyName("message_id")]
    public long Id { get; set; }
    
    [JsonPropertyName("chat")]
    public Chat? Chat { get; set; }
    
    [JsonPropertyName("text")]
    public string? Text { get; set; }
    
    [JsonPropertyName("from")]
    public User? From { get; set; }
    
    public bool IsEmpty => string.IsNullOrEmpty(Text);
}