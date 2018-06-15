using NBluem.Structure.Net.Response;

namespace NBluem.Net.Response
{
    public class BluemErrorResponse : IBluemResponse
    {
        public bool Status { get; set; }
        public string Message { get; set; }

        public string ErrorCode { get; set; }

        public string ErrorMessage { get; set; }

        public string Object { get; set; }
    }
}