using System.Net.Http;
using NBluem.Structure.Net.Request;
using NBluem.Structure.Net.Transform;

namespace NBluem.Net.Transform
{
    public class BluemResponseTransformer : IBluemResponseTransformer
    {
        public HttpRequestMessage GetHttpRequestMessage(IBluemSignedRequest request)
        {
            var type = request.RequestType;

            return new HttpRequestMessage(type.RequestMethod, type.RequestUri);
        }
    }
}