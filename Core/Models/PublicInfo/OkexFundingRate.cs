﻿using Newtonsoft.Json;
using SharpCryptoExchange.Okx.Converters;
using SharpCryptoExchange.Okx.Enums;
using System;

namespace SharpCryptoExchange.Okx.Models.PublicInfo
{
    public class OkxFundingRate
    {
        [JsonProperty("instId")]
        public string Instrument { get; set; }

        [JsonProperty("instType"), JsonConverter(typeof(InstrumentTypeConverter))]
        public OkxInstrumentType InstrumentType { get; set; }

        [JsonProperty("fundingTime"), JsonConverter(typeof(OkxTimestampConverter))]
        public DateTimeOffset FundingTime { get; set; }

        [JsonProperty("fundingRate")]
        public decimal FundingRate { get; set; }

        [JsonProperty("nextFundingTime"), JsonConverter(typeof(OkxTimestampConverter))]
        public DateTimeOffset NextFundingTime { get; set; }

        [JsonProperty("nextFundingRate")]
        public decimal NextFundingRate { get; set; }
    }
}
