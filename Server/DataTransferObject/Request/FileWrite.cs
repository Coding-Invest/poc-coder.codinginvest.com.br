using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Domain;

namespace Server.DataTransferObject.Request
{
    public class FileWrite
    {
        public string Path { get; set; }
        public string Content { get; set; }
        public FileWrite(ProtocolRequest protocol)
        {
            if (protocol.Params == null || protocol.Params.Length==0)
            {
                throw new Exception("Params cannot be null or zero");
            }

            var jsonData = protocol.Params[0].ToString();
            var jsonObject = JsonConvert.DeserializeObject<JObject>(jsonData);
            Path = (string)jsonObject["path"];
            Content = (string)jsonObject["content"];
        }
    }
}
