using System.Diagnostics;
using Server.DataTransferObject.Request;
using Server.DataTransferObject.Response;
using Server.Interfaces;

namespace Server.Services
{
    public class DotnetBuildService : IDotnetBuildService
    {
        public ProtocolResponse Handle(DotnetBuild request)
        {
            try
            {
                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "dotnet",
                        Arguments = $"build {request.Path}",
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
                        Result = $"Build successful: {(string.IsNullOrWhiteSpace(error) ? output : error)}",
                    };
                }
                else
                {
                    return new ProtocolResponse
                    {
                        Jsonrpc = "2.0",
                        Result = $"Build failed: {(string.IsNullOrWhiteSpace(error) ? output : error)}" 
                    };
                }
            }
            catch (Exception ex)
            {
                return new ProtocolResponse
                {
                    Jsonrpc = "2.0",
                    Result =  $"Error: {ex.Message}",
                };
            }
        }
    }
}