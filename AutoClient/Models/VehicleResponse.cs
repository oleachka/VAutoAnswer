using System.Collections.Generic;

namespace VAuto.Client.Models
{
    public class VehicleResponse
    {
        public VehicleResponse()
        {
            VehicleIds = new List<int>();
        }
        public IEnumerable<int> VehicleIds { get; set; }
    }

}