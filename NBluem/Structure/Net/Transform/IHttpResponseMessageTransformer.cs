using System.Net;
using System.Xml.Linq;
using NBluem.Structure.Net.Response;

namespace NBluem.Structure.Net.Transform
{
    public interface IHttpResponseMessageTransformer
    {
        IBluemResponse GetBluemResponse(HttpWebResponse response);

        XElement EPaymentInterface(HttpWebResponse response);

        string NodeName { get; }
    }
}   