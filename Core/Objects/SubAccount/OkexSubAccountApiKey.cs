using Newtonsoft.Json;
using SharpCryptoExchange.Okx.Converters;
using System;

namespace SharpCryptoExchange.Okx.Objects.SubAccount
{
    public class OkxSubAccountApiKey
    {
        [JsonProperty("subAcct")]
        public string SubAccountName { get; set; }

        [JsonProperty("label")]
        public string Label { get; set; }

        [JsonProperty("apiKey")]
        public string ApiKey { get; set; }

        /*
        [JsonProperty("secretKey")]
        public string secretKey { get; set; }

        [JsonProperty("Passphrase")]
        public string Passphrase { get; set; }

        [JsonProperty("perm"), JsonConverter(typeof(ApiPermissionConverter))]
        public OkxApiPermission Permission { get; set; }
        */

        [JsonProperty("perm")]
        public string Permissions { get; set; }

        [JsonProperty("ip")]
        public string IpAddresses { get; set; }

        [JsonProperty("ts"), JsonConverter(typeof(OkxTimestampConverter))]
        public DateTimeOffset Time { get; set; }
    }
}
