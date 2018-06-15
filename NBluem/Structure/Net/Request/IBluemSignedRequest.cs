using NBluem.Net.RequestTypes;
using NBluem.Structure.Enums;
using NBluem.ValueObjects;

namespace NBluem.Structure.Net.Request
{
    public interface IBluemSignedRequest
    {
        string RequestSignature { get; set; }

        TtrsFile File { get; set; }

        TtrsDateTime DateTime { get; set; }

        BluemContentTypeEnum ContentType { get; set; }

        BluemRequestType RequestType { get; set; }

        string UnsignedString { get; set; }
    }
}