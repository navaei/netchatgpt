using Microsoft.Extensions.Configuration;
using NetChatGPT.DALLE.OpenAiServices;
using System.Reflection;

Console.WriteLine("Starting image generator by DALLE...");

var config = BuildConfig();
IOpenAIProxy aiClient = new OpenAiService(config);

Console.WriteLine("Type your first image Prompt");
var msg = Console.ReadLine();

do
{
    var nImages = int.Parse(config["OpenAi:DALLE:N"]);
    var imageSize = config["OpenAi:DALLE:Size"];
    var prompt = new GenerateImageRequest(msg, nImages, imageSize);

    var result = await aiClient.GenerateImages(prompt);

    foreach (var item in result.Data)
    {
        Console.WriteLine(item.Url);

        var fullPath = Path.Combine(Directory.GetCurrentDirectory(), $"{Guid.NewGuid()}.png");
        var img = await aiClient.DownloadImage(item.Url);

        await File.WriteAllBytesAsync(fullPath, img);
    }

    Console.WriteLine("Enter New Prompt:");
    msg = Console.ReadLine();
} while (msg != "q");


static IConfiguration BuildConfig()
{
    var dir = Directory.GetCurrentDirectory();
    var configBuilder = new ConfigurationBuilder()
        .AddJsonFile(Path.Combine(dir, "appsettings.json"), optional: false)
        .AddUserSecrets(Assembly.GetExecutingAssembly());

    return configBuilder.Build();
}
