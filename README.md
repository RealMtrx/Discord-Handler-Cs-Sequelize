# Discord Handler C# (Sequelize / EF Core)

A modern, feature-rich Discord bot handler built with **Discord.Net v3** and **Entity Framework Core**, supporting SQLite, PostgreSQL, and MySQL.

## 🚀 Features

- **Dual Command System**: Support for both slash commands and prefix commands
- **Modular Architecture**: Clean separation of concerns with dedicated handlers
- **Anti-Crash System**: Comprehensive error handling and monitoring
- **Event-Driven**: Fully event-driven async architecture
- **Webhook Logging**: Real-time logging for errors and guild events
- **EF Core Database**: SQLite / PostgreSQL / MySQL via Entity Framework Core
- **Cooldown System**: Per-command cooldown management
- **Environment Configuration**: Secure configuration with DotNetEnv

## 📁 Project Structure

```
Discord-Handler-Cs-Sequelize/
├── DiscordHandler.csproj          # .NET project file
├── src/                           # Source code
│   ├── Program.cs                 # Main bot entry point
│   ├── Config.cs                  # Bot configuration from .env
│   ├── Bot.cs                     # Bot initialization
│   ├── Core/                      # Core utilities
│   │   ├── CommandUtils.cs        # Cooldown and utilities
│   │   ├── Emojis.cs              # Centralized emoji definitions
│   │   └── Webhooks.cs            # Webhook types and sender
│   ├── Database/
│   │   └── DbContext.cs           # EF Core DbContext setup
│   ├── Events/                    # Discord event handlers
│   │   ├── GuildCreateHandler.cs  # Handler when bot joins a server
│   │   ├── GuildDeleteHandler.cs  # Handler when bot leaves a server
│   │   ├── InteractionCreateHandler.cs # Handles slash command interactions
│   │   ├── MessageCreateHandler.cs     # Handles prefix commands
│   │   └── ReadyHandler.cs        # Bot ready event
│   ├── Handlers/                  # Handlers for modularity
│   │   ├── AntiCrash.cs           # Crash prevention and error handling
│   │   └── Logger.cs              # Logger for bot activity
│   ├── Models/
│   │   └── User.cs                # User data model (EF Core entity)
│   └── Commands/
│       ├── Prefix/                # Prefix commands
│       │   └── Public/
│       │       └── Ping.cs
│       └── Slash/                 # Slash commands
│           └── Public/
│               └── Ping.cs
```

## 🔧 Installation

1. **Clone the repository**

   ```bash
   git clone <repo-url>
   cd Discord-Handler-Cs-Sequelize
   ```

2. **Restore dependencies**

   ```bash
   dotnet restore
   ```

3. **Environment Setup**

   Copy `.env.example` to `.env` and fill in your values:

   ```env
   TOKEN=your_bot_token_here
   PREFIX=!
   BOT_NAME=Discord Handler
   DB_DIALECT=sqlite
   DB_STORAGE=discord_bot.db
   ERROR_WEBHOOK=https://discord.com/api/webhooks/your_webhook
   GUILD_LOG_WEBHOOK=https://discord.com/api/webhooks/your_webhook
   ```

4. **Run the bot**

   ```bash
   dotnet run
   ```

## 📋 Dependencies

- **Discord.Net**: v3.13 - Discord API wrapper
- **Entity Framework Core**: v8 - Database ORM
- **EF Core SQLite**: v8 - SQLite provider
- **EF Core Npgsql**: v8 - PostgreSQL provider
- **EF Core Pomelo (MySQL)**: v8 - MySQL/MariaDB provider
- **DotNetEnv**: v2.5 - Environment variable management

## 📝 Database Configuration

Set `DB_DIALECT` to one of:

- **`sqlite`** — uses `DB_STORAGE` as the file path
- **`postgres`** — uses `DB_HOST`, `DB_PORT`, `DB_USERNAME`, `DB_PASSWORD`, `DB_DATABASE`
- **`mysql`** — uses `DB_HOST`, `DB_PORT`, `DB_USERNAME`, `DB_PASSWORD`, `DB_DATABASE`

---

**Discord Handler** — Built by **Mtrx** — Discord: **0hu2**
