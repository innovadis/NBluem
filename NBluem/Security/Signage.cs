using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using NBluem.Configuration;
using NBluem.Net.Request;
using NBluem.Structure.Net.Request;
using NBluem.Structure.Security;
using NLog;

namespace NBluem.Security
{
    public class Signage : ISignage
    {
        private readonly X509Store _certStore;
        private readonly string _thumbprint;
        private readonly string _fingerprint;
        private readonly ILogger _logger;

        public Signage(X509Store certStore, ILogger logger)
        {
            _logger = logger;
            _certStore = certStore;
            _thumbprint = BluemConfiguration.Config.CertificateThumbprint;
            _fingerprint = BluemConfiguration.Config.Fingerprint;
        }

        public byte[] SignString(string payload)
        {
            try
            {
                _certStore.Open(OpenFlags.ReadOnly);
                var certCollection = _certStore.Certificates.Find(X509FindType.FindByThumbprint, _thumbprint, false);
                _certStore.Close();

                if (certCollection.Count < 1)
                {
                    throw new CryptographicException($"Certificate with with thumbprint {_thumbprint} could not be found");
                }
    
                var certificate = certCollection[0];

                var privateKey = certificate.PrivateKey as RSACryptoServiceProvider;

                if (privateKey == null)
                {
                    throw new CryptographicException($"Unable to obtain private key for certificate with thumbprint {_thumbprint}");
                }

                var buffer = Encoding.Default.GetBytes(payload);
                var signature = privateKey.SignData(buffer, CryptoConfig.MapNameToOID("SHA256"));

                return signature;
            }
            catch (Exception e)
            {
                _logger.Error(e, $"Unable to sign payload with certificate thumbprint {_thumbprint} and payload {payload}");
                throw;
            }
        }

        public string GetBase64EncodedSignatureFromRequest(IBluemRequest request)
        {
            try
            {
                var payload = request.BuildSignaturePayload();

                return $"TTRS {_fingerprint}:{GetBase64EncodedSignature(payload)}";
            }
            catch (Exception e)
            {
                _logger.Error(e, $"Unable to get base64 encoded signature with payload {request.BuildSignaturePayload()}");
                throw;
            }
        }

        public string GetBase64EncodedSignature(string payload)
        {
            try
            {
                var signedRequest = SignString(payload);

                return Convert.ToBase64String(signedRequest);
            }
            catch (Exception e)
            {
                _logger.Error(e, $"Unable to get base64 encoded signature with payload {payload}");
                throw;
            }
        }

        public IBluemSignedRequest SignRequest(IBluemRequest request)
        {
            var requestType = request.GetBluemRequestType();
            requestType.File.Signature = GetBase64EncodedSignature(requestType.File.File.ReadAsStringAsync().Result);

            return new BluemSignedRequest
            {
                DateTime = requestType.DateTime,
                File = requestType.File,
                ContentType = requestType.ContentType,
                RequestSignature = GetBase64EncodedSignatureFromRequest(request),
                RequestType = requestType,
                UnsignedString = request.BuildSignaturePayload()
            };
        }
    }
}