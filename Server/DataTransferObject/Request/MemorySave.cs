using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Domain;

namespace Server.DataTransferObject.Request
{
    public class MemorySave
    {
        public string Content { get; set; }
        public string Filename { get; set; }
        public MemorySave(ProtocolRequest protocol)
        {
            if (protocol.Params == null || protocol.Params.Length == 0)
            {
                throw new Exception("Params cannot be null or zero");
            }

            var jsonData = protocol.Params[0].ToString();
            var obj = JsonConvert.DeserializeObject<JObject>(jsonData);
            Content = (string)obj["content"];
            Filename = (string)obj["filename"] ?? "chat_history.txt";
        }
    }
}
