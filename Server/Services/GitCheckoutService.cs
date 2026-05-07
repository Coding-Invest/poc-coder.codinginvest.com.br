using System.Diagnostics;
using Server.Interfaces;
using Request = Server.DataTransferObject.Request;
using Response = Server.DataTransferObject.Response;

namespace Server.Services
{
    public class GitCheckoutService : IGitCheckoutService
    {
        public Response.ProtocolResponse Handle(Request.GitCheckout request)
        {
            var repoPath = "./repos/coder.codinginvest.com";
            var branchName = request.BranchName;

            // Executar checkout para a branch
            var checkoutArgs = $"-C {repoPath} checkout {branchName}";
            var resultCheckout = ExecuteGitCommand(checkoutArgs);
            if (resultCheckout.ExitCode != 0)
            {
                return new Response.ProtocolResponse
                {
                    Jsonrpc = "2.0",
                    Result = $"Error: {resultCheckout.Error}",
                };
            }

            return new Response.ProtocolResponse
            {
                Jsonrpc = "2.0",
                Result = "Checkout para a branch realizado com sucesso",
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