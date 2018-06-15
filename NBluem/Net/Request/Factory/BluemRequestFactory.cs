using NBluem.Net.RequestTypes;
using NBluem.Structure.Net.Request;
using NBluem.Structure.Net.Request.Factory;

namespace NBluem.Net.Request.Factory
{
    public class BluemRequestFactory : IBluemRequestFactory
    {
        public IBluemRequest CreateRequest(BluemRequestType requestType)
        {
            if (requestType is BluemPaymentRequestType)
            {
                return new BluemPaymentRequest(requestType);
            }

            if (requestType is BluemPaymentStatusRequestType)
            {
                return new BluemPaymentStatusRequest(requestType);
            }

            return null;
        }
    }
}