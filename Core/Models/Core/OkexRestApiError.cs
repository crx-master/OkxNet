using SharpCryptoExchange.Objects;

namespace SharpCryptoExchange.Okx.Models.Core
{
    public class OkxRestApiError : Error
    {
        public OkxRestApiError(int? code, string message, object data) : base(code, message, data)
        {
        }
    }
}