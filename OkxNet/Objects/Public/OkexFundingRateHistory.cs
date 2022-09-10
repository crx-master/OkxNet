﻿using Newtonsoft.Json;
using OkxNet.Converters;
using OkxNet.Enums;
using System;

namespace OkxNet.Objects.Public
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
