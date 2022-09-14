using SharpCryptoExchange.Converters;
using Newtonsoft.Json;
using SharpCryptoExchange.Okx.Converters;
using System;

namespace SharpCryptoExchange.Okx.Objects.Trading
{
    [JsonConverter(typeof(ArrayConverter))]
    public class OkxRatio
    {
        [ArrayProperty(0), JsonConverter(typeof(OkxTimestampConverter))]
        public DateTimeOffset Time { get; set; }

        [ArrayProperty(1)]
        public decimal Ratio { get; set; }
    }
}
