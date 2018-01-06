using System.Collections.Generic;
using System.Linq;

namespace VAuto.Client.Models
{
    public class AnswerRequest
    {
        public AnswerRequest(IEnumerable<Dealer> dealers)
        {
            Dealers = dealers;
        }
        public IEnumerable<Dealer> Dealers { get; set; }

        public static AnswerRequest CreateFromDataset(IEnumerable<Dealer> dealers, IEnumerable<Vehicle> vehicles)
        {
            foreach (var dealer in dealers)
            {
                dealer.Vehicles = vehicles
                    .Where(v => v.DealerId == dealer.DealerId)
                    .OrderBy(v => v.VehicleId)
                    .ToList();
            }

            var answerRequest = new AnswerRequest(dealers: dealers.OrderBy(d => d.DealerId));
            return answerRequest;
        }
    }

}