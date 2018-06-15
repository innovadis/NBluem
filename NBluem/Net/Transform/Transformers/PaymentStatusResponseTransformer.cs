using System.Net;
using NBluem.Net.Response;
using NBluem.Structure.Net.Response;
using NBluem.ValueObjects;

namespace NBluem.Net.Transform.Transformers
{
    public class PaymentStatusResponseTransformer : AbstractHttpResponseMessageTransformer
    {
        public override string NodeName => "PaymentStatusUpdate";

        public override IBluemResponse GetBluemResponse(HttpWebResponse response)
        {
            var xml = EPaymentInterface(response);

            return new BluemPaymentStatusResponse
            {
                Status = true,
                DebtorReference = xml.Element("DebtorReference").Value,
                EntranceCode = new EntranceCode(xml.Attribute("entranceCode").Value),
                PaymentReference = xml.Element("PaymentReference").Value,
                TransactionId = xml.Element("TransactionID").Value,
                PaymentStatus = xml.Element("Status").Value
            };
        }
    }
}