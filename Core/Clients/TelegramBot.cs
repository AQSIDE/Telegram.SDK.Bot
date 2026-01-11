using System.Text.Json;
using Telegram.SDK.Bot.Core.Methods;
using Telegram.SDK.Bot.Core.Types;
using Telegram.SDK.Bot.Data;
using Telegram.SDK.Bot.Enums;
using Telegram.SDK.Bot.Interfaces;

namespace Telegram.SDK.Bot.Core.Clients;

public class TelegramBot
{
    private readonly List<Func<Update, Task>> _updateHandlers = new();
    private readonly List<Func<Exception, Update?, Task>> _errorHandlers = new();
    
    private readonly TelegramClient _client;
    private CancellationTokenSource? _cts;
    private bool _isRunning;

    public bool IsRunning => _isRunning;
    public BotOptions Options { get; }

    public TelegramBot(BotOptions options)
    {
        Options = options;
        _client = new TelegramClient(options);
    }

    public Task Run()
    {
        if (_isRunning)
            return Task.CompletedTask;
        
        _cts = new CancellationTokenSource();
        _isRunning = true;

        try
        {
            return _client.StartReceiving(_updateHandlers, _errorHandlers, _cts.Token);
        }
        finally
        {
            _isRunning = false;
        }
    }

    public void Stop()
    { 
        if (!_isRunning || _cts == null) return;
        
        _cts.Cancel();
        _cts.Dispose();
        _cts = null;
        _isRunning = false;
    }

    public void AddUpdateHandler(Func<Update, Task> handler) => _updateHandlers.Add(handler);
    public void AddErrorHandler(Func<Exception, Update?, Task> handler) => _errorHandlers.Add(handler);
    
    public void RemoveUpdateHandler(Func<Update, Task> handler) => _updateHandlers.Remove(handler);
    public void RemoveErrorHandler(Func<Exception, Update?, Task> handler) => _errorHandlers.Remove(handler);

    public async Task SendText(long chatId, string text, IKeyboardMarkup? replyMarkup = null, ParseMode? parseMode = null)
    {
        await _client.CallAsync<object>(TelegramMethods.SendMessage, new
        {
            chat_id = chatId,
            text = text,
            parse_mode = (parseMode ?? Options.DefaultParseMode).ToString(),
            reply_markup = replyMarkup
        });
    }

    public async Task<Message?> EditMessage(long chatId, long messageId, string text, IKeyboardMarkup? replyMarkup = null, ParseMode? parseMode = null)
    {
        return await _client.CallAsync<Message>(TelegramMethods.EditMessageText, new
        {
            chat_id = chatId,
            message_id = messageId,
            text = text,
            parse_mode = (parseMode ?? Options.DefaultParseMode).ToString(),
            reply_markup = replyMarkup
        });
    }

    public async Task<bool> DeleteMessage(long chatId, long messageId)
    {
        return await _client.CallAsync<bool>(TelegramMethods.DeleteMessage, new
        {
            chat_id = chatId,
            message_id = messageId
        });
    }

    public async Task AnswerCallback(string callbackId, string? text = null, bool showAlert = false)
    {
        await _client.CallAsync<object>(TelegramMethods.AnswerCallbackQuery, new
        {
            callback_query_id = callbackId,
            text = text,
            show_alert = showAlert
        });
    }

    public async Task<T> SendRequest<T>(string method, object? body = null, JsonSerializerOptions? options = null)
        => await _client.CallAsync<T>(method, body, options);
}