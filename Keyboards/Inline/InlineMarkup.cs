using System.Text.Json.Serialization;
using Telegram.SDK.Bot.Interfaces;

namespace Telegram.SDK.Bot.Keyboards.Inline;

public sealed class InlineMarkup : IKeyboardMarkup
{
    [JsonPropertyName("inline_keyboard")]
    public InlineButton[][] Buttons { get; } 

    public InlineMarkup(InlineButton[][] buttons) => Buttons = buttons;
}