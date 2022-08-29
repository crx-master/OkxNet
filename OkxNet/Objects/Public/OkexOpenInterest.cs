using Newtonsoft.Json;
using OkxNet.Converters;
using OkxNet.Enums;
using System;

namespace OkxNet.Objects.Public
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
        public DateTime Time { get; set; }
    }
}
