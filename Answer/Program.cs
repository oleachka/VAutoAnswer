using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using VAuto.Client.Models;
using VAuto.Client.Services;
using Kurukuru;

namespace Answer
{
    class Program
    {
        private static IVAutoService _svc;
        static void Main(string[] args)
        {
            _svc = new VAutoService();
            MainAsync().Wait();
        }

        private static async Task MainAsync()
        {
            string datasetId = null;
            await Spinner.StartAsync("Loading Dataset", async spinner =>
            {
                datasetId = await _svc.GetDatasetIdAsync();
                spinner.Succeed($"Dataset is: '{datasetId}'");
            });

            var vehicles = await GetVehicles(datasetId);
            var dealers = await GetDealers(datasetId, vehicles);
            await Spinner.StartAsync("Posting Answer...", async spinner =>
            {
                var answerRequest = AnswerRequest.CreateFromDataset(dealers, vehicles);
                // Console.WriteLine("answer request: " + JsonConvert.SerializeObject(answerRequest, Formatting.Indented));
                var answer = await _svc.PostAnswerAsync(datasetId, answerRequest);
                spinner.Succeed(JsonConvert.SerializeObject(answer));
            });
        }

        private static async Task<IEnumerable<Vehicle>> GetVehicles(string datasetId)
        {
            IEnumerable<Vehicle> vehicles = null;
            await Spinner.StartAsync("Loading Vehicles in Parallel...", async spinner =>
            {

                var vehicleIds = await _svc.GetVehicleIdsAsync(datasetId);
                var timer = Stopwatch.StartNew();
                var vehicleTasks = vehicleIds.Select(vId => _svc.GetVehicleAsync(datasetId, vId));
                vehicles = await Task.WhenAll(vehicleTasks);
                timer.Stop();
                spinner.Succeed($"Loaded '{vehicles.Count()}' vehicles ({timer.Elapsed:c})");
            });
            return vehicles;
        }

        private static async Task<IEnumerable<Dealer>> GetDealers(string datasetId, IEnumerable<Vehicle> vehicles)
        {
            IEnumerable<Dealer> dealers = null;
            await Spinner.StartAsync("Loading Dealers in Parallel...", async spinner =>
            {
                var timer = Stopwatch.StartNew();
                var dealerTasks = vehicles
                    .Select(v => v.DealerId)
                    .Distinct()
                    .Select(dId => _svc.GetDealerAsync(datasetId, dId));
                    
                dealers = (await Task.WhenAll(dealerTasks));
                timer.Stop();
                spinner.Succeed($"Loaded '{dealers.Count()}' dealers ({timer.Elapsed:c})");
            });
            return dealers?.OrderBy(d => d.DealerId).ToList();
        }
    }
}
