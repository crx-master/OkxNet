﻿using Newtonsoft.Json;
using SharpCryptoExchange.Okx.Converters;
using SharpCryptoExchange.Okx.Enums;

namespace SharpCryptoExchange.Okx.Objects.Account
{
    public class OkxMarginAmount
    {
        [JsonProperty("instId")]
        public string Instrument { get; set; }

        [JsonProperty("posSide"), JsonConverter(typeof(PositionSideConverter))]
        public OkxPositionSide? PositionSide { get; set; }

        [JsonProperty("amt")]
        public decimal? Amount { get; set; }

        [JsonProperty("type"), JsonConverter(typeof(MarginAddReduceConverter))]
        public OkxMarginAddReduce? MarginAddReduce { get; set; }
    }
}
