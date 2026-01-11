using System.Text.Json.Serialization;
using Telegram.SDK.Bot.Keyboards.Inline;
using Telegram.SDK.Bot.Keyboards.Reply;

namespace Telegram.SDK.Bot.Interfaces;

[JsonDerivedType(typeof(InlineMarkup))]
[JsonDerivedType(typeof(ReplyMarkup))]
public interface IKeyboardMarkup { }