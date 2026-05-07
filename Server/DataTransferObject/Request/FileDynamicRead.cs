using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Domain;

namespace Server.DataTransferObject.Request
{
    public class FileDynamicRead
    {
        public string Path { get; set; }
        public int Index { get; set; }
        public int Length { get; set; }
        public FileDynamicRead(ProtocolRequest protocol)
        {
            if (protocol.Params == null || protocol.Params.Length==0)
            {
                throw new Exception("Params cannot be null or zero");
            }

            var jsonData = protocol.Params[0].ToString();
            Path = (string)JsonConvert.DeserializeObject<JObject>(jsonData)["path"];
            Index = (int)JsonConvert.DeserializeObject<JObject>(jsonData)["index"];
            Length = (int)JsonConvert.DeserializeObject<JObject>(jsonData)["length"];
        }
    }
}