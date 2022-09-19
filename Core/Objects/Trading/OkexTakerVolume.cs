using Newtonsoft.Json;
using SharpCryptoExchange.Converters;
using SharpCryptoExchange.Okx.Converters;
using System;

namespace SharpCryptoExchange.Okx.Objects.Trading
{
    [JsonConverter(typeof(ArrayConverter))]
    public class OkxTakerVolume
    {
        [JsonIgnore]
        public string Currency { get; set; }

        [ArrayProperty(0), JsonConverter(typeof(OkxTimestampConverter))]
        public DateTimeOffset Time { get; set; }

        [ArrayProperty(1)]
        public decimal SellVolume { get; set; }

        [ArrayProperty(2)]
        public decimal BuyVolume { get; set; }
    }
}
