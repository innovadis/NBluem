using NBluem.Net.RequestTypes;

namespace NBluem.Structure.Net.Request.Factory
{
    public interface IBluemRequestFactory
    {
        IBluemRequest CreateRequest(BluemRequestType requestType);
    }
}