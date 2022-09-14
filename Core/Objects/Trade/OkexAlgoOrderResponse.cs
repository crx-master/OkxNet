using Newtonsoft.Json;

namespace SharpCryptoExchange.Okx.Objects.Trade
{
    public class OkxAlgoOrderResponse
    {
        [JsonProperty("algoId")]
        public long? AlgoOrderId { get; set; }

        [JsonProperty("sCode")]
        public string Code { get; set; }

        [JsonProperty("sMsg")]
        public string Message { get; set; }
    }
}
