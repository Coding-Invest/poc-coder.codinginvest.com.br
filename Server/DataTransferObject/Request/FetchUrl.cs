using Domain;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace Server.DataTransferObject.Request
{
    public class FetchUrl
    {
        public FetchUrl(ProtocolRequest protocol)
        {
            Url = ""; // default
            if (protocol.Params != null && protocol.Params.Length > 0)
            {
                var jsonData = protocol.Params[0].ToString();
                var args = JsonConvert.DeserializeObject<JObject>(jsonData);
                var urlParam = args["url"]?.ToString();
                if (!string.IsNullOrEmpty(urlParam))
                {
                    Url = urlParam;
                }
            }
        }
        public string Url { get; set; }
    }
}