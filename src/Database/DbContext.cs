using Microsoft.EntityFrameworkCore;
using DiscordHandler.Config;
using DiscordHandler.Models;

namespace DiscordHandler.Database;

public class AppDbContext : DbContext
{
    public DbSet<User> Users => Set<User>();

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        var cfg = BotConfig.Instance;

        switch (cfg.DbDialect.ToLower())
        {
            case "sqlite":
                options.UseSqlite($"Data Source={cfg.DbStorage}");
                break;
            case "postgres":
            case "postgresql":
            case "pgsql":
                var pgPort = int.TryParse(cfg.DbPort, out var p) ? p : 5432;
                options.UseNpgsql($"Host={cfg.DbHost};Port={pgPort};Database={cfg.DbDatabase};Username={cfg.DbUsername};Password={cfg.DbPassword}");
                break;
            case "mysql":
            case "mariadb":
                var myPort = int.TryParse(cfg.DbPort, out var m) ? m : 3306;
                var connStr = $"Server={cfg.DbHost};Port={myPort};Database={cfg.DbDatabase};User={cfg.DbUsername};Password={cfg.DbPassword};";
                options.UseMySql(connStr, ServerVersion.AutoDetect(connStr));
                break;
            default:
                throw new InvalidOperationException($"Unsupported DB_DIALECT: {cfg.DbDialect}. Use sqlite, postgres, or mysql.");
        }
    }
}
