namespace NetChatGPT.DALLE.OpenAiServices
{
    public record class GenerateImageRequest(string Prompt, int N, string Size);

    public record class GenerateImageResponse(long Created, GeneratedImageData[] Data);

    public record class GeneratedImageData(string Url);
}
