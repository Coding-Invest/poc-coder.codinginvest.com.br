using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Domain;

namespace Server.DataTransferObject.Request
{
    public class MemoryLoad
    {
        public string Filename { get; set; }
        public MemoryLoad(ProtocolRequest protocol)
        {
            if (protocol.Params == null)
            {
                throw new Exception("Params cannot be null");
            }

            var jsonData = protocol.Params.Length>0? protocol.Params[0].ToString() : "{\"filename\":null}";
            var obj = JsonConvert.DeserializeObject<JObject>(jsonData);
            Filename = (string?)obj["filename"] ?? "chat_history.txt";
        }
    }
}
