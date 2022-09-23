using Newtonsoft.Json;
using SharpCryptoExchange.Okx.Converters;
using SharpCryptoExchange.Okx.Enums;
using System;

namespace SharpCryptoExchange.Okx.Models.Market
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
        public DateTimeOffset Time { get; set; }
    }
}
