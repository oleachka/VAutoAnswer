using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VAuto.Client.Services
{
    public static class HttpClientExtensions
    {
        public static async Task<T> GetAsAsync<T>(this HttpClient http, string url)
        {
            var resp = await http.GetStringAsync(url);
            return JsonConvert.DeserializeObject<T>(resp);
        }
    }
}