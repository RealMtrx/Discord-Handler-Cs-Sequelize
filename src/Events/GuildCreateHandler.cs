using Discord.WebSocket;
using DiscordHandler.Core;

namespace DiscordHandler.Events;

public static class GuildCreateHandler
{
    public static Func<SocketGuild, Task> GetHandler()
    {
        return async (guild) =>
        {
            try
            {
                var icon = guild.IconUrl;
                Console.WriteLine($"  \ud83c\udfe5 Joined guild: {guild.Name} ({guild.Id})");
                await JoinGuildWebhook.SendJoinGuildWebhook(
                    guildName: guild.Name,
                    guildId: guild.Id.ToString(),
                    ownerId: guild.OwnerId.ToString(),
                    memberCount: guild.MemberCount,
                    iconUrl: icon
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  \u274c Error in guild join event: {ex.Message}");
            }
        };
    }
}
