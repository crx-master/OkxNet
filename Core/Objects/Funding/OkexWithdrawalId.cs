using Newtonsoft.Json;

namespace SharpCryptoExchange.Okx.Objects.Funding
{
    public class OkxWithdrawalId
    {
        [JsonProperty("wdId")]
        public string WithdrawalId { get; set; }
    }
}