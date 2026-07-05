using DiscordHandler.Core;

namespace DiscordHandler.Handlers;

public static class AntiCrash
{
    public static void Setup()
    {
        AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
        {
            var ex = args.ExceptionObject as Exception;
            Console.WriteLine($"  \u274c Unhandled Exception: {ex?.Message}");
            _ = ErrorWebhook.SendErrorWebhook($"**Unhandled Exception**\n```{ex?.Message?[..Math.Min(ex.Message.Length, 1900)]}```");
        };

        TaskScheduler.UnobservedTaskException += (sender, args) =>
        {
            Console.WriteLine($"  \u274c Unobserved Task Exception: {args.Exception?.Message}");
            _ = ErrorWebhook.SendErrorWebhook($"**Task Exception**\n```{args.Exception?.Message?[..Math.Min(args.Exception.Message.Length, 1900)]}```");
            args.SetObserved();
        };

        Console.WriteLine("  \u2705 AntiCrash handler set up");
    }
}
