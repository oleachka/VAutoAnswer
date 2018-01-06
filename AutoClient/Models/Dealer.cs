using System.Collections.Generic;

namespace VAuto.Client.Models
{
    public class Dealer
    {
        public int DealerId { get; set; }
        public string Name { get; set; }
        public IEnumerable<Vehicle> Vehicles { get; set;}
    }

}