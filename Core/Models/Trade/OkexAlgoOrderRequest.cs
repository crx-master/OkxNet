using Newtonsoft.Json;

namespace SharpCryptoExchange.Okx.Models.Trade
{
    public class OkxAlgoOrderRequest
    {
        [JsonProperty("algoId")]
        public long AlgoOrderId { get; set; }

        [JsonProperty("instId")]
        public string Instrument { get; set; }
    }
}
