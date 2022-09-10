using Newtonsoft.Json;
using OkxNet.Converters;
using OkxNet.Enums;
using System;

namespace OkxNet.Objects.Public
{
    public class OkxEstimatedPrice
    {
        [JsonProperty("instId")]
        public string Instrument { get; set; }

        [JsonProperty("instType"), JsonConverter(typeof(InstrumentTypeConverter))]
        public OkxInstrumentType InstrumentType { get; set; }

        [JsonProperty("settlePx")]
        public decimal EstimatedPrice { get; set; }

        [JsonProperty("ts"), JsonConverter(typeof(OkxTimestampConverter))]
        public DateTimeOffset Time { get; set; }
    }
}
