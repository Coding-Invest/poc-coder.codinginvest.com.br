using System.Text.Json;
using System.Web;
using Domain;
using Server.Interfaces;
using Request = Server.DataTransferObject.Request;
using Response = Server.DataTransferObject.Response;

namespace Server.Services
{
    public class FetchUrlService : IFetchUrlService
    {
        public async Task<Response.ProtocolResponse> Handle(Request.FetchUrl request)
        {
            var result = string.Empty;
            var error = string.Empty;

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync(request.Url);
                    response.EnsureSuccessStatusCode();
                    var content = await response.Content.ReadAsStringAsync();
                    var fileName = Tools.WORKING_DIRECTORY+"/pagesfetched/"+ HttpUtility.UrlEncode(request.Url) + ".txt";
                    File.WriteAllText(fileName, content);
                    result = "Conteúdo salvo com sucesso em :" + fileName;
                }
            }
            catch (Exception ex)
            {
                error = $"Erro ao buscar URL: {ex.Message}";
            }

            return new Response.ProtocolResponse
            {
                Jsonrpc = "2.0",
                Result = string.IsNullOrWhiteSpace(error) ? result : error,
            };
        }
    }
}