using CryptoExchangeNet.Attributes;
using CryptoExchangeNet.Objects;
using Newtonsoft.Json;

namespace OkxNet.Objects.Core
{
    public class OkxRestApiError : Error
    {
        public OkxRestApiError(int? code, string message, object data) : base(code, message, data)
        {
        }
    }
}