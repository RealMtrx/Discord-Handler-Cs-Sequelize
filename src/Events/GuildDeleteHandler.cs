using Discord.WebSocket;
using DiscordHandler.Core;

namespace DiscordHandler.Events;

public static class GuildDeleteHandler
{
    public static Func<SocketGuild, Task> GetHandler(Func<int> getRemainingCount)
    {
        return async (guild) =>
        {
            try
            {
                Console.WriteLine($"  \ud83d\udc4b Left guild: {guild.Name} ({guild.Id})");
                await LeaveGuildWebhook.SendLeaveGuildWebhook(
                    guildId: guild.Id.ToString(),
                    guildName: guild.Name,
                    memberCount: guild.MemberCount,
                    remainingServers: getRemainingCount()
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  \u274c Error in guild remove event: {ex.Message}");
            }
        };
    }
}
