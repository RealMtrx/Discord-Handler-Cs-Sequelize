namespace DiscordHandler.Models;

public class EventFile
{
    public required string Name { get; set; }
    public bool Once { get; set; }
    public required Delegate Handler { get; set; }
}
