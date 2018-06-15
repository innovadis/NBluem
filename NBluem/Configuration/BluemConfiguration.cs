using System;
using System.Configuration;

namespace NBluem.Configuration
{
    public class BluemConfiguration : ConfigurationSection
    {
        private const string DefaultPaymentRequestUri = "https://viamijnbank.net/pr/createTransaction";
        private const string DefaultPaymentStatusUri = "https://viamijnbank.net/pr/requestTransactionStatus";
        private const string DefaultBrandId = "IDEAL";
        private const string DefaultHashSalt = "BluemDefaultHashSalt";
        private const string DefaultSkinId = "PayRequest";
        private const string DefaultCurrency = "EUR";
        private const int DefaultHashLength = 16;

        public const string SectionName = "NBluem.BluemConfiguration";

        private static BluemConfiguration _config;

        /// <summary>Return singleton instance of configuration</summary>
        public static BluemConfiguration Config
        {
            get
            {
                if (_config != null) return _config;
                try
                {
                    _config = (BluemConfiguration)ConfigurationManager.GetSection(SectionName);

                    if (_config == null) throw new Exception($"{SectionName} not found");

                    return _config;
                }
                catch (Exception ex)
                {
                    throw new Exception($"{nameof(BluemConfiguration)}, error: {ex.Message}", ex);
                }
            }
        }

        /// <summary>Certificate thumbprint for certificate to sign request with</summary>
        [ConfigurationProperty("CertificateThumbprint", IsKey = false, IsRequired = true)]
        public string CertificateThumbprint => Convert.ToString(this["CertificateThumbprint"]);

        /// <summary>Certificate thumbprint for certificate to sign request with</summary>
        [ConfigurationProperty("Fingerprint", IsKey = false, IsRequired = true)]
        public string Fingerprint => Convert.ToString(this["Fingerprint"]);

        /// <summary>Certificate thumbprint for certificate to sign request with</summary>
        [ConfigurationProperty("SenderId", IsKey = false, IsRequired = true)]
        public string SenderId => Convert.ToString(this["SenderId"]);

        /// <summary>Certificate thumbprint for certificate to sign request with</summary>
        [ConfigurationProperty("ReturnUrl", IsKey = false, IsRequired = true)]
        public string ReturnUrl => Convert.ToString(this["ReturnUrl"]);

        [ConfigurationProperty("BrandId", IsKey = false, IsRequired = false, DefaultValue = DefaultBrandId)]
        public string BrandId => Convert.ToString(this["BrandId"]);

        /// <summary>Payment request URL</summary>
        [ConfigurationProperty("PaymentRequestUri", IsKey = false, IsRequired = false, DefaultValue = DefaultPaymentRequestUri)]
        public Uri PaymentRequestUri => new Uri(Convert.ToString(this["PaymentRequestUri"]));

        /// <summary>Payment status request URL</summary>
        [ConfigurationProperty("PaymentStatusUri", IsKey = false, IsRequired = false, DefaultValue = DefaultPaymentStatusUri)]
        public Uri PaymentStatusUri => new Uri(Convert.ToString(this["PaymentStatusUri"]));

        /// <summary>Payment status request URL</summary>
        [ConfigurationProperty("HashSalt", IsKey = false, IsRequired = false, DefaultValue = DefaultHashSalt)]
        public string HashSalt => Convert.ToString(this["HashSalt"]);

        /// <summary>Payment status request URL</summary>
        [ConfigurationProperty("HashLength", IsKey = false, IsRequired = false, DefaultValue = DefaultHashLength)]
        public int HashLength => Convert.ToInt32(this["HashLength"]);
        
        [ConfigurationProperty("SkindId", IsKey = false, IsRequired = false, DefaultValue = DefaultSkinId)]
        public string SkindId => Convert.ToString(this["SkindId"]);

        [ConfigurationProperty("Currency", IsKey = false, IsRequired = false, DefaultValue = DefaultCurrency)]
        public string Currency => Convert.ToString(this["Currency"]);

        [ConfigurationProperty("DumpReports", IsKey = false, IsRequired = false, DefaultValue = false)]
        public bool DumpReports { get; set; }
    }
}