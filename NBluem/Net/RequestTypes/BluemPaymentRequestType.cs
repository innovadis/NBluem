using System;
using System.Net.Http;
using NBluem.Configuration;
using NBluem.Net.Transform.Transformers;
using NBluem.Structure.Enums;
using NBluem.Structure.Net.Transform;

namespace NBluem.Net.RequestTypes
{
    public class BluemPaymentRequestType : BluemRequestType
    {
        public override Uri RequestUri => BluemConfiguration.Config.PaymentRequestUri;

        public override HttpMethod RequestMethod => HttpMethod.Post;

        public override BluemContentTypeEnum ContentType => BluemContentTypeEnum.PTX;

        public override IHttpResponseMessageTransformer Transformer => new PaymentTransactionResponseTransformer();
    }
}