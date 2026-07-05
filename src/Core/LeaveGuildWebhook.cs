using DiscordHandler.Config;

namespace DiscordHandler.Core;

public static class LeaveGuildWebhook
{
    public static async Task SendLeaveGuildWebhook(string guildId, string guildName, int memberCount, int remainingServers)
    {
        if (!BotConfig.Instance.LeaveGuildWebhookEnabled)
            return;

        var embed = new WebhookEmbed
        {
            Title = "\ud83d\udc4b Bot Left Server",
            Description = $"**Server:** {guildName}\n**ID:** {guildId}",
            Color = 0xFF0000,
            Fields =
            [
                new WebhookField { Name = "\ud83d\udc65 Members", Value = $"{memberCount} members", Inline = true },
                new WebhookField { Name = "\ud83d\udcc5 Left At", Value = $"<t:{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}:F>", Inline = true },
                new WebhookField { Name = "\ud83d\udcca Remaining Servers", Value = $"{remainingServers} servers", Inline = true }
            ],
            Footer = new WebhookFooter { Text = WebhookSender.FooterText(BotConfig.Instance.BotName, "Guild Leave Logger") },
            Timestamp = WebhookSender.MakeTimestamp()
        };

        await WebhookSender.SendWebhook(BotConfig.Instance.LeaveGuildWebhook, embed);
    }
}
