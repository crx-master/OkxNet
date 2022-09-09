using CryptoExchangeNet.Attributes;
using CryptoExchangeNet.Converters;
using Newtonsoft.Json;
using OkxNet.Converters;
using System;

namespace OkxNet.Objects.Trading
{
    [JsonConverter(typeof(ArrayConverter))]
    public class OkxTakerVolume
    {
        [JsonIgnore]
        public string Currency { get; set; }

        [ArrayProperty(0), JsonConverter(typeof(OkxTimestampConverter))]
        public DateTime Time { get; set; }

        [ArrayProperty(1)]
        public decimal SellVolume { get; set; }

        [ArrayProperty(2)]
        public decimal BuyVolume { get; set; }
    }
}
