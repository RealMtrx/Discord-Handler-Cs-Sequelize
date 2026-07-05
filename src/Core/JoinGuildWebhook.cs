using DiscordHandler.Config;

namespace DiscordHandler.Core;

public static class JoinGuildWebhook
{
    public static async Task SendJoinGuildWebhook(string guildName, string guildId, string ownerId, int memberCount, string? iconUrl)
    {
        if (!BotConfig.Instance.JoinGuildWebhookEnabled)
            return;

        var embed = new WebhookEmbed
        {
            Title = "\ud83c\udf89 Bot Joined New Server!",
            Description = $"**Server:** {guildName}\n**ID:** {guildId}",
            Color = 0x57F287,
            Fields =
            [
                new WebhookField { Name = "\ud83d\udc51 Owner", Value = $"<@{ownerId}>", Inline = true },
                new WebhookField { Name = "\ud83d\udc65 Members", Value = $"{memberCount} members", Inline = true },
                new WebhookField { Name = "\ud83d\udcc5 Joined At", Value = $"<t:{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}:F>", Inline = true }
            ],
            Footer = new WebhookFooter { Text = WebhookSender.FooterText(BotConfig.Instance.BotName, "Guild Join Logger") },
            Timestamp = WebhookSender.MakeTimestamp(),
            Thumbnail = new WebhookThumbnail { Url = iconUrl ?? "https://cdn.discordapp.com/embed/avatars/0.png" }
        };

        await WebhookSender.SendWebhook(BotConfig.Instance.JoinGuildWebhook, embed);
    }
}
