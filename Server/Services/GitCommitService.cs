using System.Diagnostics;
using Server.Interfaces;
using Request = Server.DataTransferObject.Request;
using Response = Server.DataTransferObject.Response;

namespace Server.Services
{
    public class GitCommitService : IGitCommitService
    {
        public Response.ProtocolResponse Handle(Request.GitCommit request)
        {
            var stdOutput = string.Empty;
            var stdError = string.Empty;
            string repoPath = request.WorkDirectory;

            try
            {
                var safeMessage = request.Message.Replace("\"", "\\\"");
                var commitStartInfo = new ProcessStartInfo
                {
                    FileName = "git",
                    Arguments = $"commit -a -m \"{safeMessage}\"",
                    WorkingDirectory = repoPath,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                };

                using (Process commitProcess = Process.Start(commitStartInfo))
                {
                    stdOutput += commitProcess.StandardOutput.ReadToEnd();
                    stdError += commitProcess.StandardError.ReadToEnd();
                    commitProcess.WaitForExit();

                    if (commitProcess.ExitCode != 0)
                        stdError += $"Erro no git commit (ExitCode {commitProcess.ExitCode}).";
                }
            }
            catch (Exception ex)
            {
                stdError = $"Erro ao executar git commit: {ex.Message}";
            }

            return new Response.ProtocolResponse
            {
                Jsonrpc = "2.0",
                Result = string.IsNullOrWhiteSpace(stdError) ? stdOutput : stdError,
            };
        }
    }
}