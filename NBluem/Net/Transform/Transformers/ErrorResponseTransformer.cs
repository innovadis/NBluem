using System.Net;
using NBluem.Net.Response;
using NBluem.Structure.Net.Response;
using NBluem.ValueObjects;

namespace NBluem.Net.Transform.Transformers
{
    public class ErrorResponseTransformer : AbstractHttpResponseMessageTransformer
    {
        public override string NodeName => "PaymentErrorResponse";

        public override IBluemResponse GetBluemResponse(HttpWebResponse response)
        {
            var xml = EPaymentInterface(response);
            var error = xml.Element("Error");

            return new BluemErrorResponse()
            {
                ErrorCode = error.Element("ErrorCode").Value,
                ErrorMessage = error.Element("ErrorMessage").Value,
                Object = error.Element("Object").Value
            };
        }
    }
}