using Newtonsoft.Json;
using OkxNet.Converters;
using OkxNet.Enums;
using System;

namespace OkxNet.Objects.Market
{
    public class OkxBrickTicker
    {
        [JsonProperty("instType"), JsonConverter(typeof(InstrumentTypeConverter))]
        public OkxInstrumentType InstrumentType { get; set; }

        [JsonProperty("instId")]
        public string Instrument { get; set; }

        /// <summary>
        /// Quote Volume
        /// </summary>
        [JsonProperty("vol24h")]
        public decimal Volume { get; set; }

        /// <summary>
        /// Base Volume
        /// </summary>
        [JsonProperty("volCcy24h")]
        public decimal VolumeCurrency { get; set; }

        [JsonProperty("ts"), JsonConverter(typeof(OkxTimestampConverter))]
        public DateTime Time { get; set; }
    }
}
