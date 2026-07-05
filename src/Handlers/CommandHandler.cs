using System.Reflection;
using Discord;
using DiscordHandler.Models;

namespace DiscordHandler.Handlers;

public static class CommandHandler
{
    public static StartupData LoadSlashCommands(Bot bot)
    {
        var data = new StartupData();

        var commandTypes = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => t.Namespace != null &&
                   t.Namespace.StartsWith("DiscordHandler.Commands.Slash") &&
                   t.GetMethod("GetCommand") != null)
            .ToList();

        foreach (var type in commandTypes)
        {
            try
            {
                var method = type.GetMethod("GetCommand");
                if (method == null) continue;

                var result = method.Invoke(null, null);
                if (result is not SlashCommand cmd) continue;

                bot.SlashCommands[cmd.Name] = cmd;
                var category = type.Namespace?.Split('.').LastOrDefault() ?? "public";
                Console.WriteLine($"  \u2705 Slash command loaded: /{cmd.Data.Description.Name} ({category})");
                data.TotalSlash++;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  \u274c Failed to load slash command {type.Name}: {ex.Message}");
            }
        }

        return data;
    }
}
