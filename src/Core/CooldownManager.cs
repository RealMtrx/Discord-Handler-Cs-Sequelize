using System.Collections.Concurrent;

namespace DiscordHandler.Core;

public class CooldownManager
{
    private readonly ConcurrentDictionary<string, ConcurrentDictionary<string, long>> _cooldowns = new();

    public (bool OnCooldown, int Remaining) Check(string userId, string command, int cooldownMs = 3000)
    {
        var now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

        var commandMap = _cooldowns.GetOrAdd(command, _ => new ConcurrentDictionary<string, long>());

        if (commandMap.TryGetValue(userId, out var expires) && now < expires)
        {
            return (true, (int)((expires - now) / 1000));
        }

        commandMap[userId] = now + cooldownMs;
        return (false, 0);
    }
}
