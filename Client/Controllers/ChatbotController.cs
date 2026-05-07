using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Server.DataTransferObject.Response;
using System.Net.Http.Headers;

namespace Client.Controllers
{
    [Route("api/[controller]")]
    public partial class ChatbotController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _configuration;
        private readonly string _llmUrl;
        private readonly string _llmBearerToken;
        private readonly string _mcpServerURL;
        private readonly int _llmSucessiveRequest;
        private int _currentRequestQuantity = 0;

        public ChatbotController(IHttpClientFactory clientFactory, IConfiguration configuration)
        {
            _clientFactory = clientFactory;
            _configuration = configuration;
            _llmUrl = _configuration.GetSection("Settings:LLMUrl")?.Value?.ToString() ?? string.Empty;
            _llmBearerToken = _configuration.GetSection("Settings:LLMBearerToken")?.Value?.ToString() ?? string.Empty;
            _mcpServerURL = Environment.GetEnvironmentVariable("SERVER_BASE_URL") ?? _configuration.GetSection("Settings:MCPServerUrl")?.Value?.ToString() ?? string.Empty;
            _llmSucessiveRequest = Convert.ToInt32(_configuration.GetSection("Settings:LLMSucessiveRequest")?.Value?.ToString() ?? "100");
        }

        [HttpPost, Route("send"), Authorize]
        public async Task<IActionResult> Send([FromBody] Payload payload)
        {
            try
            {
                _currentRequestQuantity = 0;
                ClientToLLM.Memory.Add(new Message { Role = "user", Content = payload.Prompt });
                var request = new CompletionRequest
                {
                    Messages = ClientToLLM.Memory
                };
                var result = await SendGenerateRequest(request);

                if (!result)
                {
                    return StatusCode(500, "Erro ao processar geração de tokens");
                }

                await Response.CompleteAsync();
            }
            catch (Exception ex)
            {
                return Ok(new { message = "error", detail = ex.Message });
            }

            return Ok(new { message = "success" });
        }

        private async Task<bool> SendGenerateRequest(CompletionRequest request)
        {
            using var httpClientLLM = _clientFactory.CreateClient();
            httpClientLLM.Timeout = Timeout.InfiniteTimeSpan;
            httpClientLLM.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _llmBearerToken);
            var serializedRequest = JsonConvert.SerializeObject(request);
            using var httpRequestLLM = new HttpRequestMessage(HttpMethod.Post, _llmUrl)
            {
                Content = new StringContent(serializedRequest, System.Text.Encoding.UTF8, "application/json"),
                Headers =
                {
                    { "Accept", "text/event-stream" }
                }
            };
            var responseLLM = await httpClientLLM.SendAsync(httpRequestLLM, HttpCompletionOption.ResponseHeadersRead);
            if (!responseLLM.IsSuccessStatusCode)
            {
                return false;
            }

            var stream = await responseLLM.Content.ReadAsStreamAsync();
            var reader = new StreamReader(stream);
            var llmCommandText = string.Empty;
            bool readingStarted = false;
            var llmResponseContent = string.Empty;
            while (!reader.EndOfStream)
            {
                var line = await reader.ReadLineAsync();
                var jsonData = Newtonsoft.Json.JsonConvert.DeserializeObject<Newtonsoft.Json.Linq.JObject>(line.Trim());
                var jsonResponseContent = jsonData["choices"][0]["message"]["content"]?.ToString();
                llmResponseContent += jsonResponseContent;
                if ((!readingStarted && jsonResponseContent.StartsWith("{")) || llmCommandText.Length > 0)
                {
                    llmCommandText += jsonResponseContent;
                    continue;
                }
                readingStarted = true;
                await Response.Body.WriteAsync(System.Text.Encoding.UTF8.GetBytes(line));
                await Response.Body.FlushAsync();
            }

            if (llmResponseContent.StartsWith("{")){
                try
                {
                    Newtonsoft.Json.JsonConvert.DeserializeObject<JObject>(llmCommandText);
                }
                catch(Exception ex)
                {
                    var generateRequest = new CompletionRequest
                    {
                        Messages = ClientToLLM.Memory.Concat(new List<Message>
                        {
                            new Message
                            {
                                Role = "system",
                                Content = "Error:"+ex.Message
                            }
                        }).ToList()
                    };

                    return await SendGenerateRequest(generateRequest);
                }
            }

            if (string.IsNullOrEmpty(llmResponseContent))
            {
                var generateRequest = new CompletionRequest
                {
                    Messages = ClientToLLM.Memory.Concat(new List<Message>
                        {
                            new Message
                            {
                                Role = "system",
                                Content = "Error: assistant AI should avoid answering empty"
                            }
                        }).ToList()
                };

                return await SendGenerateRequest(generateRequest);
            }

            ClientToLLM.Memory.Add(new Message { Role = "assistant", Content = llmResponseContent });

            if (llmCommandText.Length > 0)
            {
                var command = Newtonsoft.Json.JsonConvert.DeserializeObject<Schema>(llmCommandText);
                var protocol = new ProtocolRequest
                {
                    Jsonrpc = "2.0",
                    Method = command.Method,
                    Params = command.Args.ToArray(),
                };

                var handler = new HttpClientHandler();
                handler.ServerCertificateCustomValidationCallback =
                    HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
                using var httpClientMCPServer = new HttpClient(handler);
                httpClientMCPServer.Timeout = Timeout.InfiniteTimeSpan;
                var serialized = JsonConvert.SerializeObject(protocol);

                using var httpRequestMCPServer = new HttpRequestMessage(HttpMethod.Post, _mcpServerURL)
                {
                    Content = new StringContent(serialized, System.Text.Encoding.UTF8, "application/json")
                };

                httpRequestMCPServer.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var responseMCPServer = await httpClientMCPServer.SendAsync(httpRequestMCPServer, HttpCompletionOption.ResponseHeadersRead);

                if (!responseMCPServer.IsSuccessStatusCode)
                {
                    return false;
                }

                var result = await responseMCPServer.Content.ReadAsStringAsync();
                var protocolResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<ProtocolResponse>(result);

                ClientToLLM.Memory.Add(new Message { Role = "system", Content = protocolResponse?.Result ?? string.Empty });
                var generateRequest = new CompletionRequest
                {
                    Messages = ClientToLLM.Memory
                };

                _currentRequestQuantity++;
                if (_currentRequestQuantity >= _llmSucessiveRequest) 
                { 
                    throw new Exception("Comunicação interrompida: Excessivas requisições em sequência"); 
                }

                return await SendGenerateRequest(generateRequest);
            }
            return true;
        }

        [HttpPost,Route("Summarize"), AllowAnonymous]
        public async Task<IActionResult> Summarize([FromBody] Payload payload)
        {
            try
            {
                using var llmClient = _clientFactory.CreateClient();
                llmClient.BaseAddress = new Uri(_llmUrl);
                var request = new
                {
                    text = payload.Prompt
                };
                var serialized = JsonConvert.SerializeObject(request);
                using var httpRequest = new HttpRequestMessage(HttpMethod.Post, _llmUrl)
                {
                    Content = new StringContent(serialized, System.Text.Encoding.UTF8, "application/json")
                };
                httpRequest.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await llmClient.SendAsync(httpRequest, HttpCompletionOption.ResponseHeadersRead);

                if (!response.IsSuccessStatusCode)
                {
                    return StatusCode(500, "Erro ao processar sumarização");
                }

                var result = await response.Content.ReadAsStringAsync();
                return Ok(new { message = "success", detail = result });
            }
            catch (Exception ex)
            {
                return Ok(new { message = "error", detail = ex.Message });
            }
        }
    }
}