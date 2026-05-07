using Domain;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;

namespace Server.DataTransferObject.Request
{
    public class GitPush
    {
        public GitPush(ProtocolRequest protocol, IConfiguration configuration)
        {
            TargetDirectory = configuration.GetSection("Git:TargetDirectory")?.Value ?? string.Empty;

            if (protocol.Params != null && protocol.Params.Length > 0)
            {
                var jsonData = protocol.Params[0].ToString();
                var args = JsonConvert.DeserializeObject<JObject>(jsonData);
                var directoryParam = args["directory"]?.ToString();

                if (!string.IsNullOrEmpty(directoryParam))
                {
                    TargetDirectory = directoryParam;
                }
            }

            if (string.IsNullOrEmpty(TargetDirectory))
            {
                throw new ArgumentNullException("TargetDirectory", "TargetDirectory cannot be null or empty");
            }
        }
        public string TargetDirectory { get; set; }
    }
}