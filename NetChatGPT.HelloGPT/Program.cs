using System.Text.Json;
using System.Text;
using NetChatGPT.Common;

var chatGptApiKey = "YOUR-CHATGPT-API-KEY";
Console.WriteLine("Hello! Please Type Your Prompt:");
var prompt = Console.ReadLine();

var httpReq = new HttpRequestMessage(HttpMethod.Post, "https://api.openai.com/v1/engines/text-davinci-003/completions");
httpReq.Headers.Add("Authorization", $"Bearer {chatGptApiKey}");
httpReq.Content = new StringContent($"{{ \"prompt\": \"{prompt}\", \"max_tokens\": 120 }}", Encoding.UTF8, "application/json");
var httpResponse = await new HttpClient().SendAsync(httpReq);
var responseString = await httpResponse.Content.ReadAsStringAsync();
var response = JsonSerializer.Deserialize<ChatGPTResponse>(responseString);
Console.WriteLine(response.Choices.FirstOrDefault()?.Text);

