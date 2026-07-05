using DiscordHandler.Config;
using DiscordHandler.Models;

namespace DiscordHandler.Handlers;

public static class Logger
{
    public static void PrintStartupBanner(StartupData data, double elapsed)
    {
        var now = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss UTC");

        Console.WriteLine();
        Console.WriteLine(new string('=', 50));
        Console.WriteLine($"  {BotConfig.Instance.BotName}");
        Console.WriteLine(new string('=', 50));
        Console.WriteLine($"  \u23f0 Started at: {now}");
        Console.WriteLine($"  \u2699\ufe0f Loaded {data.TotalSlash} slash commands");
        Console.WriteLine($"  \ud83d\udce0 Loaded {data.TotalPrefix} prefix commands");
        Console.WriteLine($"  \ud83c\udf89 Loaded {data.TotalEvents} events");
        Console.WriteLine($"  \u26a1 Ready in {elapsed:F2}s");
        Console.WriteLine(new string('=', 50));
        Console.WriteLine();
    }
}
