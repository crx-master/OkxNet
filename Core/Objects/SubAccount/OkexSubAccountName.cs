using Newtonsoft.Json;

namespace SharpCryptoExchange.Okx.Objects.SubAccount
{
    public class OkxSubAccountName
    {
        [JsonProperty("subAcct")]
        public string SubAccountName { get; set; }
    }
}
