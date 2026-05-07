using System.Diagnostics;
using Server.Interfaces;
using Request = Server.DataTransferObject.Request;
using Response = Server.DataTransferObject.Response;

namespace Server.Services
{
    public class GitBranchCreateService : IGitBranchCreateService
    {
        public Response.ProtocolResponse Handle(Request.GitBranchCreate request)
        {
            var repoPath = "./repos/coder.codinginvest.com";
            var branchName = request.BranchName;

            // Criar branch local
            var createBranchArgs = $"-C {repoPath} checkout -b {branchName}";
            var resultCreate = ExecuteGitCommand(createBranchArgs);
            if (resultCreate.ExitCode != 0)
            {
                return new Response.ProtocolResponse
                {
                    Jsonrpc = "2.0",
                    Result = $"Error: {resultCreate.Error}",
                };
            }

            // Push para remoto
            var pushArgs = $"-C {repoPath} push --set-upstream origin {branchName}";
            var resultPush = ExecuteGitCommand(pushArgs);
            if (resultPush.ExitCode != 0)
            {
                return new Response.ProtocolResponse
                {
                    Jsonrpc = "2.0",
                    Result = $"Error: {resultPush.Error}",
                };
            }

            return new Response.ProtocolResponse
            {
                Jsonrpc = "2.0",
                Result = "Branch remota criada com sucesso no GitHub",
            };
        }

        private (string Output, string Error, int ExitCode) ExecuteGitCommand(string arguments)
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = "git",
                Arguments = arguments,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
            };

            using (Process process = new Process { StartInfo = startInfo })
            {
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
                    stdError = $"Erro ao executar comando git: {ex.Message}";
                    return (stdOutput, stdError, -1);
                }

                return (stdOutput, stdError, process.ExitCode);
            }
        }
    }
}
