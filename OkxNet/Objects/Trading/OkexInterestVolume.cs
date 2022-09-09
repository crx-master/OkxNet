using CryptoExchangeNet.Converters;
using Newtonsoft.Json;
using OkxNet.Converters;
using System;

namespace OkxNet.Objects.Trading
{
    [JsonConverter(typeof(ArrayConverter))]
    public class OkxInterestVolume
    {
        [ArrayProperty(0), JsonConverter(typeof(OkxTimestampConverter))]
        public DateTime Time { get; set; }

        [ArrayProperty(1)]
        public decimal OpenInterest { get; set; }

        [ArrayProperty(2)]
        public decimal Volume { get; set; }
    }
}
