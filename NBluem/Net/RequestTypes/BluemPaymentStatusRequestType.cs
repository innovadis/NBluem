using System;
using System.Net.Http;
using NBluem.Configuration;
using NBluem.Net.Transform.Transformers;
using NBluem.Structure.Enums;
using NBluem.Structure.Net.Transform;

namespace NBluem.Net.RequestTypes
{
    public class BluemPaymentStatusRequestType : BluemRequestType
    {
        public override Uri RequestUri => BluemConfiguration.Config.PaymentStatusUri;

        public override HttpMethod RequestMethod => HttpMethod.Post;

        public override BluemContentTypeEnum ContentType => BluemContentTypeEnum.PSX;

        public override IHttpResponseMessageTransformer Transformer => new PaymentStatusResponseTransformer();
    }
}