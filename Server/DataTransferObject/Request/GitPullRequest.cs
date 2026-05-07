using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Domain;

namespace Server.DataTransferObject.Request
{
    public class GitPullRequest
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public string Head { get; set; }
        public string Base { get; set; }
        public GitPullRequest(ProtocolRequest protocol)
        {
            if (protocol.Params == null)
            {
                Title = "Pull Request";
                Body = "Descrição do Pull Request";
                Head = null;
                Base = "master";
            }
            else
            {
                var jsonData = protocol.Params[0].ToString();
                var obj = JsonConvert.DeserializeObject<JObject>(jsonData);
                Title = obj["title"]?.ToString() ?? "Pull Request";
                Body = obj["body"]?.ToString() ?? "Descrição do Pull Request";
                Head = obj["head"]?.ToString();
                Base = obj["base"]?.ToString() ?? "master";
            }
        }
    }
}