using Discord;
using Discord.WebSocket;
using DiscordHandler.Models;

namespace DiscordHandler.Commands.Slash.Public;

public static class Ping
{
    public static SlashCommand GetCommand()
    {
        var builder = new SlashCommandBuilder()
            .WithName("ping")
            .WithDescription("\ud83c\udfd3 Show bot latency");

        var props = builder.Build();

        return new SlashCommand
        {
            Name = "ping",
            Data = props,
            Category = "public",
            Handler = async (slash) =>
            {
                var latency = slash.Client.Latency;

                var embed = new EmbedBuilder()
                    .WithTitle("\ud83c\udfd3 Pong!")
                    .WithDescription($"> **WebSocket Latency:** `{latency}ms`\n> **API Latency:** `{latency}ms`")
                    .WithColor(Color.Blue)
                    .WithFooter($"{slash.Client.CurrentUser?.Username} \u2022 Ping")
                    .Build();

                await slash.RespondAsync(embed: embed);
            }
        };
    }
}
