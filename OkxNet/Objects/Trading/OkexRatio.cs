using CryptoExchangeNet.Converters;
using Newtonsoft.Json;
using OkxNet.Converters;
using System;

namespace OkxNet.Objects.Trading
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
