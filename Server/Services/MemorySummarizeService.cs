using Domain;
using Server.Interfaces;
using Request = Server.DataTransferObject.Request;
using Response = Server.DataTransferObject.Response;

namespace Server.Services
{
    public class MemorySummarizeService : IMemorySummarizeService
    {
        private readonly IConfiguration _configuration;

        public MemorySummarizeService
            (IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<Response.ProtocolResponse> Handle(Request.MemorySummarize request)
        {
            try
            {
                var response = string.Empty;
                using (var client = new HttpClient())
                {
                    var clientURL = _configuration.GetSection("Settings:ClientUrl")?.Value?.ToString() ?? string.Empty;
                    client.BaseAddress = new Uri(clientURL);
                    client.DefaultRequestHeaders.Add("Accept", "application/json");
                    var content = new StringContent(request.Content);
                    var result = await client.PostAsync("/api/chatbot/summarize", content);
                    if (result.IsSuccessStatusCode)
                    {
                        response = await result.Content.ReadAsStringAsync();
                        ClientToLLM.Memory = [new Message { Role = "system", Content = Tools.PRIMARY_INSTRUCTION }, new Message { Role="system", Content=response}];
                    }
                    else
                    {
                        response = $"Error: {result.ReasonPhrase}";
                    }
                }
                    return new Response.ProtocolResponse
                    {
                        Jsonrpc = "2.0",
                        Result = response,
                    };
            }
            catch (Exception ex)
            {
                return new Response.ProtocolResponse
                {
                    Jsonrpc = "2.0",
                    Result = $"Error: {ex.Message}",
                };
            }
        }
    }
}