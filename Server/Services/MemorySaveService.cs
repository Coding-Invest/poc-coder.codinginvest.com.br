using System.IO;
using Server.Interfaces;
using Request = Server.DataTransferObject.Request;
using Response = Server.DataTransferObject.Response;

namespace Server.Services
{
    public class MemorySaveService : IMemorySaveService
    {
        public Response.ProtocolResponse Handle(Request.MemorySave request)
        {
            try
            {
                var path = Path.Combine("./repos", request.Filename);
                File.WriteAllText(path, request.Content);
                return new Response.ProtocolResponse
                {
                    Jsonrpc = "2.0",
                    Result = $"Histórico salvo em {request.Filename}",
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
