using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using NBluem.Configuration;
using NBluem.Net.Transform;
using NBluem.Structure.Enums;
using NBluem.Structure.Exceptions;
using NBluem.Structure.Net.Request;
using NBluem.Structure.Net.Response;
using NLog;

namespace NBluem.Net
{
    public class BluemClient : IBluemClient
    {
        private static readonly Encoding encoding = Encoding.UTF8;
        private readonly ILogger _logger;

        public BluemClient(ILogger logger)
        {
            _logger = logger;
        }

        public IBluemResponse SendRequest(IBluemSignedRequest request)
        {
            try
            {
                var content = request.File.File.ReadAsStringAsync().Result;
                var bluemContentType = Enum.GetName(typeof(BluemContentTypeEnum), request.ContentType);
                var httpRequest = WebRequest.Create(request.RequestType.RequestUri);

                var formDataBoundary = string.Format("----------{0:N}", Guid.NewGuid());
                var contentType = "multipart/form-data; boundary=" + formDataBoundary;
                var postParameters = new Dictionary<string, Tuple<string, string>>();
                postParameters.Add(request.File.Filename,
                    new Tuple<string, string>(content,
                        $"Content-Type: {request.File.MimeType}; type={bluemContentType}; charset=utf-8;"));
                var formData = GetMultipartFormData(postParameters, formDataBoundary);

                httpRequest.Method = request.RequestType.RequestMethod.Method;
                httpRequest.ContentType = contentType;
                httpRequest.Headers.Add("x-ttrs-authorization", request.RequestSignature);
                httpRequest.Headers.Add("x-ttrs-date", request.DateTime.ToString());
                httpRequest.Headers.Add("x-ttrs-files-count", "1");
                httpRequest.Headers.Add("x-ttrs-filename", $"{request.File.Filename}:{request.File.Signature}");

                try
                {
                    using (var requestStream = httpRequest.GetRequestStream())
                    {
                        requestStream.Write(formData, 0, formData.Length);
                        requestStream.Close();

                        var response = httpRequest.GetResponse() as HttpWebResponse;
                        
                        var transformer = request.RequestType.Transformer;

                        DumpReport(request.UnsignedString, httpRequest, formData);

                        return transformer.GetBluemResponse(response);
                    }
                }
                catch (WebException e)
                {
                    var report = DumpReport(request.UnsignedString, httpRequest, formData);
                    _logger.Error(e, report);

                    throw new BluemRequestException(e);
                }
            }
            catch (Exception e)
            {
                _logger.Error(e);

                throw;
            }
        }

        /**
         * Example found on https://gist.github.com/bgrins/1789787
         */
        private static byte[] GetMultipartFormData(Dictionary<string, Tuple<string, string>> postParameters,
            string boundary)
        {
            Stream formDataStream = new MemoryStream();
            var needsCLRF = false;

            foreach (var param in postParameters)
            {
                // Thanks to feedback from commenters, add a CRLF to allow multiple parameters to be added.
                // Skip it on the first parameter, add it to subsequent parameters.
                if (needsCLRF)
                    formDataStream.Write(encoding.GetBytes("\r\n"), 0, encoding.GetByteCount("\r\n"));

                needsCLRF = true;
                var postData = string.Format(
                    "--{0}\r\nContent-Disposition: form-data; name=\"{1}\"; filename=\"{1}\";\r\n{2}\r\n\r\n{3}",
                    boundary,
                    param.Key,
                    param.Value.Item2,
                    param.Value.Item1);
                formDataStream.Write(encoding.GetBytes(postData), 0, encoding.GetByteCount(postData));
            }

            // Add the end of the request.  Start with a newline
            var footer = "\r\n--" + boundary + "--\r\n";
            formDataStream.Write(encoding.GetBytes(footer), 0, encoding.GetByteCount(footer));

            // Dump the Stream into a byte[]
            formDataStream.Position = 0;
            var formData = new byte[formDataStream.Length];
            formDataStream.Read(formData, 0, formData.Length);
            formDataStream.Close();

            return formData;
        }

        private string DumpReport(string unsignedString, WebRequest request, byte[] formData)
        {
            var report = $"Unsigned string:\r\n{unsignedString}\r\nHeaders:\r\n";
            foreach (var header in request.Headers.AllKeys)
                report += $"{header}: {request.Headers[header]}\r\n";
            report += $"\r\nFormdata:\r\n{Encoding.UTF8.GetString(formData)}";

            if (BluemConfiguration.Config.DumpReports)
            {
                _logger.Info(report);
#if DEBUG

                Debug.WriteLine(report);
#endif
            }

            return report;
        }
    }
}