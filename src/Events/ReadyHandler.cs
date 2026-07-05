using Discord;
using Discord.WebSocket;
using DiscordHandler.Config;
using DiscordHandler.Core;

namespace DiscordHandler.Events;

public static class ReadyHandler
{
    private static bool _readyCalled;

    public static Func<Task> GetHandler(Bot bot)
    {
        return async () =>
        {
            if (_readyCalled)
                return;
            _readyCalled = true;

            var client = bot.Client;
            if (client.CurrentUser == null)
                return;

            Console.WriteLine($"  \u2705 Logged in as {client.CurrentUser} (ID: {client.CurrentUser.Id})");

            await client.SetGameAsync($"{BotConfig.Instance.BotName} | {BotConfig.Instance.Prefix}help", null, ActivityType.Watching);

            _ = ReadyWebhook.SendReadyWebhook(
                botUsername: client.CurrentUser.ToString() ?? "Unknown",
                botId: client.CurrentUser.Id.ToString(),
                serverCount: client.Guilds.Count
            );

            _ = RegisterSlashCommands(bot);
        };
    }

    private static async Task RegisterSlashCommands(Bot bot)
    {
        try
        {
            foreach (var (name, cmd) in bot.SlashCommands)
            {
                await bot.Client.CreateGlobalApplicationCommandAsync(cmd.Data);
            }
            Console.WriteLine($"  \u2705 Registered {bot.SlashCommands.Count} global slash commands");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"  \u274c Failed to register slash commands: {ex.Message}");
        }
    }
}
