﻿using Newtonsoft.Json;

namespace SharpCryptoExchange.Okx.Models.Funding
{
    public class OkxWithdrawalId
    {
        [JsonProperty("wdId")]
        public string WithdrawalId { get; set; }
    }
}