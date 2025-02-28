﻿using Newtonsoft.Json;

namespace SharpCryptoExchange.Okx.Models.Account
{
    public class OkxMaximumAvailableAmount
    {
        [JsonProperty("instId")]
        public string Instrument { get; set; }

        [JsonProperty("availBuy")]
        public decimal? AvailableBuy { get; set; }

        [JsonProperty("availSell")]
        public decimal? AvailableSell { get; set; }
    }
}
