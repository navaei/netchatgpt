using System.Text.Json.Serialization;

namespace NetChatGPT.Common
{
    public class ChatGPTRequest
    {
        [JsonPropertyName("model")]
        public string? Model
        {
            get;
            set;
        }

        [JsonPropertyName("prompt")]
        public string? Prompt
        {
            get;
            set;
        }

        [JsonPropertyName("max_tokens")]
        public int? MaxTokens
        {
            get;
            set;
        }

        //[JsonPropertyName("session_id")]
        //public string SessionId
        //{
        //    get;
        //    set;
        //}
    }
}