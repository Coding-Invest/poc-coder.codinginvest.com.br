using Server.Interfaces;
using Request = Server.DataTransferObject.Request;
using Response = Server.DataTransferObject.Response;
using Newtonsoft.Json;

namespace Server.Services
{
    public class GoogleSearchService : IGoogleSearchService
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public GoogleSearchService(IConfiguration configuration)
        {
            _configuration = configuration;
            _httpClient = new HttpClient();
        }

        public Response.ProtocolResponse Handle(Request.GoogleSearch request)
        {
            var apiKey = _configuration.GetSection("Google:ApiKey")?.Value;
            var cx = _configuration.GetSection("Google:Cx")?.Value;

            if (string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(cx))
            {
                return new Response.ProtocolResponse
                {
                    Jsonrpc = "2.0",
                    Result = "Erro: Configuração da API do Google não encontrada. Verifique appsettings.json para 'Google.ApiKey' e 'Google.Cx'.",
                };
            }

            try
            {
                var url = $"https://www.googleapis.com/customsearch/v1?key={apiKey}&cx={cx}&q={Uri.EscapeDataString(request.Query)}&num={request.NumResults}";
                var response = _httpClient.GetAsync(url).Result;
                response.EnsureSuccessStatusCode();
                var json = response.Content.ReadAsStringAsync().Result;
                var searchResult = JsonConvert.DeserializeObject<GoogleSearchResult>(json);

                if (searchResult.Items != null)
                {
                    var results = searchResult.Items.Select(item => new
                    {
                        Title = item.Title,
                        Url = item.Link,
                        Snippet = item.Snippet
                    });
                    return new Response.ProtocolResponse
                    {
                        Jsonrpc = "2.0",
                        Result = JsonConvert.SerializeObject(results),
                    };
                }
                else
                {
                    return new Response.ProtocolResponse
                    {
                        Jsonrpc = "2.0",
                        Result = "[]",
                    };
                }
            }
            catch (Exception ex)
            {
                return new Response.ProtocolResponse
                {
                    Jsonrpc = "2.0",
                    Result = $"Erro ao executar pesquisa no Google: {ex.Message}",
                };
            }
        }

        private class GoogleSearchResult
        {
            public List<GoogleSearchItem> Items { get; set; }
        }

        private class GoogleSearchItem
        {
            public string Title { get; set; }
            public string Link { get; set; }
            public string Snippet { get; set; }
        }
    }
}