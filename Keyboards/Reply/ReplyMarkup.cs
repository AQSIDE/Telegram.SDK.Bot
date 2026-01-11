using System.Text.Json.Serialization;
using Telegram.SDK.Bot.Interfaces;

namespace Telegram.SDK.Bot.Keyboards.Reply;

public sealed class ReplyMarkup : IKeyboardMarkup
{
    [JsonPropertyName("keyboard")]
    public ReplyButton[][] Buttons { get; }
    
    [JsonPropertyName("input_field_placeholder")]
    public string? Placeholder { get; set; }

    [JsonPropertyName("resize_keyboard")]
    public bool ResizeKeyboard { get; set; } = true;

    [JsonPropertyName("one_time_keyboard")]
    public bool OneTimeKeyboard { get; set; } = true;

    public ReplyMarkup(ReplyButton[][] buttons) => Buttons = buttons;
}