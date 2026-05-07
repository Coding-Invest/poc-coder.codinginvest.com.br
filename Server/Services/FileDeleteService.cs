using Server.Interfaces;
using Request = Server.DataTransferObject.Request;
using Response = Server.DataTransferObject.Response;

namespace Server.Services
{
    public class FileDeleteService : IFileDeleteService
    {
        public Response.ProtocolResponse Handle(Request.FileDelete request)
        {
            string result;

            try
            {
                if (string.IsNullOrWhiteSpace(request.Path))
                    throw new ArgumentException("The path can not be empty.");

                if (File.Exists(request.Path))
                {
                    File.Delete(request.Path);
                    result = $"File deleted successfully: {request.Path}";
                }
                else if (Directory.Exists(request.Path))
                {
                    Directory.Delete(request.Path, true);
                    result = $"Directory Deleted successfully: {request.Path}";
                }
                else
                {
                    result = $"Path not found: {request.Path}";
                }
            }
            catch (Exception ex)
            {
                result = $"Error when deleting: {ex.Message}";
            }

            return new Response.ProtocolResponse
            {
                Jsonrpc = "2.0",
                Result = result
            };
        }
    }
}
