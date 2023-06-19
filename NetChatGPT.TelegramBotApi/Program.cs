using NetChatGPT.TelegramBotApi.Logic;
using System.Reflection;
using Telegram.Bot;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("appsettings.json", optional: false)
        .AddUserSecrets(Assembly.GetExecutingAssembly());

// Get configs from appsetings.
var botToken = builder.Configuration.GetValue<string>("Telegram:BotToken");
var appConfig = new AppConfig()
{
    OpenAiApiKey = builder.Configuration.GetValue<string>("OpenAi:ApiKey"),
    OpenAiUrl = builder.Configuration.GetValue<string>("OpenAi:Url"),
    SubscriptionId = builder.Configuration.GetValue<string>("OpenAi:SubscriptionId"),
};

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddSingleton(appConfig);
builder.Services.AddSingleton(new TelegramBotClient(botToken));
builder.Services.AddTransient<IMainApp, MainApp>();
var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();
app.MapControllers();
app.Run();
