using NBluem.Structure.Net.Request;
using NBluem.Structure.Net.Response;

namespace NBluem.Net.Transform
{
    public interface IBluemClient
    {
        IBluemResponse SendRequest(IBluemSignedRequest request);
    }
}