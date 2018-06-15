using System.Net.Http;
using NBluem.Structure.Net.Request;

namespace NBluem.Structure.Net.Transform
{
    public interface IBluemResponseTransformer
    {
        HttpRequestMessage GetHttpRequestMessage(IBluemSignedRequest request);
    }
}