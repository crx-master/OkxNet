using Newtonsoft.Json;
using SharpCryptoExchange.Okx.Converters;
using SharpCryptoExchange.Okx.Enums;
using System;

namespace SharpCryptoExchange.Okx.Objects.SubAccount
{
    public class OkxSubAccount
    {
        [JsonProperty("enable")]
        public bool Enable { get; set; }

        [JsonProperty("gAuth")]
        public bool GoogleAuth { get; set; }

        [JsonProperty("subAcct")]
        public string SubAccountName { get; set; }

        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("mobile")]
        public string Mobile { get; set; }

        [JsonProperty("canTransOut")]
        public bool CanTransOut { get; set; }

        [JsonProperty("ts"), JsonConverter(typeof(OkxTimestampConverter))]
        public DateTimeOffset Time { get; set; }

        [JsonProperty("type"), JsonConverter(typeof(SubAccountTypeConverter))]
        public OkxSubAccountType Type { get; set; }
    }
}
