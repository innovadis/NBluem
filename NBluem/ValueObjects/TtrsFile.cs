using System.Net.Http;

namespace NBluem.ValueObjects
{
    public class TtrsFile
    {
        public string MimeType { get; set; }

        public string FileType { get; set; }

        public string Filename { get; set; }

        public string Signature { get; set; }

        public StringContent File { get; set; }
    }
}