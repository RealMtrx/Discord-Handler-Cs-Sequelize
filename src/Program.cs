using System.Diagnostics;

using DiscordHandler;
using DiscordHandler.Config;
using DiscordHandler.Database;
using DiscordHandler.Handlers;

DotNetEnv.Env.Load();

BotConfig.Load();
var config = BotConfig.Instance;

Console.WriteLine();
Console.WriteLine(new string('=', 50));
Console.WriteLine($"  Starting {config.BotName}...");
Console.WriteLine(new string('=', 50));
Console.WriteLine();

var stopwatch = Stopwatch.StartNew();

Console.WriteLine("  \ud83d\udee1\ufe0f Setting up anti-crash...");
AntiCrash.Setup();

var bot = new Bot();

Console.WriteLine("  \ud83d\udce0 Loading events...");
var eventData = EventHandler.LoadEvents(bot);

Console.WriteLine("  \u26a1 Loading slash commands...");
var slashData = CommandHandler.LoadSlashCommands(bot);

Console.WriteLine("  \ud83d\udcac Loading prefix commands...");
var prefixData = PrefixHandler.LoadPrefixCommands(bot);

Console.WriteLine("  \ud83c\udfdb\ufe0f Setting up database...");
using var db = new AppDbContext();
await db.Database.EnsureCreatedAsync();

var data = new StartupData
{
    TotalSlash = slashData.TotalSlash,
    TotalPrefix = prefixData.TotalPrefix,
    TotalEvents = eventData.TotalEvents
};

stopwatch.Stop();
Logger.PrintStartupBanner(data, stopwatch.Elapsed.TotalSeconds);

try
{
    await bot.StartAsync();
    await Task.Delay(-1);
}
finally
{
    await bot.StopAsync();
}
