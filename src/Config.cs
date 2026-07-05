namespace DiscordHandler.Config;

public class BotConfig
{
    public static BotConfig Instance { get; private set; } = new();

    public string Token { get; private set; } = "#";
    public string ClientId { get; private set; } = "#";
    public string BotName { get; private set; } = "Discord Handler";
    public string Prefix { get; private set; } = "$";
    public List<string> OwnerIds { get; private set; } = [];
    public string ErrorWebhook { get; private set; } = "#";
    public string SlashCommandWebhook { get; private set; } = "#";
    public string PrefixCommandWebhook { get; private set; } = "#";
    public string JoinGuildWebhook { get; private set; } = "#";
    public string LeaveGuildWebhook { get; private set; } = "#";
    public string ReadyWebhook { get; private set; } = "#";

    public string DbDialect { get; private set; } = "sqlite";
    public string DbStorage { get; private set; } = "discord_bot.db";
    public string DbHost { get; private set; } = "localhost";
    public string DbPort { get; private set; } = "5432";
    public string DbUsername { get; private set; } = "postgres";
    public string DbPassword { get; private set; } = "password";
    public string DbDatabase { get; private set; } = "discord_bot";

    public bool ErrorWebhookEnabled => ErrorWebhook != "#" && !string.IsNullOrEmpty(ErrorWebhook);
    public bool SlashCommandWebhookEnabled => SlashCommandWebhook != "#" && !string.IsNullOrEmpty(SlashCommandWebhook);
    public bool PrefixCommandWebhookEnabled => PrefixCommandWebhook != "#" && !string.IsNullOrEmpty(PrefixCommandWebhook);
    public bool JoinGuildWebhookEnabled => JoinGuildWebhook != "#" && !string.IsNullOrEmpty(JoinGuildWebhook);
    public bool LeaveGuildWebhookEnabled => LeaveGuildWebhook != "#" && !string.IsNullOrEmpty(LeaveGuildWebhook);
    public bool ReadyWebhookEnabled => ReadyWebhook != "#" && !string.IsNullOrEmpty(ReadyWebhook);

    public static void Load()
    {
        var config = new BotConfig
        {
            Token = Env("TOKEN", "#"),
            ClientId = Env("CLIENT_ID", "#"),
            BotName = Env("BOT_NAME", "Discord Handler"),
            Prefix = Env("PREFIX", "$"),
            OwnerIds = (Env("OWNER_IDS", "#,#") ?? "#,#").Split(',').Select(x => x.Trim()).ToList(),
            ErrorWebhook = Env("ERROR_WEBHOOK", "#"),
            SlashCommandWebhook = Env("SLASH_WEBHOOK", "#"),
            PrefixCommandWebhook = Env("PREFIX_WEBHOOK", "#"),
            JoinGuildWebhook = Env("JOIN_WEBHOOK", "#"),
            LeaveGuildWebhook = Env("LEAVE_WEBHOOK", "#"),
            ReadyWebhook = Env("READY_WEBHOOK", "#"),
            DbDialect = Env("DB_DIALECT", "sqlite"),
            DbStorage = Env("DB_STORAGE", "discord_bot.db"),
            DbHost = Env("DB_HOST", "localhost"),
            DbPort = Env("DB_PORT", "5432"),
            DbUsername = Env("DB_USERNAME", "postgres"),
            DbPassword = Env("DB_PASSWORD", "password"),
            DbDatabase = Env("DB_DATABASE", "discord_bot"),
        };

        Instance = config;
    }

    private static string Env(string key, string fallback)
    {
        return Environment.GetEnvironmentVariable(key) ?? fallback;
    }
}
