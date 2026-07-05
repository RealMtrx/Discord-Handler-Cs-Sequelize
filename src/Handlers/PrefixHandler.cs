using System.Reflection;
using DiscordHandler.Config;
using DiscordHandler.Models;

namespace DiscordHandler.Handlers;

public static class PrefixHandler
{
    public static StartupData LoadPrefixCommands(Bot bot)
    {
        var data = new StartupData();

        var commandTypes = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => t.Namespace != null &&
                   t.Namespace.StartsWith("DiscordHandler.Commands.Prefix") &&
                   t.GetMethod("GetCommand") != null)
            .ToList();

        foreach (var type in commandTypes)
        {
            try
            {
                var method = type.GetMethod("GetCommand");
                if (method == null) continue;

                var result = method.Invoke(null, null);
                if (result is not PrefixCommand cmd) continue;

                bot.PrefixCommands[cmd.Name] = cmd;
                var category = type.Namespace?.Split('.').LastOrDefault() ?? "public";
                Console.WriteLine($"  \u2705 Prefix command loaded: {BotConfig.Instance.Prefix}{cmd.Name} ({category})");
                data.TotalPrefix++;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  \u274c Failed to load prefix command {type.Name}: {ex.Message}");
            }
        }

        return data;
    }
}
