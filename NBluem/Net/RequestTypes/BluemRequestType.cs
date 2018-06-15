using System;
using System.Net.Http;
using NBluem.Structure.Enums;
using NBluem.Structure.Net.Transform;
using NBluem.ValueObjects;

namespace NBluem.Net.RequestTypes
{
    public abstract class BluemRequestType
    {
        private TtrsDateTime _dateTime;

        public abstract Uri RequestUri { get; }

        public abstract HttpMethod RequestMethod { get; }

        public TtrsDateTime DateTime => _dateTime ?? (_dateTime = new TtrsDateTime(System.DateTime.UtcNow));

        public int FilesCount => 1;

        public TtrsFile File { get; set; }

        public abstract BluemContentTypeEnum ContentType { get; }

        public abstract IHttpResponseMessageTransformer Transformer { get; }
    }
}