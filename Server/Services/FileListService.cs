using Server.Interfaces;
using Request = Server.DataTransferObject.Request;
using Response = Server.DataTransferObject.Response;

namespace Server.Services
{
    public class FileListService : IFileListService
    {
        public Response.ProtocolResponse Handle(Request.FileList request)
        {
            var result = string.Empty;
            try
            {
                var path = string.IsNullOrWhiteSpace(request.Directory) ? "./repos" : request.Directory;
                var directories = Directory.GetDirectories(path).Select(d=>"directory:"+d);
                var files = Directory.GetFiles(path).Select(f=>"file:"+f);
                var paths = directories.Concat(files).Select(p => p.Replace("\\", "/")).ToList();
                result = string.Join("\n", paths);
            }
            catch (Exception ex)
            {
                result = $"Erro ao listar arquivos: {ex.Message}";
            }

            return new Response.ProtocolResponse
            {
                Jsonrpc = "2.0",
                Result = result
            };
        }
    }
}
