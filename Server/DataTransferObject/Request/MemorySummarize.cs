using Domain;

namespace Server.DataTransferObject.Request
{
    public class MemorySummarize
    {
        public string Content { get; set; }
        public MemorySummarize(ProtocolRequest protocol)
        {
            // Assuming no params, or parse if needed
            Content = string.Join("\r\n", ClientToLLM.Memory.Skip(1).Select(x => x.Role + ":" + x.Content));
        }
    }
}