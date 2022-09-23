using Newtonsoft.Json;
using SharpCryptoExchange.Okx.Converters;
using SharpCryptoExchange.Okx.Enums;

namespace SharpCryptoExchange.Okx.Models.Trade
{
    public class OkxClosePositionResponse
    {
        [JsonProperty("instId")]
        public string Instrument { get; set; }

        [JsonProperty("posSide"), JsonConverter(typeof(PositionSideConverter))]
        public OkxPositionSide PositionSide { get; set; }
    }
}
