using Newtonsoft.Json;
using SharpCryptoExchange.Okx.Converters;
using SharpCryptoExchange.Okx.Enums;
using System;

namespace SharpCryptoExchange.Okx.Objects.Public
{
    public class OkxOpenInterest
    {
        [JsonProperty("instType"), JsonConverter(typeof(InstrumentTypeConverter))]
        public OkxInstrumentType InstrumentType { get; set; }

        [JsonProperty("instId")]
        public string Instrument { get; set; }

        [JsonProperty("oi")]
        public decimal? OpenInterestCont { get; set; }

        [JsonProperty("oiCcy")]
        public decimal? OpenInterestCoin { get; set; }

        [JsonProperty("ts"), JsonConverter(typeof(OkxTimestampConverter))]
        public DateTimeOffset Time { get; set; }
    }
}
