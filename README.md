<div align="center">
  <h1>Discord Handler — C# (SQL Edition)</h1>
  <p><strong>A production-ready Discord bot framework built with Discord.Net v3 and Entity Framework Core — supports SQLite, PostgreSQL, and MySQL with a modular src/ architecture.</strong></p>

  <p>
    <a href="https://github.com/RealMtrx/Discord-Handler-Cs-Sequelize/blob/main/LICENSE"><img src="https://img.shields.io/badge/license-MIT-blue.svg" alt="License"></a>
    <a href="https://github.com/RealMtrx/Discord-Handler-Cs-Sequelize/releases"><img src="https://img.shields.io/badge/version-0.9.0--beta-yellow" alt="Version 0.9.0 Beta"></a>
    <a href="https://github.com/RealMtrx/Discord-Handler-Cs-Sequelize/stargazers"><img src="https://img.shields.io/github/stars/RealMtrx/Discord-Handler-Cs-Sequelize" alt="Stars"></a>
    <a href="https://github.com/RealMtrx/Discord-Handler-Cs-Sequelize/issues"><img src="https://img.shields.io/github/issues/RealMtrx/Discord-Handler-Cs-Sequelize" alt="Issues"></a>
    <a href="https://github.com/RealMtrx/Discord-Handler-Cs-Sequelize/network"><img src="https://img.shields.io/github/forks/RealMtrx/Discord-Handler-Cs-Sequelize" alt="Forks"></a>
    <a href="https://github.com/RealMtrx/Discord-Handler/graphs/contributors"><img src="https://img.shields.io/badge/ecosystem-26%20repos-brightgreen" alt="26 Repos"></a>
    <a href="https://discord.gg/0hu2"><img src="https://img.shields.io/badge/discord-0hu2-5865F2" alt="Discord"></a>
  </p>

  <br>

  <p>
    <a href="#-features">Features</a> •
    <a href="#-quick-start">Quick Start</a> •
    <a href="#-project-structure">Structure</a> •
    <a href="#-database-configuration">Database Config</a> •
    <a href="#-api-reference">API</a> •
    <a href="#-mongodb-edition">MongoDB Edition</a> •
    <a href="#-related-repositories">Ecosystem</a>
  </p>
</div>

---

## Overview

Discord Handler C# (SQL Edition) is the **C# Sequelize variant** of the multi-language Discord Handler ecosystem. Built on **Discord.Net v3** with **Entity Framework Core**, it provides an asynchronous, event-driven foundation for Discord bots with dual command support (slash + prefix), relational database persistence, webhook-based logging, and a comprehensive anti-crash layer.

The entry point (`Program.cs`) boots in a predictable async sequence: initialize the anti-crash handler, configure the database context, load slash commands, prefix commands, events, and EF Core models, present a startup report, and finally log in. A graceful shutdown handler for `ProcessExit` / `SIGTERM` is also registered.

## Features

- **Dual Command System** — Slash commands via Discord.Net's interaction framework and prefix commands via `MessageReceived`
- **Modular Architecture** — Separated concerns across `Core/`, `Database/`, `Events/`, `Handlers/`, `Models/`, and `Commands/`
- **.NET 8** — Built on modern .NET with nullable reference types and implicit usings
- **Anti-Crash** — Global `AppDomain.CurrentDomain.UnhandledException` and `TaskScheduler.UnobservedTaskException` interception via `AntiCrash.cs`
- **Webhook Logging** — Six dedicated webhooks: errors, slash commands, prefix commands, guild join, guild leave, and ready
- **EF Core Database** — Persistent storage via Entity Framework Core supporting SQLite, PostgreSQL, and MySQL
- **Cooldown System** — Per-command cooldown tracked in `Core/CommandUtils.cs`
- **Environment Configuration** — All secrets managed via `DotNetEnv` in `Config.cs`
- **Async-Await** — Fully asynchronous architecture with `async`/`await` throughout

## Quick Start

```bash
git clone https://github.com/RealMtrx/Discord-Handler-Cs-Sequelize.git
cd Discord-Handler-Cs-Sequelize
dotnet restore
```

Copy `.env.example` to `.env` and fill in your values:

```env
TOKEN=your_bot_token
CLIENT_ID=your_client_id
BOT_NAME=Discord Handler
OWNER_IDS=owner_id_1,owner_id_2
PREFIX=$
DB_DIALECT=sqlite
DB_STORAGE=discord_bot.db
ERROR_WEBHOOK=your_webhook_url
SLASH_WEBHOOK=your_webhook_url
PREFIX_WEBHOOK=your_webhook_url
JOIN_WEBHOOK=your_webhook_url
LEAVE_WEBHOOK=your_webhook_url
READY_WEBHOOK=your_webhook_url
```

```bash
dotnet run
```

### Dependencies

| Package | Version | Purpose |
|---------|---------|---------|
| `Discord.Net` | 3.16.\* | Discord API wrapper |
| `Microsoft.EntityFrameworkCore` | 8.\* | EF Core ORM |
| `Microsoft.EntityFrameworkCore.Sqlite` | 8.\* | SQLite provider |
| `Npgsql.EntityFrameworkCore.PostgreSQL` | 8.\* | PostgreSQL provider |
| `Pomelo.EntityFrameworkCore.MySql` | 8.\* | MySQL/MariaDB provider |
| `DotNetEnv` | 2.\* | Environment variable management |

## Project Structure

```
Discord-Handler-Cs-Sequelize/
├── DiscordHandler.csproj          # .NET project file
├── src/
│   ├── Program.cs                 # Entry point — async boot sequence
│   ├── Config.cs                  # Bot configuration (token, prefixes, webhooks)
│   ├── Bot.cs                     # Bot initialization and client setup
│   ├── Core/
│   │   ├── CommandUtils.cs        # Cooldown and utility helpers
│   │   ├── Emojis.cs              # Centralized emoji definitions
│   │   └── Webhooks.cs            # Webhook types and sender
│   ├── Database/
│   │   └── DbContext.cs           # EF Core DbContext setup
│   ├── Events/
│   │   ├── GuildCreateHandler.cs   # Handler when bot joins a server
│   │   ├── GuildDeleteHandler.cs   # Handler when bot leaves a server
│   │   ├── InteractionCreateHandler.cs # Handles slash command interactions
│   │   ├── MessageCreateHandler.cs     # Handles prefix commands
│   │   └── ReadyHandler.cs        # Bot ready event
│   ├── Handlers/
│   │   ├── AntiCrash.cs           # Global error interception
│   │   └── Logger.cs              # Logger for bot activity
│   ├── Models/
│   │   └── User.cs                # User data model (EF Core entity)
│   └── Commands/
│       ├── Prefix/Public/Ping.cs  # Example prefix command
│       └── Slash/Public/Ping.cs   # Example slash command
```

## Database Configuration

Set `DB_DIALECT` to one of the following:

| Dialect | Required Environment Variables |
|---------|-------------------------------|
| `sqlite` | `DB_STORAGE` — file path (e.g., `discord_bot.db`) |
| `postgres` | `DB_HOST`, `DB_PORT`, `DB_USERNAME`, `DB_PASSWORD`, `DB_DATABASE` |
| `mysql` | `DB_HOST`, `DB_PORT`, `DB_USERNAME`, `DB_PASSWORD`, `DB_DATABASE` |

The `DbContext.cs` file dynamically selects the appropriate EF Core provider based on `DB_DIALECT`:

```csharp
public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        var dialect = Config.Get("DB_DIALECT");
        switch (dialect)
        {
            case "sqlite":
                options.UseSqlite($"Data Source={Config.Get("DB_STORAGE")}");
                break;
            case "postgres":
                options.UseNpgsql($"Host={Config.Get("DB_HOST")};Database={Config.Get("DB_DATABASE")};Username={Config.Get("DB_USERNAME")};Password={Config.Get("DB_PASSWORD")}");
                break;
            case "mysql":
                options.UseMySql($"Server={Config.Get("DB_HOST")};Database={Config.Get("DB_DATABASE")};User={Config.Get("DB_USERNAME")};Password={Config.Get("DB_PASSWORD")};", ServerVersion.AutoDetect);
                break;
        }
    }
}
```

## API Reference

### Entry Point — `src/Program.cs`

```csharp
static async Task Main(string[] args)
```

Creates a `DiscordSocketClient` with `GatewayIntents.Guilds`, `GatewayIntents.GuildMessages`, and `GatewayIntents.MessageContent`. Loads handlers sequentially: AntiCrash → DbContext → slash commands → prefix commands → events → models. Logs in via `client.LoginAsync(TokenType.Bot, config.Token)`. Handles `ProcessExit` and `SIGTERM` for graceful shutdown.

### Configuration — `src/Config.cs`

| Key | Type | Description |
|-----|------|-------------|
| `Token` | `string` | Discord bot token |
| `ClientId` | `string` | Discord application client ID |
| `BotName` | `string` | Display name for startup report |
| `Prefix` | `string` | Prefix for text commands |
| `OwnerIds` | `string[]` | Array of bot owner Discord IDs |
| `DbDialect` | `string` | Database dialect (`sqlite`, `postgres`, `mysql`) |
| `ErrorWebhook` | `string` | Webhook URL for error reports |
| `SlashWebhook` | `string` | Webhook URL for slash command usage |
| `PrefixWebhook` | `string` | Webhook URL for prefix command usage |
| `JoinWebhook` | `string` | Webhook URL for guild joins |
| `LeaveWebhook` | `string` | Webhook URL for guild leaves |
| `ReadyWebhook` | `string` | Webhook URL for ready notifications |

### Events

| Event | File | Trigger |
|-------|------|---------|
| `Ready` | `Events/ReadyHandler.cs` | Bot goes online — logs startup, sends ready webhook |
| `GuildAvailable` / `JoinedGuild` | `Events/GuildCreateHandler.cs` | Bot joins a server — sends join webhook |
| `GuildDeleted` | `Events/GuildDeleteHandler.cs` | Bot leaves a server — sends leave webhook |
| `SlashCommandExecuted` | `Events/InteractionCreateHandler.cs` | Slash command used — routes to command module |
| `MessageReceived` | `Events/MessageCreateHandler.cs` | Message sent — checks prefix, routes to prefix command |

### Handlers

- **AntiCrash** — Registers `AppDomain.CurrentDomain.UnhandledException` and `TaskScheduler.UnobservedTaskException` reporters
- **Logger** — Writes a startup report with command/event counts, database status, and anti-crash status

### Core Utilities

- **CommandUtils** — `CheckCooldown(client, commandName, userId)` and error formatting helpers
- **Emojis** — Centralized `Emoji` definitions for consistent bot responses
- **Webhooks** — `WebhookSender` with `SendError`, `SendSlash`, `SendPrefix`, `SendJoin`, `SendLeave`, `SendReady` methods

## Adding Commands

### Slash Command

Create `src/Commands/Slash/[Category]/[name].cs`:

```csharp
using Discord.Interactions;

public class PingCommand : InteractionModuleBase<SocketInteractionContext>
{
    [SlashCommand("ping", "Replies with Pong!")]
    public async Task Execute()
    {
        await RespondAsync("Pong! 🏓");
    }
}
```

### Prefix Command

Create `src/Commands/Prefix/[Category]/[name].cs`:

```csharp
using Discord.Commands;

public class PingCommand : ModuleBase<SocketCommandContext>
{
    [Command("ping")]
    [Summary("Replies with Pong!")]
    public async Task Execute()
    {
        await ReplyAsync("Pong! 🏓");
    }
}
```

The `CommandHandler` and `PrefixHandler` services automatically discover and register new modules on the next restart. No manual wiring is needed.

## MongoDB Edition

A **MongoDB** variant of this handler is available for teams that prefer a document database:

[RealMtrx/Discord-Handler-Cs](https://github.com/RealMtrx/Discord-Handler-Cs)

It replaces `Database/DbContext.cs` with a MongoDB driver connection (via `MongoDB.Driver`) and swaps EF Core entities for BSON documents. All other modules — events, commands, handlers, core utilities — remain identical.

## Related Repositories

The Discord Handler ecosystem spans **26 repositories** across 13 languages, each available in both MongoDB and Sequelize editions.

### Base Repositories (MongoDB)

| Language | Repository |
|----------|------------|
| C++ | [RealMtrx/Discord-Handler-Cpp](https://github.com/RealMtrx/Discord-Handler-Cpp) |
| C# | [RealMtrx/Discord-Handler-Cs](https://github.com/RealMtrx/Discord-Handler-Cs) |
| Dart | [RealMtrx/Discord-Handler-Dart](https://github.com/RealMtrx/Discord-Handler-Dart) |
| Go | [RealMtrx/Discord-Handler-Go](https://github.com/RealMtrx/Discord-Handler-Go) |
| Java | [RealMtrx/Discord-Handler-Java](https://github.com/RealMtrx/Discord-Handler-Java) |
| JavaScript | [RealMtrx/Discord-Handler-Js](https://github.com/RealMtrx/Discord-Handler-Js) |
| Kotlin | [RealMtrx/Discord-Handler-Kt](https://github.com/RealMtrx/Discord-Handler-Kt) |
| Lua | [RealMtrx/Discord-Handler-Lua](https://github.com/RealMtrx/Discord-Handler-Lua) |
| PHP | [RealMtrx/Discord-Handler-Php](https://github.com/RealMtrx/Discord-Handler-Php) |
| Python | [RealMtrx/Discord-Handler-Py](https://github.com/RealMtrx/Discord-Handler-Py) |
| Ruby | [RealMtrx/Discord-Handler-Rb](https://github.com/RealMtrx/Discord-Handler-Rb) |
| Rust | [RealMtrx/Discord-Handler-Rs](https://github.com/RealMtrx/Discord-Handler-Rs) |
| TypeScript | [RealMtrx/Discord-Handler](https://github.com/RealMtrx/Discord-Handler) ← hub |

### Sequelize (SQL) Editions

| Language | Repository |
|----------|------------|
| C++ | [RealMtrx/Discord-Handler-Cpp-Sequelize](https://github.com/RealMtrx/Discord-Handler-Cpp-Sequelize) |
| C# | [RealMtrx/Discord-Handler-Cs-Sequelize](https://github.com/RealMtrx/Discord-Handler-Cs-Sequelize) |
| Dart | [RealMtrx/Discord-Handler-Dart-Sequelize](https://github.com/RealMtrx/Discord-Handler-Dart-Sequelize) |
| Go | [RealMtrx/Discord-Handler-Go-Sequelize](https://github.com/RealMtrx/Discord-Handler-Go-Sequelize) |
| Java | [RealMtrx/Discord-Handler-Java-Sequelize](https://github.com/RealMtrx/Discord-Handler-Java-Sequelize) |
| JavaScript | [RealMtrx/Discord-Handler-Js-Sequelize](https://github.com/RealMtrx/Discord-Handler-Js-Sequelize) |
| Kotlin | [RealMtrx/Discord-Handler-Kt-Sequelize](https://github.com/RealMtrx/Discord-Handler-Kt-Sequelize) |
| Lua | [RealMtrx/Discord-Handler-Lua-Sequelize](https://github.com/RealMtrx/Discord-Handler-Lua-Sequelize) |
| PHP | [RealMtrx/Discord-Handler-Php-Sequelize](https://github.com/RealMtrx/Discord-Handler-Php-Sequelize) |
| Python | [RealMtrx/Discord-Handler-Py-Sequelize](https://github.com/RealMtrx/Discord-Handler-Py-Sequelize) |
| Ruby | [RealMtrx/Discord-Handler-Rb-Sequelize](https://github.com/RealMtrx/Discord-Handler-Rb-Sequelize) |
| Rust | [RealMtrx/Discord-Handler-Rs-Sequelize](https://github.com/RealMtrx/Discord-Handler-Rs-Sequelize) |
| TypeScript | [RealMtrx/Discord-Handler-Ts-Sequelize](https://github.com/RealMtrx/Discord-Handler-Ts-Sequelize) |

> **[RealMtrx/Discord-Handler](https://github.com/RealMtrx/Discord-Handler)** — the TypeScript hub and flagship repository. Star it to support the ecosystem.

## License

Distributed under the MIT License. See `LICENSE` for more information.

---

Built by **Mtrx** — Discord: **0hu2** — [RealMtrx/Discord-Handler](https://github.com/RealMtrx/Discord-Handler)
