using Server.Interfaces;
using Request = Server.DataTransferObject.Request;
using Response = Server.DataTransferObject.Response;

namespace Server.Services
{
    public class FileReadService : IFileReadService
    {
        public Response.ProtocolResponse Handle(Request.FileRead request)
        {
            string result;

            try
            {
                if (string.IsNullOrWhiteSpace(request.Path))
                    throw new ArgumentException("File path can not be empty.");

                if (!File.Exists(request.Path))
                    throw new FileNotFoundException($"File not found: {request.Path}");

                result = File.ReadAllText(request.Path);
            }
            catch (Exception ex)
            {
                result = $"Erro ao ler arquivo: {ex.Message}";
            }

            return new Response.ProtocolResponse
            {
                Jsonrpc = "2.0",
                Result = result
            };
        }
    }
}
