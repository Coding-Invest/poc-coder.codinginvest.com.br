using System.Diagnostics;
using Server.Interfaces;
using Request = Server.DataTransferObject.Request;
using Response = Server.DataTransferObject.Response;

namespace Server.Services
{
    public class GitPushService : IGitPushService
    {
        public Response.ProtocolResponse Handle(Request.GitPush request)
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = "git",
                Arguments = $"push",
                WorkingDirectory = request.TargetDirectory,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
            };

            Process process = new Process { StartInfo = startInfo };

            var stdOutput = string.Empty;
            var stdError = string.Empty;

            try
            {
                process.Start();
                stdOutput = process.StandardOutput.ReadToEnd();
                stdError = process.StandardError.ReadToEnd();
                process.WaitForExit();
            }
            catch (Exception ex)
            {
                stdError = $"Erro ao executar git push: {ex.Message}";
            }

            return new Response.ProtocolResponse
            {
                Jsonrpc = "2.0",
                Result = string.IsNullOrWhiteSpace(stdError) ? stdOutput : stdError,
            };
        }
    }
}