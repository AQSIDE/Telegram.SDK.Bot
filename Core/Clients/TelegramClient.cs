using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Telegram.SDK.Bot.Core.Methods;
using Telegram.SDK.Bot.Core.Types;
using Telegram.SDK.Bot.Data;
using Telegram.SDK.Bot.Enums;

namespace Telegram.SDK.Bot.Core.Clients;

internal class TelegramClient
{
    private readonly HttpClient _http;
    private readonly Uri _baseUrl;
    private readonly BotOptions _options;
    private readonly string _token;

    private long _offset;

    public TelegramClient(BotOptions options)
    {
        _token = options.Token;
        _options = options;
        _offset = options.Offset;
        _baseUrl = new Uri($"https://api.telegram.org/bot{options.Token}/");
        _http = new HttpClient
        {
            BaseAddress = _baseUrl
        };
    }

    public async Task StartReceiving(List<Func<Update, Task>> updateHandlers, List<Func<Exception, Update?, Task>> errorHandlers,
        CancellationToken ct)
    {
        while (!ct.IsCancellationRequested)
        {
            try
            {
                var updates = await CallAsync<Update[]>(TelegramMethods.GetUpdates, new
                {
                    offset = _offset,
                    timeout = _options.Timeout,
                    allowed_updates = _options.GetAllowedUpdatesNames(_options.AllowedUpdates)
                });

                if (updates.Length == 0) continue;
                
                foreach (var update in updates)
                {
                    try
                    {
                        SetUpdateType(update);
                        await Task.WhenAll(updateHandlers.Select(f => f.Invoke(update)));
                    }
                    catch (Exception ex)
                    {
                        await Task.WhenAll(errorHandlers.Select(f => f.Invoke(ex, update)));
                    }
                    finally
                    {
                        _offset = update.Id + 1;
                    }
                }
            }
            catch (OperationCanceledException)
            {
            }
            catch (Exception ex)
            {
                await Task.WhenAll(errorHandlers.Select(f => f.Invoke(ex, null)));
                await Task.Delay(5000, ct);
            }
        }
    }

    public async Task<T> CallAsync<T>(string method, object? body = null, JsonSerializerOptions? options = null)
    {
        options ??= new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        HttpResponseMessage response;

        if (body != null)
        {
            var json = JsonSerializer.Serialize(body, options);
            response = await _http.PostAsync(method, new StringContent(json, Encoding.UTF8, "application/json"));
        }
        else
        {
            response = await _http.GetAsync(method);
        }

        response.EnsureSuccessStatusCode();

        var stream = await response.Content.ReadAsStreamAsync();
        var apiResponse = await JsonSerializer.DeserializeAsync<TelegramResponse<T>>(stream, options);

        if (apiResponse == null) throw new Exception("Failed to deserialize Telegram response");
        if (!apiResponse.Ok) throw new Exception(apiResponse.Description);

        return apiResponse.Result;
    }

    private void SetUpdateType(Update update)
    {
        var type = update switch
        {
            { Message: not null } => UpdateType.Message,
            { EditedMessage: not null } => UpdateType.EditedMessage,
            { CallbackQuery: not null } => UpdateType.CallbackQuery,
            { ChannelPost: not null } => UpdateType.ChannelPost,
            { EditedChannelPost: not null } => UpdateType.EditedChannelPost,
            { InlineQuery: not null } => UpdateType.InlineQuery,
            { ChosenInlineResult: not null } => UpdateType.ChosenInlineResult,
            // { BusinessConnection: not null } => UpdateType.BusinessConnection,
            // { BusinessMessage: not null } => UpdateType.BusinessMessage,
            // { EditedBusinessMessage: not null } => UpdateType.EditedBusinessMessage,
            // { DeletedBusinessMessages: not null } => UpdateType.DeletedBusinessMessages,
            // { MessageReaction: not null } => UpdateType.MessageReaction,
            // { MessageReactionCount: not null } => UpdateType.MessageReactionCount,
            // { ChatBoost: not null } => UpdateType.ChatBoost,
            // { ChatBoostRemoved: not null } => UpdateType.ChatBoostRemoved,
            // { Poll: not null } => UpdateType.Poll,
            // { PollAnswer: not null } => UpdateType.PollAnswer,
            // { ShippingQuery: not null } => UpdateType.ShippingQuery,
            // { PreCheckoutQuery: not null } => UpdateType.PreCheckoutQuery,
            // { PurchasedPaidMedia: not null } => UpdateType.PurchasedPaidMedia,
            // { ChatMember: not null } => UpdateType.ChatMember,
            // { MyChatMember: not null } => UpdateType.MyChatMember,
            // { ChatJoinRequest: not null } => UpdateType.ChatJoinRequest,
            _ => UpdateType.Unknown
        };
        
        update.Type = type;
    }
}