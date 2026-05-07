using Domain;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;

namespace Server.DataTransferObject.Request
{
    public class GoogleSearch
    {
        public GoogleSearch(ProtocolRequest protocol, IConfiguration configuration)
        {
            Query = "";
            NumResults = 5; // default
            if (protocol.Params != null && protocol.Params.Length > 0)
            {
                var jsonData = protocol.Params[0].ToString();
                var args = JsonConvert.DeserializeObject<JObject>(jsonData);
                var queryParam = args["query"]?.ToString();
                if (!string.IsNullOrEmpty(queryParam))
                {
                    Query = queryParam;
                }
                var numResultsParam = args["numResults"]?.ToString();
                if (!string.IsNullOrEmpty(numResultsParam) && int.TryParse(numResultsParam, out int num))
                {
                    NumResults = num;
                }
            }
        }
        public string Query { get; set; }
        public int NumResults { get; set; }
    }
}