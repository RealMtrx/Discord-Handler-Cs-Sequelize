using Discord;

namespace DiscordHandler.Events;

public static class ErrorHandler
{
    public static Func<LogMessage, Task> GetHandler()
    {
        return async (msg) =>
        {
            if (msg.Severity == LogSeverity.Critical || msg.Severity == LogSeverity.Error)
            {
                Console.WriteLine($"  \u274c [{msg.Severity}] {msg.Message}");
                if (msg.Exception != null)
                {
                    Console.WriteLine($"  {msg.Exception}");
                }
            }
            await Task.CompletedTask;
        };
    }
}
