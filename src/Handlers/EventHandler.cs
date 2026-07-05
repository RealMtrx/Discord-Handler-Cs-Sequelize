using Discord;
using DiscordHandler.Events;
using DiscordHandler.Models;

namespace DiscordHandler.Handlers;

public static class EventHandler
{
    public static StartupData LoadEvents(Bot bot)
    {
        var data = new StartupData();

        try
        {
            bot.Client.Log += ErrorHandler.GetHandler();
            Console.WriteLine("  \u2705 Loaded event: on_error");
            data.TotalEvents++;

            bot.Client.Ready += ReadyHandler.GetHandler(bot);
            Console.WriteLine("  \u2705 Loaded event: on_ready");
            data.TotalEvents++;

            bot.Client.MessageReceived += MessageCreateHandler.GetHandler(bot);
            Console.WriteLine("  \u2705 Loaded event: on_message");
            data.TotalEvents++;

            bot.Client.InteractionCreated += InteractionCreateHandler.GetHandler(bot);
            Console.WriteLine("  \u2705 Loaded event: on_interaction");
            data.TotalEvents++;

            bot.Client.JoinedGuild += GuildCreateHandler.GetHandler();
            Console.WriteLine("  \u2705 Loaded event: on_guild_join");
            data.TotalEvents++;

            bot.Client.LeftGuild += GuildDeleteHandler.GetHandler(() => bot.Client.Guilds.Count);
            Console.WriteLine("  \u2705 Loaded event: on_guild_remove");
            data.TotalEvents++;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"  \u274c Failed to load events: {ex.Message}");
        }

        return data;
    }
}
