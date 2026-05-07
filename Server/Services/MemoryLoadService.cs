using System.IO;
using Server.Interfaces;
using Request = Server.DataTransferObject.Request;
using Response = Server.DataTransferObject.Response;

namespace Server.Services
{
    public class MemoryLoadService : IMemoryLoadService
    {
        public Response.ProtocolResponse Handle(Request.MemoryLoad request)
        {
            try
            {
                var path = Path.Combine("./repos", request.Filename);
                var content = File.ReadAllText(path);
                return new Response.ProtocolResponse
                {
                    Jsonrpc = "2.0",
                    Result = content,
                };
            }
            catch (Exception ex)
            {
                return new Response.ProtocolResponse
                {
                    Jsonrpc = "2.0",
                    Result = $"Error: {ex.Message}",
                };
            }
        }
    }
}
