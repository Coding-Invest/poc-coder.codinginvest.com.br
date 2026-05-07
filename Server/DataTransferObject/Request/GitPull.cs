using Domain;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace Server.DataTransferObject.Request
{
    public class GitPull
    {
        public GitPull(ProtocolRequest protocol, IConfiguration configuration)
        {
            TargetDirectory = configuration.GetSection("Git:TargetDirectory")?.Value ?? string.Empty;

            if (protocol.Params != null && protocol.Params.Length > 0)
            {
                var jsonData = protocol.Params[0].ToString();
                var args = JsonConvert.DeserializeObject<JObject>(jsonData);
                var repositoryUrlParam = args["repositoryUrl"]?.ToString() ?? configuration.GetConnectionString("Git:RepositoryUrl");
                var directoryParam = args["directory"]?.ToString();

                if (!string.IsNullOrEmpty(repositoryUrlParam))
                {
                    RepositoryUrl = repositoryUrlParam;
                }
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
        public string RepositoryUrl { get; set; }
        public string TargetDirectory { get; set; }
    }
}