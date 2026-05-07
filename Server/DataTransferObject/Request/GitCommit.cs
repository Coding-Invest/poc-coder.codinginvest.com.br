using Domain;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace Server.DataTransferObject.Request
{
    public class GitCommit
    {
        public GitCommit(ProtocolRequest protocol)
        {
            if (protocol.Params != null && protocol.Params.Length > 0)
            {
                var jsonData = protocol.Params[0].ToString();
                var args = JsonConvert.DeserializeObject<JObject>(jsonData);
                Message = args["message"]?.ToString() ?? "Commit changes";
                WorkDirectory = args["workDirectory"]?.ToString() ?? Tools.WORKING_DIRECTORY;
            }
            else
            {
                Message = "Commit changes";
                WorkDirectory = Tools.WORKING_DIRECTORY;
            }
        }
        public string Message { get; set; }
        public string WorkDirectory { get; set; }
    }
}