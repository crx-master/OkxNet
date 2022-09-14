using Newtonsoft.Json;
using SharpCryptoExchange.Okx.Converters;
using SharpCryptoExchange.Okx.Enums;

namespace SharpCryptoExchange.Okx.Objects.Account
{
    public class OkxAccountGreeksType
    {
        [JsonProperty("greeksType"), JsonConverter(typeof(GreeksTypeConverter))]
        public OkxGreeksType GreeksType { get; set; }
    }
}
