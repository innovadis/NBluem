using System;
using System.Security.Cryptography.X509Certificates;
using NBluem.Net;
using NBluem.Net.Request;
using NBluem.Net.Request.Factory;
using NBluem.Net.RequestTypes;
using NBluem.Net.Response;
using NBluem.Security;
using NBluem.Structure.Net.Response;
using NBluem.ValueObjects;
using NLog;

namespace NBluem
{
    public class TestClass
    {
        public BluemTransactionResponse DoTest()
        {
            var store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
            var logger = new LogFactory().GetCurrentClassLogger(typeof(TestClass));
            var signage = new Signage(store, logger);

            var paymentRequest = new BluemRequestFactory().CreateRequest(new BluemPaymentRequestType()) as IBluePaymentRequest;
            paymentRequest.AddPaymentData(new EntranceCode(), "foobar", "qooxdoo", "Such description", 12.34, DateTime.Today.AddMonths(1).AddMinutes(10).AddSeconds(21));

            BluemClient client = new BluemClient(logger);
            var signedRequest = signage.SignRequest(paymentRequest);
            var response = (BluemTransactionResponse) client.SendRequest(signedRequest);
            return response;
        }
    }
}