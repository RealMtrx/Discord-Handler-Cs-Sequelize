using Discord;

namespace DiscordHandler.Models;

public class PrefixCommand
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public string Category { get; set; } = "public";
    public List<string> Aliases { get; set; } = [];
    public required Func<SocketMessage, string[], Task> Handler { get; set; }
}
