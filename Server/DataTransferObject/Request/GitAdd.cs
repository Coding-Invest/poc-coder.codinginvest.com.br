using Domain;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;

namespace Server.DataTransferObject.Request
{
    public class GitAdd
    {
        public GitAdd(ProtocolRequest protocol, IConfiguration configuration)
        {
            WorkDirectory = configuration.GetSection("Git:TargetDirectory")?.Value ?? Tools.WORKING_DIRECTORY;
            Path = "."; // default
            if (protocol.Params != null && protocol.Params.Length > 0)
            {
                var jsonData = protocol.Params[0].ToString();
                var args = JsonConvert.DeserializeObject<JObject>(jsonData);
                var pathParam = args["path"]?.ToString();
                if (!string.IsNullOrEmpty(pathParam))
                {
                    Path = pathParam;
                }
            }
        }
        public string Path { get; set; }
        public string WorkDirectory { get; set; }
    }
}