using Newtonsoft.Json;

namespace SharpCryptoExchange.Okx.Models.Account
{
    public class OkxInterestRate
    {
        [JsonProperty("ccy")]
        public string Currency { get; set; }

        [JsonProperty("interestRate")]
        public decimal? InterestRate { get; set; }
    }
}
