using Domain;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;

namespace Server.DataTransferObject.Request
{
    public class GitClone
    {
        public GitClone(ProtocolRequest protocol, IConfiguration configuration)
        {
            var personalAccessToken = configuration.GetSection("Git:PersonalAccessToken")?.Value ?? string.Empty;
            TargetDirectory = configuration.GetSection("Git:TargetDirectory")?.Value ?? string.Empty;
            RepositoryUrl = string.Format(configuration.GetSection("Git:RepositoryUrl")?.Value ?? string.Empty, personalAccessToken);

            if (protocol.Params != null && protocol.Params.Length > 0)
            {
                var jsonData = protocol.Params[0].ToString();
                var args = JsonConvert.DeserializeObject<JObject>(jsonData);
                var repositoryUrlParam = args["repositoryUrl"]?.ToString();
                var directoryParam = args["directory"]?.ToString();

                if (!string.IsNullOrEmpty(repositoryUrlParam))
                {
                    RepositoryUrl = repositoryUrlParam;
                    if (RepositoryUrl.Contains("{0}"))
                    {
                        RepositoryUrl = string.Format(RepositoryUrl, personalAccessToken);
                    }
                }
                if (!string.IsNullOrEmpty(directoryParam))
                {
                    TargetDirectory = directoryParam;
                }
            }

            if (string.IsNullOrEmpty(RepositoryUrl))
            {
                throw new ArgumentNullException("RepositoryUrl", "RepositoryUrl cannot be null or empty");
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