using Newtonsoft.Json;
using SharpCryptoExchange.Okx.Converters;
using System;

namespace SharpCryptoExchange.Okx.Objects.Public
{
    public class OkxTime
    {
        /// <summary>
        /// System time, Unix timestamp format in milliseconds, e.g. 1597026383085
        /// </summary>
        [JsonProperty("ts"), JsonConverter(typeof(OkxTimestampConverter))]
        public DateTimeOffset Time { get; set; }
    }
}
