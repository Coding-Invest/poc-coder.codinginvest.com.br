using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Server.Interfaces;
using Request = Server.DataTransferObject.Request;
using Response = Server.DataTransferObject.Response;

namespace Server.Services
{
    public class GitPullRequestService : IGitPullRequestService
    {
        private readonly IConfiguration _configuration;

        public GitPullRequestService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Response.ProtocolResponse Handle(Request.GitPullRequest request)
        {
            var repoPath = "./repos/coder.codinginvest.com";

            // Obter branch atual se Head não for especificado
            var head = request.Head;
            if (string.IsNullOrEmpty(head))
            {
                var currentBranchArgs = $"-C {repoPath} rev-parse --abbrev-ref HEAD";
                var currentBranchResult = ExecuteGitCommand(currentBranchArgs);
                if (currentBranchResult.ExitCode != 0)
                {
                    return new Response.ProtocolResponse
                    {
                        Jsonrpc = "2.0",
                        Result = $"Error: {currentBranchResult.Error}",
                    };
                }
                head = currentBranchResult.Output.Trim();
            }
            var baseBranch = request.Base ?? "master";

            // Obter owner/repo do .git/config
            var remoteUrlArgs = $"-C {repoPath} config --get remote.origin.url";
            var remoteUrlResult = ExecuteGitCommand(remoteUrlArgs);
            if (remoteUrlResult.ExitCode != 0)
            {
                return new Response.ProtocolResponse
                {
                    Jsonrpc = "2.0",
                    Result = $"Error: {remoteUrlResult.Error}",
                };
            }
            var remoteUrl = remoteUrlResult.Output.Trim();
            var repoInfo = ParseGitUrl(remoteUrl);
            if (repoInfo == null)
            {
                return new Response.ProtocolResponse
                {
                    Jsonrpc = "2.0",
                    Result = "Erro ao parsear URL do repositório",
                };
            }

            var token = _configuration["Git:PersonalAccessToken"];
            if (string.IsNullOrEmpty(token))
            {
                return new Response.ProtocolResponse
                {
                    Jsonrpc = "2.0",
                    Result = "GitHub Personal Access Token não configurado",
                };
            }

            // Criar PR via API
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("AppName", "1.0"));
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));

                var payload = new
                {
                    title = request.Title,
                    body = request.Body,
                    head = head,
                    @base = baseBranch
                };
                var json = JsonConvert.SerializeObject(payload);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var url = $"https://api.github.com/repos/{repoInfo.Value.owner}/{repoInfo.Value.repo}/pulls";
                var response = client.PostAsync(url, content).Result;
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = response.Content.ReadAsStringAsync().Result;
                    var pr = JsonConvert.DeserializeObject<JObject>(responseContent);
                    return new Response.ProtocolResponse
                    {
                        Jsonrpc = "2.0",
                        Result = $"Pull Request criado com sucesso: {pr["html_url"]}",
                    };
                }
                else
                {
                    var error = response.Content.ReadAsStringAsync().Result;
                    return new Response.ProtocolResponse
                    {
                        Jsonrpc = "2.0",
                        Result = $"Erro ao criar PR: {error}",
                    };
                }
            }
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

        private (string owner, string repo)? ParseGitUrl(string url)
        {
            // Exemplo: https://github.com/owner/repo.git ou git@github.com:owner/repo.git
            if (url.Contains("github.com"))
            {
                var parts = url.Replace(".git", "").Split('/');
                if (parts.Length >= 2)
                {
                    var owner = parts[parts.Length - 2];
                    var repo = parts[parts.Length - 1];
                    return (owner, repo);
                }
            }
            return null;
        }
    }
}