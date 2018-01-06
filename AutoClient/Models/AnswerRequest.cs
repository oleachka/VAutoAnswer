using System.Collections.Generic;

namespace VAuto.Client.Models
{
    public class AnswerRequest
    {
        public AnswerRequest(IEnumerable<Dealer> dealers)
        {
            Dealers = dealers;
        }
        public IEnumerable<Dealer> Dealers { get; set; }
    }

}