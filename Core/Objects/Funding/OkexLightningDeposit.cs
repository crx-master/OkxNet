using Newtonsoft.Json;
using SharpCryptoExchange.Okx.Converters;
using System;

namespace SharpCryptoExchange.Okx.Objects.Funding
{
    public class OkxLightningDeposit
    {
        [JsonProperty("cTime"), JsonConverter(typeof(OkxTimestampConverter))]
        public DateTimeOffset Time { get; set; }

        [JsonProperty("invoice")]
        public string Invoice { get; set; }
    }
}