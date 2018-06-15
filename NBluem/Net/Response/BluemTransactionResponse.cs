using System.Net.Http;
using NBluem.Structure.Net.Response;
using NBluem.ValueObjects;

namespace NBluem.Net.Response
{
    public class BluemTransactionResponse : IBluemResponse
    {
        public bool Status { get; set; }

        public string Message { get; set; }

        public EntranceCode EntranceCode { get; set; }

        public string PaymentReference { get; set; }

        public string DebtorReference { get; set; }

        public string TransactionId { get; set; }

        public string TransactionUrl { get; set; }

        public string ShortTransactionUrl { get; set; }
    }
}