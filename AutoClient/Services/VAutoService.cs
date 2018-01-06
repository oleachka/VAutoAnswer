using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using VAuto.Client.Models;

namespace VAuto.Client.Services
{
    public class VAutoService : IVAutoService
    {
        private HttpClient _http;
        public VAutoService()
        {
            _http = new HttpClient()
            {
                BaseAddress = new Uri("http://vautointerview.azurewebsites.net")
            };
        }
        public async Task<string> GetDatasetIdAsync()
        {
            var response = await _http.GetAsAsync<DatasetResponse>("api/datasetId");
            return response.DatasetId;
        }

        public async Task<Dealer> GetDealerAsync(string datasetId, int dealerId)
        {
            var response = await _http.GetAsAsync<Dealer>($"api/{datasetId}/dealers/{dealerId}");
            return response;
        }

        public async Task<Vehicle> GetVehicleAsync(string datasetId, int vehicleId)
        {
            var response = await _http.GetAsAsync<Vehicle>($"api/{datasetId}/vehicles/{vehicleId}");
            return response;
        }

        public async Task<IEnumerable<int>> GetVehicleIdsAsync(string datasetId)
        {
            var response = await _http.GetAsAsync<VehicleResponse>($"api/{datasetId}/vehicles");
            return response.VehicleIds;
        }

        public async Task<Answer> PostAnswerAsync(string datasetId, AnswerRequest request)
        {
            var url = $"api/{datasetId}/answer";
            var content = new StringContent(
                JsonConvert.SerializeObject(request),
                Encoding.UTF8,
                "application/json"
            );
            var response = await _http.PostAsync(url, content);
            var str = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Answer>(str);
        }
    }
}