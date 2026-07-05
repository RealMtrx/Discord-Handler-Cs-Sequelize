using DiscordHandler.Config;

namespace DiscordHandler.Core;

public static class ErrorWebhook
{
    public static async Task SendErrorWebhook(string errorMsg)
    {
        if (!BotConfig.Instance.ErrorWebhookEnabled)
            return;

        var embed = new WebhookEmbed
        {
            Title = "\u274c Bot Error Report",
            Description = $"**Error:** {errorMsg}",
            Color = 0xFF0000,
            Fields =
            [
                new WebhookField { Name = "\ud83d\udcc5 Timestamp", Value = WebhookSender.MakeTimestamp(), Inline = true }
            ],
            Footer = new WebhookFooter { Text = WebhookSender.FooterText(BotConfig.Instance.BotName, "Error Logger") },
            Timestamp = WebhookSender.MakeTimestamp()
        };

        await WebhookSender.SendWebhook(BotConfig.Instance.ErrorWebhook, embed);
    }
}
