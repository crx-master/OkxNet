using Newtonsoft.Json;

namespace SharpCryptoExchange.Okx.Models.SubAccount
{
    public class OkxSubAccountTransfer
    {
        [JsonProperty("transId")]
        public long? TransferId { get; set; }
    }
}
