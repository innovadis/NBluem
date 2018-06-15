using System.Net.Http;
using NBluem.Structure.Net.Response;

namespace NBluem.Net.Response
{
    public class BluemResponse : IBluemResponse
    {
        public bool Status { get; set; }

        public int StatusCode { get; set; }

        public string Message { get; set; }

        public HttpContent Content { get; set; }
    }
}