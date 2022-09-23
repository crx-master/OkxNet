using Newtonsoft.Json;
using SharpCryptoExchange.Okx.Converters;
using SharpCryptoExchange.Okx.Enums;
using System;

namespace SharpCryptoExchange.Okx.Models.Account
{
    public class OkxFeeRate
    {
        [JsonProperty("category"), JsonConverter(typeof(FeeRateCategoryConverter))]
        public OkxFeeRateCategory? Category { get; set; }

        [JsonProperty("ts"), JsonConverter(typeof(OkxTimestampConverter))]
        public DateTimeOffset Time { get; set; }

        [JsonProperty("level")]
        public string Level { get; set; }

        [JsonProperty("maker")]
        public decimal? Maker { get; set; }

        [JsonProperty("taker")]
        public decimal? Taker { get; set; }

        [JsonProperty("instType"), JsonConverter(typeof(InstrumentTypeConverter))]
        public OkxInstrumentType InstrumentType { get; set; }

        [JsonProperty("delivery")]
        public decimal? Delivery { get; set; }

        [JsonProperty("exercise")]
        public decimal? Exercise { get; set; }
    }
}
