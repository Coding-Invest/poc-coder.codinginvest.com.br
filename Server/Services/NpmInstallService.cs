using System.Diagnostics;
using Server.DataTransferObject.Request;
using Server.DataTransferObject.Response;
using Server.Interfaces;

namespace Server.Services
{
    public class NpmInstallService : INpmInstallService
    {
        public ProtocolResponse Handle(NpmInstall request)
        {
            try
            {
                // Executa npm install no diretório especificado
                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "npm",
                        Arguments = "install",
                        WorkingDirectory = request.Path,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    }
                };
                process.Start();
                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();
                process.WaitForExit();

                if (process.ExitCode == 0)
                {
                    return new ProtocolResponse
                    {
                        Jsonrpc = "2.0",
                        Result = $"Dependências instaladas com sucesso. Output: {output}"
                    };
                }
                else
                {
                    return new ProtocolResponse
                    {
                        Jsonrpc = "2.0",
                        Result = $"Erro ao instalar dependências: {error}"
                    };
                }
            }
            catch (Exception ex)
            {
                return new ProtocolResponse
                {
                    Jsonrpc = "2.0",
                    Result = $"Erro: {ex.Message}"
                };
            }
        }
    }
}