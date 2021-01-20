using System.Net.Http;
using System.Threading.Tasks;

namespace FicosoLib
{
    public class FCSHttpClient
    {
        HttpClient _client;
        public FCSHttpClient(HttpClient client)
        {
            _client = client;
        }

        public async Task<HttpResponseMessage> GetFillingAsync(string filingId)
        {
            return  await _client.GetAsync($"OnlineServices/api/File/V1/Filing/{filingId}");

        }
    }
}