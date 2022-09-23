using Newtonsoft.Json;

namespace SharpCryptoExchange.Okx.Models.SubAccount
{
    public class OkxSubAccountName
    {
        [JsonProperty("subAcct")]
        public string SubAccountName { get; set; }
    }
}
