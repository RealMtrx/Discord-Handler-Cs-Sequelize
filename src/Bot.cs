using Discord;
using Discord.WebSocket;
using DiscordHandler.Config;
using DiscordHandler.Models;

namespace DiscordHandler;

public class Bot
{
    public DiscordSocketClient Client { get; }
    public BotConfig Config => BotConfig.Instance;
    public Dictionary<string, SlashCommand> SlashCommands { get; } = [];
    public Dictionary<string, PrefixCommand> PrefixCommands { get; } = [];

    public Bot()
    {
        var config = new DiscordSocketConfig
        {
            GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.MessageContent | GatewayIntents.GuildMembers
        };

        Client = new DiscordSocketClient(config);
    }

    public async Task StartAsync()
    {
        await Client.LoginAsync(TokenType.Bot, Config.Token);
        await Client.StartAsync();
    }

    public async Task StopAsync()
    {
        await Client.StopAsync();
        await Client.LogoutAsync();
    }
}
