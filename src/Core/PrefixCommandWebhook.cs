using DiscordHandler.Config;

namespace DiscordHandler.Core;

public static class PrefixCommandWebhook
{
    public static async Task SendUsage(string userId, string userName, string commandName, string guildName, string? avatarUrl)
    {
        if (!BotConfig.Instance.PrefixCommandWebhookEnabled)
            return;

        var embed = new WebhookEmbed
        {
            Title = $"{Emojis.Slash} Prefix Command Used",
            Description = $"**Command:** `{BotConfig.Instance.Prefix}{commandName}`",
            Color = 0x57F287,
            Fields =
            [
                new WebhookField { Name = $"{Emojis.User} User Info", Value = $"**UserName:** {userName}\n**ID:** {userId}", Inline = true },
                new WebhookField { Name = $"{Emojis.Server} Server", Value = guildName, Inline = true },
                new WebhookField { Name = $"{Emojis.Loading} Time", Value = $"<t:{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}:R>", Inline = true }
            ],
            Footer = new WebhookFooter { Text = WebhookSender.FooterText(BotConfig.Instance.BotName, "Prefix Command Logger") },
            Timestamp = WebhookSender.MakeTimestamp(),
            Thumbnail = new WebhookThumbnail { Url = avatarUrl ?? "https://cdn.discordapp.com/embed/avatars/0.png" }
        };

        await WebhookSender.SendWebhook(BotConfig.Instance.PrefixCommandWebhook, embed);
    }

    public static async Task SendError(string userId, string userName, string commandName, string guildName, string errorMsg)
    {
        if (!BotConfig.Instance.PrefixCommandWebhookEnabled)
            return;

        var embed = new WebhookEmbed
        {
            Title = $"{Emojis.Error} Prefix Command Error",
            Description = $"**Command:** `{BotConfig.Instance.Prefix}{commandName}`\n**Error:** {errorMsg}",
            Color = 0xFF0000,
            Fields =
            [
                new WebhookField { Name = $"{Emojis.User} User Info", Value = $"{userName} ({userId})", Inline = true },
                new WebhookField { Name = $"{Emojis.Server} Server", Value = guildName, Inline = true },
                new WebhookField { Name = $"{Emojis.Loading} Time", Value = $"<t:{DateTimeOffset.UtcNow.ToUnixTimeSeconds()}:F>", Inline = true }
            ],
            Footer = new WebhookFooter { Text = WebhookSender.FooterText(BotConfig.Instance.BotName, "Error Logger") },
            Timestamp = WebhookSender.MakeTimestamp()
        };

        await WebhookSender.SendWebhook(BotConfig.Instance.PrefixCommandWebhook, embed);
    }
}
