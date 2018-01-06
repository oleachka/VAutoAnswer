using System.Collections.Generic;
using System.Threading.Tasks;
using VAuto.Client.Models;

namespace VAuto.Client.Services
{
    public interface IVAutoService
    {

        Task<string> GetDatasetIdAsync();

        Task<IEnumerable<int>> GetVehicleIdsAsync(string datasetId);

        Task<Vehicle> GetVehicleAsync(string datasetId, int vehicleId);

        Task<Dealer> GetDealerAsync(string datasetId, int dealerId);

        Task<Answer> PostAnswerAsync(string datasetId, AnswerRequest request);
    }
}