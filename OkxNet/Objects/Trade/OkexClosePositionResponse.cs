using Newtonsoft.Json;
using OkxNet.Converters;
using OkxNet.Enums;

namespace OkxNet.Objects.Trade
{
    public class OkxClosePositionResponse
    {
        [JsonProperty("instId")]
        public string Instrument { get; set; }

        [JsonProperty("posSide"), JsonConverter(typeof(PositionSideConverter))]
        public OkxPositionSide PositionSide { get; set; }
    }
}
