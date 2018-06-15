using System;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using NBluem.Configuration;
using NBluem.Net.RequestTypes;
using NBluem.Structure.Enums;
using NBluem.Structure.Net.Request;
using NBluem.ValueObjects;

namespace NBluem.Net.Request
{
    public class BluemPaymentRequest : AbstractBluemRequest, IBluePaymentRequest
    {
        internal BluemPaymentRequest(BluemRequestType requestType) : base(requestType)
        {
        }

        public void AddPaymentData(EntranceCode entranceCode, string paymentReference, string debtorReference, string description, double amount, DateTime dueDateTime)
        {
            var paymentXml = BuildXml(entranceCode, paymentReference, debtorReference, description, amount, dueDateTime);

            var xmlDoc = new XDocument(
                new XDeclaration("1.0", "utf-8", "yes"),
                paymentXml);

            using (var mem = new MemoryStream())
            using (var writer = new XmlTextWriter(mem, Encoding.UTF8))
            {
                writer.Formatting = Formatting.Indented;
                xmlDoc.WriteTo(writer);
                writer.Flush();
                mem.Flush();
                mem.Seek(0, SeekOrigin.Begin);
                using (var reader = new StreamReader(mem))
                {
                    var xml = reader.ReadToEnd();
                    
                    TtrsFile attachment = new TtrsFile
                    {
                        File = new StringContent(xml),
                        Filename = Filename(),
                        FileType = "xml",
                        MimeType = "application/xml"
                    };

                    AddFile(attachment);
                }
            }
        }

        private XElement BuildXml(EntranceCode entranceCode, string paymentReference, string debtorReference, string description, 
            double amount, DateTime dueDateTime)
        {
            var returnUrl = BluemConfiguration.Config.ReturnUrl
                .Replace("{entranceCode}", entranceCode.ToString())
                .Replace("{paymentReference}", paymentReference)
                .Replace("{debtorReference}", debtorReference);

            return new XElement("EPaymentInterface",
                new XAttribute("type", "TransactionRequest"),
                new XAttribute("mode", "direct"),
                new XAttribute("senderID", BluemConfiguration.Config.SenderId),
                new XAttribute("version", "1.0"),
                new XAttribute("createDateTime", GetBluemRequestType().DateTime.ToUtc().ToString("yyyy-MM-ddTHH:mm:ss.000Z")),
                new XAttribute("messageCount", 1),

                new XElement("PaymentTransactionRequest",
                    new XAttribute("entranceCode", entranceCode),
                    new XAttribute("brandID", BluemConfiguration.Config.BrandId),
                    new XAttribute("documentType", "PayRequest"),
                    new XAttribute("language", "nl"),

                    new XElement("PaymentReference", paymentReference),
                    new XElement("DebtorReference", debtorReference),
                    new XElement("Description", description),
                    new XElement("SkinID", BluemConfiguration.Config.SkindId),
                    new XElement("Currency", BluemConfiguration.Config.Currency),
                    new XElement("Amount", amount.ToString("F2", CultureInfo.InvariantCulture)),
                    new XElement("DueDateTime", dueDateTime.ToUtc().ToString("yyyy-MM-ddTHH:mm:ss.000Z")),
                    new XElement("DebtorReturnURL",
                        returnUrl
                    )
                )
            );
        }

        private string Filename()
        {
            var request = GetBluemRequestType();
            var requestType = Enum.GetName(typeof(BluemContentTypeEnum), request.ContentType);
            var senderId = BluemConfiguration.Config.SenderId;
            var timestamp = request.DateTime.Iso8601Timestamp;

            return $"{requestType}-{senderId}-BSP1-{timestamp}.xml";
        }
    }

    public interface IBluePaymentRequest : IBluemRequest
    {
        void AddPaymentData(EntranceCode entranceCode, string paymentReference, string debtorReference, string description, 
            double amount, DateTime dueDateTime);
    }

    public static class UtcDateTimeExtension
    {
        public static DateTime ToUtc(this DateTime dateTime)
        {
            return new DateTime(dateTime.Ticks, DateTimeKind.Utc);
        }
    }
}