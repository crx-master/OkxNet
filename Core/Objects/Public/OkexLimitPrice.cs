using Newtonsoft.Json;
using SharpCryptoExchange.Okx.Converters;
using SharpCryptoExchange.Okx.Enums;
using System;

namespace SharpCryptoExchange.Okx.Objects.Public
{
    public class OkxLimitPrice
    {
        [JsonProperty("instId")]
        public string Instrument { get; set; }

        [JsonProperty("instType"), JsonConverter(typeof(InstrumentTypeConverter))]
        public OkxInstrumentType InstrumentType { get; set; }

        [JsonProperty("buyLmt")]
        public decimal BuyLimit { get; set; }

        [JsonProperty("sellLmt")]
        public decimal SellLimit { get; set; }

        [JsonProperty("ts"), JsonConverter(typeof(OkxTimestampConverter))]
        public DateTimeOffset Time { get; set; }
    }
}
