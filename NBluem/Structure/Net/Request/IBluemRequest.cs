using System.Net.Http;
using NBluem.Net.RequestTypes;
using NBluem.ValueObjects;

namespace NBluem.Structure.Net.Request
{
    public interface IBluemRequest
    {
        BluemRequestType GetBluemRequestType();

        string BuildSignaturePayload();

        void AddFile(TtrsFile file);

        string GetFileSignature();

        StringContent GetFileContent();

        string GetFileContentAsString();

        string GetFileMimeType();

        string GetFileName();
    }
}