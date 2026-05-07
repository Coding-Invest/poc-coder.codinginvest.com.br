using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Domain;

namespace Server.DataTransferObject.Request
{
    public class FileDelete
    {
        public string Path { get; set; }
        public FileDelete(ProtocolRequest protocol)
        {
            if (protocol.Params == null || protocol.Params.Length==0)
            {
                throw new Exception("Params cannot be null or zero");
            }

            var jsonData = protocol.Params[0].ToString();
            Path = (string)JsonConvert.DeserializeObject<JObject>(jsonData)["path"];
        }
    }
}
