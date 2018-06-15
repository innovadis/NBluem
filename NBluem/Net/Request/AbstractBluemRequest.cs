using System;
using System.Net.Http;
using NBluem.Net.RequestTypes;
using NBluem.Structure.Net.Request;
using NBluem.ValueObjects;

namespace NBluem.Net.Request
{
    public abstract class AbstractBluemRequest : IBluemRequest
    {
        protected BluemRequestType RequestType;

        protected AbstractBluemRequest(BluemRequestType requestType)
        {
            RequestType = requestType;
        }

        public BluemRequestType GetBluemRequestType()
        {
            return RequestType;
        }

        public string BuildSignaturePayload()
        {
            var method = RequestType.RequestMethod.Method.Trim();
            var dateTime = RequestType.DateTime.ToString().Trim();
            var filesCount = RequestType.FilesCount.ToString().Trim();
            var filename = $"{RequestType.File.Filename.Trim()}:{RequestType.File.Signature.Trim()}";
            var host = RequestType.RequestUri.Host.Trim();
            var requestUri = RequestType.RequestUri.AbsolutePath.Trim();

            return $"{String.Empty}{method}\n{dateTime}\n{filesCount}\n{filename}\n{host}\n{requestUri}\n{String.Empty}\n";
        }

        public void AddFile(TtrsFile file)
        {
            RequestType.File = file;
        }

        public string GetFileSignature()
        {
            return RequestType.File.Signature;
        }

        public StringContent GetFileContent()
        {
            return RequestType.File.File;
        }

        public string GetFileContentAsString()
        {
            return GetFileContent().ReadAsStringAsync().Result;
        }

        public string GetFileMimeType()
        {
            return RequestType.File.MimeType;
        }

        public string GetFileName()
        {
            return RequestType.File.Filename;
        }
    }
}