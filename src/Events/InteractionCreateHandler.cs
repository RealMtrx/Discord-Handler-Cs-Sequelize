using Discord;
using Discord.WebSocket;
using DiscordHandler.Core;
using DiscordHandler.Models;

namespace DiscordHandler.Events;

public static class InteractionCreateHandler
{
    private static readonly CooldownManager CooldownManager = new();

    public static Func<SocketInteraction, Task> GetHandler(Bot bot)
    {
        return async (interaction) =>
        {
            if (interaction is not SocketSlashCommand slash)
                return;

            var commandName = slash.CommandName;

            if (slash.Channel is not SocketGuildChannel guildChannel)
            {
                await slash.RespondAsync("Commands are only available in servers!", ephemeral: true);
                return;
            }

            if (!bot.SlashCommands.TryGetValue(commandName, out var cmd))
                return;

            var user = slash.User;
            var userId = user.Id.ToString();
            var guildName = guildChannel.Guild.Name;
            var userName = user.ToString();

            var (onCooldown, remaining) = CooldownManager.Check(userId, commandName);
            if (onCooldown)
            {
                await slash.RespondAsync($"Please wait {remaining}s before using this command again.", ephemeral: true);
                return;
            }

            try
            {
                await cmd.Handler(slash);

                _ = SlashCommandWebhook.SendUsage(
                    userId: userId,
                    userName: userName,
                    commandName: commandName,
                    guildName: guildName,
                    avatarUrl: user.GetAvatarUrl() ?? user.GetDefaultAvatarUrl()
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  \u274c Error in slash command {commandName}: {ex.Message}");

                var embed = new EmbedBuilder()
                    .WithTitle("\u274c An error occurred")
                    .WithDescription($"**Error:** {ex.Message}")
                    .WithColor(Color.Red)
                    .Build();

                try
                {
                    if (!slash.HasResponded)
                        await slash.RespondAsync(embed: embed, ephemeral: true);
                    else
                        await slash.FollowupAsync(embed: embed, ephemeral: true);
                }
                catch { }

                await SlashCommandWebhook.SendError(
                    userId: userId,
                    userName: userName,
                    commandName: commandName,
                    guildName: guildName,
                    errorMsg: ex.Message
                );
            }
        };
    }
}
