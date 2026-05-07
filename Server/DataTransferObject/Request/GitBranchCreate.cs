using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Domain;

namespace Server.DataTransferObject.Request
{
    public class GitBranchCreate
    {
        public string BranchName { get; set; }
        public GitBranchCreate(ProtocolRequest protocol)
        {
            if (protocol.Params == null || protocol.Params.Length==0)
            {
                throw new Exception("Params cannot be null or zero");
            }

            var jsonData = protocol.Params[0].ToString();
            BranchName = (string)JsonConvert.DeserializeObject<JObject>(jsonData)["branchName"];
        }
    }
}
