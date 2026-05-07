using Domain;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Server.DataTransferObject.Request
{
    public class FileList
    {
        public FileList(ProtocolRequest protocol)
        {
            if (protocol.Params == null || protocol.Params.Length==0)
            {
                throw new Exception("Params cannot be null or zero");
            }

            var jsonData = protocol.Params[0].ToString();
            Directory = (string)JsonConvert.DeserializeObject<JObject>(jsonData)["path"];
        }
        public string Directory { get; set; }
    }
}
