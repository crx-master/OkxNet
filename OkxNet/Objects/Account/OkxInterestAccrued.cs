using Newtonsoft.Json;
using OkxNet.Converters;
using OkxNet.Enums;
using System;

namespace OkxNet.Objects.Account
{
    public class OkxInterestAccrued
    {
        [JsonProperty("instId")]
        public string Instrument { get; set; }

        [JsonProperty("ccy")]
        public string Currency { get; set; }

        [JsonProperty("mgnMode"), JsonConverter(typeof(MarginModeConverter))]
        public OkxMarginMode MarginMode { get; set; }

        [JsonProperty("interest")]
        public decimal? Interest { get; set; }

        [JsonProperty("interestRate")]
        public decimal? InterestRate { get; set; }

        [JsonProperty("liab")]
        public decimal? Liabilities { get; set; }

        [JsonProperty("ts"), JsonConverter(typeof(OkxTimestampConverter))]
        public DateTime Time { get; set; }
    }
}
