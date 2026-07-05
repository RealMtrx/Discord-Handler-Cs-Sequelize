using DiscordHandler.Config;

namespace DiscordHandler.Core;

public static class ReadyWebhook
{
    public static async Task SendReadyWebhook(string botUsername, string botId, int serverCount)
    {
        if (!BotConfig.Instance.ReadyWebhookEnabled)
            return;

        var embed = new WebhookEmbed
        {
            Title = "\ud83d\udfe2 Bot is Online!",
            Description = $"**Bot:** {botUsername}\n**Status:** Online and Ready",
            Color = 0x00FF00,
            Fields =
            [
                new WebhookField { Name = "\ud83e\udd16 Bot Info", Value = $"**ID:** {botId}", Inline = true },
                new WebhookField { Name = "\ud83c\udfe0 Servers", Value = $"{serverCount} servers", Inline = true }
            ],
            Footer = new WebhookFooter { Text = WebhookSender.FooterText(BotConfig.Instance.BotName, "System Logger") },
            Timestamp = WebhookSender.MakeTimestamp()
        };

        await WebhookSender.SendWebhook(BotConfig.Instance.ReadyWebhook, embed);
    }
}
