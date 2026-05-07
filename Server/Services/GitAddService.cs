using System.Diagnostics;
using Server.Interfaces;
using Request = Server.DataTransferObject.Request;
using Response = Server.DataTransferObject.Response;

namespace Server.Services
{
    public class GitAddService : IGithubAddService
    {
        public Response.ProtocolResponse Handle(Request.GitAdd request)
        {
            var stdOutput = string.Empty;
            var stdError = string.Empty;
            string repoPath = request.WorkDirectory;

            try
            {
                var safePath = request.Path.Replace("\"", "\\\"");
                var addStartInfo = new ProcessStartInfo
                {
                    FileName = "git",
                    Arguments = $"add {safePath}",
                    WorkingDirectory = repoPath,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                };

                using (Process addProcess = Process.Start(addStartInfo))
                {
                    stdOutput += addProcess.StandardOutput.ReadToEnd();
                    stdError += addProcess.StandardError.ReadToEnd();
                    addProcess.WaitForExit();

                    if (addProcess.ExitCode != 0)
                        stdError += $"Erro no git add (ExitCode {addProcess.ExitCode}).";
                }
            }
            catch (Exception ex)
            {
                stdError = $"Erro ao executar git add: {ex.Message}";
            }

            return new Response.ProtocolResponse
            {
                Jsonrpc = "2.0",
                Result = string.IsNullOrWhiteSpace(stdError) ? stdOutput : stdError,
            };
        }
    }
}