using Telegram.SDK.Bot.Enums;

namespace Telegram.SDK.Bot.Data;

public class BotOptions
{
    public string Token { get; }
    public long Offset { get; set; }
    public int Timeout { get; set; } = 30;
    public UpdateType[] AllowedUpdates { get; set; }
    public ParseMode DefaultParseMode { get; set; }

    public BotOptions(string token, UpdateType[] allowedUpdates)
    {
        Token = token;
        AllowedUpdates = allowedUpdates;
    }

    internal string[] GetAllowedUpdatesNames(UpdateType[] allowedUpdates)
    {
        string GetName(UpdateType type)
        {
            return type switch
            {
                UpdateType.Message => "message",
                UpdateType.CallbackQuery => "callback_query",
                UpdateType.EditedMessage => "edited_message",
                UpdateType.ChannelPost => "channel_post",
                _ => "unknown",
            };
        }
        
        return allowedUpdates.Select(GetName).ToArray();
    }
}