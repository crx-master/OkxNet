﻿using Newtonsoft.Json;
using SharpCryptoExchange.Okx.Converters;
using SharpCryptoExchange.Okx.Enums;

namespace SharpCryptoExchange.Okx.Models.Account
{
    public class OkxAccountPositionMode
    {
        [JsonProperty("posMode"), JsonConverter(typeof(PositionModeConverter))]
        public OkxPositionMode PositionMode { get; set; }
    }
}
