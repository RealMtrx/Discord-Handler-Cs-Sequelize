using Discord;
using Discord.WebSocket;
using DiscordHandler.Models;

namespace DiscordHandler.Commands.Prefix.Public;

public static class Ping
{
    public static PrefixCommand GetCommand()
    {
        return new PrefixCommand
        {
            Name = "ping",
            Description = "\ud83c\udfd3 Show bot latency",
            Category = "public",
            Aliases = ["pong"],
            Handler = async (message, args) =>
            {
                var sent = await message.Channel.SendMessageAsync("Pinging...");

                var embed = new EmbedBuilder()
                    .WithTitle("\ud83c\udfd3 Pong!")
                    .WithDescription($"> **Message Latency:** `{message.Timestamp.ToUnixTimeMilliseconds()}ms`")
                    .WithColor(Color.Blue)
                    .WithFooter($"{message.Author.Username} \u2022 Ping")
                    .Build();

                await sent.ModifyAsync(msg => msg.Content = null);
                await sent.ModifyAsync(msg => msg.Embed = embed);
            }
        };
    }
}
