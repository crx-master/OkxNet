using SharpCryptoExchange.Converters;
using Newtonsoft.Json;
using SharpCryptoExchange.Okx.Converters;
using System;
using System.Collections.Generic;

namespace SharpCryptoExchange.Okx.Objects.Market
{
    public class OkxOrderBook
    {
        public string Instrument { get; set; }

        [JsonProperty("asks")]
        public IEnumerable<OkxOrderBookRow> Asks { get; set; } = new List<OkxOrderBookRow>();

        [JsonProperty("bids")]
        public IEnumerable<OkxOrderBookRow> Bids { get; set; } = new List<OkxOrderBookRow>();

        [JsonProperty("ts"), JsonConverter(typeof(OkxTimestampConverter))]
        public DateTimeOffset Time { get; set; }

        [JsonProperty("action")]
        public string Action { get; set; }

        [JsonProperty("checksum")]
        public long? Checksum { get; set; }
    }
}
