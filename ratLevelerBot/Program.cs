﻿using RatLevelerBot;
using RatLevelerBot.Models;
using RatLevelerBot.Options;
using RatLevelerBot.Services;
using RatLevelerBot.Services.Repositories;
using RatLevelerBot.Controllers;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot;

var builder = WebApplication.CreateBuilder(args);

// Setup Bot configuration
var botConfigurationSection = builder.Configuration.GetSection(BotConfiguration.Configuration);
builder.Services.Configure<BotConfiguration>(botConfigurationSection);

var botConfiguration = botConfigurationSection.Get<BotConfiguration>();

if (botConfiguration == null) {
    throw new NullReferenceException(nameof(BotConfiguration));
}

builder.Services.AddHttpClient("telegram_bot_client")
    .AddTypedClient<ITelegramBotClient>((httpClient, sp) =>
    {
        BotConfiguration? botConfig = sp.GetConfiguration<BotConfiguration>();
        TelegramBotClientOptions options = new(botConfig.BotToken);
        return new TelegramBotClient(options, httpClient);
    });

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add repositories
builder.Services.AddScoped<IRepository<User>, UserRepository>();
builder.Services.AddScoped<IRepository<Level>, LevelRepository>();
builder.Services.AddScoped<IRepository<Chat>, ChatRepository>();
builder.Services.AddScoped<IRepository<UserLevel>, UserLevelRepository>();

builder.Services.AddScoped<IRatLevelerService, RatLevelerService>();

builder.Services.AddScoped<IBotCommandExecuter, BotCommandExecuter>();
builder.Services.AddScoped<RatMessageHandler>();

builder.Services.AddHostedService<ConfigureWebhook>();

builder.Services
    .AddControllers()
    .AddNewtonsoftJson();

var app = builder.Build();

app.MapBotWebhookRoute<BotController>(route: botConfiguration.Route);
app.MapControllers();
app.Run();