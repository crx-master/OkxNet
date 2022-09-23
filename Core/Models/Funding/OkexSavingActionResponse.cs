using Newtonsoft.Json;
using SharpCryptoExchange.Okx.Converters;
using SharpCryptoExchange.Okx.Enums;

namespace SharpCryptoExchange.Okx.Models.Funding
{
    public class OkxSavingActionResponse
    {
        [JsonProperty("ccy")]
        public string Currency { get; set; }

        [JsonProperty("amt")]
        public decimal? Amount { get; set; }

        [JsonProperty("rate")]
        public decimal? PurchaseRate { get; set; }

        [JsonProperty("side"), JsonConverter(typeof(SavingActionSideConverter))]
        public OkxSavingActionSide Side { get; set; }
    }
}