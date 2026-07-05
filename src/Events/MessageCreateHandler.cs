using Discord;
using Discord.WebSocket;
using DiscordHandler.Config;
using DiscordHandler.Core;
using DiscordHandler.Models;

namespace DiscordHandler.Events;

public static class MessageCreateHandler
{
    private static readonly CooldownManager CooldownManager = new();

    public static Func<SocketMessage, Task> GetHandler(Bot bot)
    {
        return async (message) =>
        {
            if (message is not SocketUserMessage userMsg)
                return;

            if (userMsg.Author.IsBot)
                return;

            if (userMsg.Channel is not SocketGuildChannel guildChannel)
                return;

            var prefix = BotConfig.Instance.Prefix;
            var content = userMsg.Content;

            if (!content.StartsWith(prefix))
                return;

            var parts = content[prefix.Length..].Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 0)
                return;

            var cmdName = parts[0].ToLower();
            var args = parts.Length > 1 ? parts[1..] : [];

            if (!bot.PrefixCommands.TryGetValue(cmdName, out var cmd))
            {
                cmd = bot.PrefixCommands.Values.FirstOrDefault(c => c.Aliases.Contains(cmdName));
                if (cmd == null)
                    return;
            }

            var userId = userMsg.Author.Id.ToString();
            var (onCooldown, remaining) = CooldownManager.Check(userId, cmd.Name);
            if (onCooldown)
            {
                await userMsg.ReplyAsync($"Please wait {remaining}s before using this command again.");
                return;
            }

            try
            {
                await cmd.Handler(userMsg, args);

                _ = PrefixCommandWebhook.SendUsage(
                    userId: userId,
                    userName: userMsg.Author.ToString(),
                    commandName: cmd.Name,
                    guildName: guildChannel.Guild.Name,
                    avatarUrl: userMsg.Author.GetAvatarUrl() ?? userMsg.Author.GetDefaultAvatarUrl()
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  \u274c Error in prefix command {cmd.Name}: {ex.Message}");
                await userMsg.ReplyAsync($"\u274c **Error:** {ex.Message}");

                _ = PrefixCommandWebhook.SendError(
                    userId: userId,
                    userName: userMsg.Author.ToString(),
                    commandName: cmd.Name,
                    guildName: guildChannel.Guild.Name,
                    errorMsg: ex.Message
                );
            }
        };
    }
}
