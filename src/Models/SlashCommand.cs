using Discord;

namespace DiscordHandler.Models;

public class SlashCommand
{
    public required string Name { get; set; }
    public required SlashCommandProperties Data { get; set; }
    public string Category { get; set; } = "public";
    public required Func<SocketSlashCommand, Task> Handler { get; set; }
}
