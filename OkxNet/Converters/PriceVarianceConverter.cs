using CryptoExchangeNet.Converters;
using OkxNet.Enums;
using System.Collections.Generic;

namespace OkxNet.Converters
{
    internal class PriceVarianceConverter : BaseConverter<OkxPriceVariance>
    {
        public PriceVarianceConverter() : this(true) { }
        public PriceVarianceConverter(bool quotes) : base(quotes) { }

        protected override List<KeyValuePair<OkxPriceVariance, string>> Mapping => new List<KeyValuePair<OkxPriceVariance, string>>
        {
            new KeyValuePair<OkxPriceVariance, string>(OkxPriceVariance.Spread, "pxSpread"),
            new KeyValuePair<OkxPriceVariance, string>(OkxPriceVariance.Variance, "pxVar"),
        };
    }
}