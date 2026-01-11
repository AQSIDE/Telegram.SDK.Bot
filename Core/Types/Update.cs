using System.Text.Json.Serialization;
using Telegram.SDK.Bot.Enums;

namespace Telegram.SDK.Bot.Core.Types;

public sealed class Update
{
    [JsonPropertyName("update_id")] 
    public long Id { get; set; }

    // -- Messages --
    [JsonPropertyName("message")] 
    public Message? Message { get; set; }

    [JsonPropertyName("edited_message")] 
    public Message? EditedMessage { get; set; }

    [JsonPropertyName("channel_post")] 
    public Message? ChannelPost { get; set; }

    [JsonPropertyName("edited_channel_post")]
    public Message? EditedChannelPost { get; set; }
    
    // -- Inline & Callbacks --
    [JsonPropertyName("callback_query")] 
    public CallbackQuery? CallbackQuery { get; set; }

    [JsonPropertyName("inline_query")] 
    public InlineQuery? InlineQuery { get; set; }

    [JsonPropertyName("chosen_inline_result")]
    public ChosenInlineResult? ChosenInlineResult { get; set; }

    // -- Business --
    // [JsonPropertyName("business_connection")]
    // public object? BusinessConnection { get; set; } 
    
    // [JsonPropertyName("business_message")] 
    // public Message? BusinessMessage { get; set; }
    
    // [JsonPropertyName("edited_business_message")]
    // public Message? EditedBusinessMessage { get; set; }
    
    // [JsonPropertyName("deleted_business_messages")]
    // public object? DeletedBusinessMessages { get; set; }
    
    // // -- Reactions & Boosts --
    // [JsonPropertyName("message_reaction")] 
    // public object? MessageReaction { get; set; }
    
    // [JsonPropertyName("message_reaction_count")]
    // public object? MessageReactionCount { get; set; }
    
    // [JsonPropertyName("chat_boost")] 
    // public object? ChatBoost { get; set; }
    
    // [JsonPropertyName("chat_boost_removed")]
    // public object? ChatBoostRemoved { get; set; }
    
    // // -- Polls --
    // [JsonPropertyName("poll")] 
    // public object? Poll { get; set; }
    
    // [JsonPropertyName("poll_answer")] 
    // public object? PollAnswer { get; set; }
    
    // // -- Payments --
    // [JsonPropertyName("shipping_query")] 
    // public object? ShippingQuery { get; set; }
    
    // [JsonPropertyName("pre_checkout_query")]
    // public object? PreCheckoutQuery { get; set; }
    
    // [JsonPropertyName("purchased_paid_media")]
    // public object? PurchasedPaidMedia { get; set; }
    
    // // -- Admin & Members --
    // [JsonPropertyName("chat_member")] 
    // public object? ChatMember { get; set; }
    
    // [JsonPropertyName("my_chat_member")] 
    // public object? MyChatMember { get; set; }
    
    // [JsonPropertyName("chat_join_request")]
    // public object? ChatJoinRequest { get; set; }

    public UpdateType Type { get; set; }
}