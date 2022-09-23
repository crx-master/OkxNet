using Newtonsoft.Json;
using SharpCryptoExchange.Okx.Converters;
using SharpCryptoExchange.Okx.Enums;
using System;

namespace SharpCryptoExchange.Okx.Models.PublicInfo
{
    public class OkxFundingRateHistory
    {
        [JsonProperty("instId")]
        public string Instrument { get; set; }

        [JsonProperty("instType"), JsonConverter(typeof(InstrumentTypeConverter))]
        public OkxInstrumentType InstrumentType { get; set; }

        [JsonProperty("fundingTime"), JsonConverter(typeof(OkxTimestampConverter))]
        public DateTimeOffset FundingTime { get; set; }

        [JsonProperty("fundingRate")]
        public decimal FundingRate { get; set; }

        [JsonProperty("realizedRate")]
        public decimal RealizedRate { get; set; }
    }
}
