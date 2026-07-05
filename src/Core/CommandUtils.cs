namespace DiscordHandler.Core;

public class ErrorReport
{
    public string Message { get; set; } = string.Empty;
    public string CommandName { get; set; } = string.Empty;
    public string Timestamp { get; set; } = DateTime.UtcNow.ToString("o");
}

public static class CommandUtils
{
    public static ErrorReport FormatError(Exception ex, string commandName)
    {
        return new ErrorReport
        {
            Message = ex.Message,
            CommandName = commandName,
            Timestamp = DateTime.UtcNow.ToString("o")
        };
    }

    public static void LogCommandUsage(string userId, string userName, string commandName, string guildName)
    {
        Console.WriteLine($"[Command] {userName} ({userId}) used {commandName} in {guildName}");
    }
}
