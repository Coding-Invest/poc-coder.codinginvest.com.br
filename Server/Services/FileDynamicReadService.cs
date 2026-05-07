using Server.Interfaces;
using Request = Server.DataTransferObject.Request;
using Response = Server.DataTransferObject.Response;

namespace Server.Services
{
    public class FileDynamicReadService : IFileDynamicReadService
    {
        public Response.ProtocolResponse Handle(Request.FileDynamicRead request)
        {
            string result;

            try
            {
                if (string.IsNullOrWhiteSpace(request.Path))
                    throw new ArgumentException("File path can not be empty.");

                if (!File.Exists(request.Path))
                    throw new FileNotFoundException($"File not found: {request.Path}");

                if (request.Index < 0)
                    throw new ArgumentException("Index cannot be negative.");

                if (request.Length < 0)
                    throw new ArgumentException("Length cannot be negative.");

                var content = File.ReadAllText(request.Path);
                if (request.Index >= content.Length)
                    throw new ArgumentException("Index out of range.");

                var startIndex = request.Index;
                var endIndex = Math.Min(startIndex + request.Length, content.Length);
                result = content.Substring(startIndex, endIndex - startIndex);
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