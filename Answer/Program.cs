using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using VAuto.Client.Models;
using VAuto.Client.Services;

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
            var datasetId = await _svc.GetDatasetIdAsync();
            Console.WriteLine("Dataset is: " + datasetId);

            var vehicles = await GetVehicles(datasetId);
            var dealers = await GetDealers(datasetId, vehicles);
            var answerRequest = AnswerRequest.CreateFromDataset(dealers, vehicles);
            Console.WriteLine("answer request: " + JsonConvert.SerializeObject(answerRequest, Formatting.Indented));
            var answer = await _svc.PostAnswerAsync(datasetId, answerRequest);

            var c = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Answer is: " + JsonConvert.SerializeObject(answer));
            Console.ForegroundColor = c;
            //Console.ReadKey();
        }

        private static async Task<IEnumerable<Vehicle>> GetVehicles(string datasetId)
        {
            var vehicleIds = await _svc.GetVehicleIdsAsync(datasetId);
            var timer = Stopwatch.StartNew();

            // Load all vehicles in parallel. 
            var vehicleTasks = vehicleIds.Select(vId => _svc.GetVehicleAsync(datasetId, vId));
            var vehicles = await Task.WhenAll(vehicleTasks);

            Console.WriteLine($"Loaded '{vehicles.Count()}' vehicles ({timer.Elapsed:c})");
            timer.Stop();
            return vehicles;
        }

        private static async Task<IEnumerable<Dealer>> GetDealers(string datasetId, IEnumerable<Vehicle> vehicles)
        {
            var timer = Stopwatch.StartNew();
            var dealerIds = vehicles.Select(v => v.DealerId).Distinct().ToList();

            // Load all dealers in parallel
            var dealerTasks = dealerIds.Select(dId => _svc.GetDealerAsync(datasetId, dId));
            var dealers = await Task.WhenAll(dealerTasks);
            timer.Stop();
            Console.WriteLine($"Loaded '{dealers.Count()}' dealers ({timer.Elapsed:c})");
            return dealers.OrderBy(d => d.DealerId).ToList();
        }
    }
}
