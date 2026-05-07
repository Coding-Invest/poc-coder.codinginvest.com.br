using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Domain;

namespace Server.DataTransferObject.Request
{
    public class NpmInstall
    {
        public string Path { get; set; }

        public NpmInstall(ProtocolRequest protocol)
        {
            if (protocol.Params == null || protocol.Params.Length == 0)
            {
                throw new Exception("Params cannot be null or zero");
            }
            var jsonData = protocol.Params[0].ToString();
            var jObject = JsonConvert.DeserializeObject<JObject>(jsonData);
            Path = (string)jObject["path"];
        }
    }
}