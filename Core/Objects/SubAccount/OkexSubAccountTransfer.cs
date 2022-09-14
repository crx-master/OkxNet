using Newtonsoft.Json;

namespace SharpCryptoExchange.Okx.Objects.SubAccount
{
    public class OkxSubAccountTransfer
    {
        [JsonProperty("transId")]
        public long? TransferId { get; set; }
    }
}
