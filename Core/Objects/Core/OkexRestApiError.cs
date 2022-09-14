using SharpCryptoExchange.Attributes;
using SharpCryptoExchange.Objects;
using Newtonsoft.Json;

namespace SharpCryptoExchange.Okx.Objects.Core
{
    public class OkxRestApiError : Error
    {
        public OkxRestApiError(int? code, string message, object data) : base(code, message, data)
        {
        }
    }
}