using Server.Interfaces;
using Request = Server.DataTransferObject.Request;
using Response = Server.DataTransferObject.Response;

namespace Server.Services
{
    public class FileWriteService : IFileWriteService
    {
        public Response.ProtocolResponse Handle(Request.FileWrite request)
        {
            string result;

            try
            {
                if (string.IsNullOrWhiteSpace(request.Path))
                    throw new ArgumentException("Path can not be empty.");

                var directory = Path.GetDirectoryName(request.Path);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                File.WriteAllText(request.Path, request.Content ?? string.Empty);

                result = $"File written successfully in: {request.Path}";
            }
            catch (Exception ex)
            {
                result = $"Error when writting file: {ex.Message}";
            }

            return new Response.ProtocolResponse
            {
                Jsonrpc = "2.0",
                Result = result
            };
        }
    }
}
