using Newtonsoft.Json;
using OkxNet.Converters;
using OkxNet.Enums;
using System;

namespace OkxNet.Objects.Market
{
    public class OkxTrade
    {
        [JsonProperty("instId")]
        public string Instrument { get; set; }

        [JsonProperty("tradeId")]
        public long TradeId { get; set; }

        [JsonProperty("px")]
        public decimal Price { get; set; }

        [JsonProperty("sz")]
        public decimal Quantity { get; set; }

        [JsonProperty("side"), JsonConverter(typeof(TradeSideConverter))]
        public OkxTradeSide Side { get; set; }

        [JsonProperty("ts"), JsonConverter(typeof(OkxTimestampConverter))]
        public DateTime Time { get; set; }
    }
}
