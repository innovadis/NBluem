using NBluem.Structure.Net.Request;

namespace NBluem.Structure.Security
{
    public interface ISignage
    {
        byte[] SignString(string payloa);

        string GetBase64EncodedSignatureFromRequest(IBluemRequest request);

        string GetBase64EncodedSignature(string payload);
    }
}