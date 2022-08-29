﻿using Newtonsoft.Json;
using OkxNet.Converters;
using OkxNet.Enums;

namespace OkxNet.Objects.Funding
{
    public class OkxSavingActionResponse
    {
        [JsonProperty("ccy")]
        public string Currency { get; set; }

        [JsonProperty("amt")]
        public decimal? Amount { get; set; }
        
        [JsonProperty("rate")]
        public decimal? PurchaseRate { get; set; }

        [JsonProperty("side"), JsonConverter(typeof(SavingActionSideConverter))]
        public OkxSavingActionSide Side { get; set; }
    }
}