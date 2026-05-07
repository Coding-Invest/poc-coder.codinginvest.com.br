using Domain;
using Newtonsoft.Json;

namespace Client.Controllers
{
    public partial class ChatbotController
    {
        public class CompletionRequest
        {
            [JsonProperty("model")]
            public string Model { get; set; } = "grok-code-fast-1";
            [JsonProperty("messages")]
            public IList<Message> Messages { get; set; }
        }
    }
}
