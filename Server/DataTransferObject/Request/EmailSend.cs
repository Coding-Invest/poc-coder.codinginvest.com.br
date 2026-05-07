using Domain;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace Server.DataTransferObject.Request
{
    public class EmailSend
    {
        public EmailSend(ProtocolRequest protocol)
        {
            if (protocol.Params != null && protocol.Params.Length > 0)
            {
                var jsonData = protocol.Params[0].ToString();
                var args = JsonConvert.DeserializeObject<JObject>(jsonData);
                To = args["to"]?.ToString() ?? "recipient@example.com";
                Subject = args["subject"]?.ToString() ?? "Test Subject";
                Body = args["body"]?.ToString() ?? "Test Body";
            }
            else
            {
                To = "recipient@example.com";
                Subject = "Test Subject";
                Body = "Test Body";
            }
        }
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}