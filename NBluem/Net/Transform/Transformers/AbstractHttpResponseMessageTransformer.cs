using System;
using System.IO;
using System.Net;
using System.Xml.Linq;
using System.Xml.XPath;
using NBluem.Structure.Exceptions;
using NBluem.Structure.Net.Response;
using NBluem.Structure.Net.Transform;

namespace NBluem.Net.Transform.Transformers
{
    public abstract class AbstractHttpResponseMessageTransformer : IHttpResponseMessageTransformer
    {
        private const string EPaymentInterfaceNode = "EPaymentInterface";

        public abstract string NodeName { get; }

        public abstract IBluemResponse GetBluemResponse(HttpWebResponse response);

        public XElement EPaymentInterface(HttpWebResponse response)
        {
            using (var reader = new StreamReader(response.GetResponseStream()))
            {
                var el = XDocument.Parse(reader.ReadToEnd())
                    .Element(EPaymentInterfaceNode);

                if (null == el.Element(NodeName))
                    throw new BluemTransformException($"Element with name {NodeName} could not be found.");

                if (Convert.ToBoolean(el.Element(NodeName).XPathEvaluate(@"boolean(//Error)")))
                {
                    var code = el.XPathSelectElement("//Error/ErrorCode");
                    var message = el.XPathSelectElement("//Error/ErrorMessage");
                    throw new BluemTransformException($"An error with code {code.Value} has occured: {message.Value}");
                }

                return  el.Element(NodeName);
            }
        }
    }
}