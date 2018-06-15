using NBluem.Net.RequestTypes;
using NBluem.Structure.Enums;
using NBluem.Structure.Net.Request;
using NBluem.ValueObjects;

namespace NBluem.Net.Request
{
    class BluemSignedRequest : IBluemSignedRequest
    {
        public string RequestSignature { get; set; }
        public TtrsFile File { get; set; }
        public TtrsDateTime DateTime { get; set; }
        public BluemContentTypeEnum ContentType { get; set; }
        public BluemRequestType RequestType { get; set; }
        public string UnsignedString { get; set; }
    }
}