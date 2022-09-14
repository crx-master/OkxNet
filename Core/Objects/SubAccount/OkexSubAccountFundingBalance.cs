using Newtonsoft.Json;
using SharpCryptoExchange.Okx.Converters;
using System;
using System.Collections.Generic;

namespace SharpCryptoExchange.Okx.Objects.SubAccount
{
    public class OkxSubAccountFundingBalance
    {
        [JsonProperty("availBal")]
        public decimal? AvailableBalance { get; set; }

        [JsonProperty("bal")]
        public decimal? Balance { get; set; }

        [JsonProperty("ccy")]
        public string Currency { get; set; }

        [JsonProperty("frozenBal")]
        public decimal? FrozenBalance { get; set; }
    }
}
