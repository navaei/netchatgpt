using System.Text.Json.Serialization;

namespace NetChatGPT.Common
{
    public class ChatGPTUsage
    {
        [JsonPropertyName("prompt_tokens")]
        public int PromptTokens
        {
            get;
            set;
        }

        [JsonPropertyName("completion_token")]
        public int CompletionTokens
        {
            get;
            set;
        }

        [JsonPropertyName("total_tokens")]
        public int TotalTokens
        {
            get;
            set;
        }
    }
}