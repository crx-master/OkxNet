using CryptoExchangeNet.Converters;
using OkxNet.Enums;
using System.Collections.Generic;

namespace OkxNet.Converters
{
    internal class QuantityTypeConverter : BaseConverter<OkxQuantityType>
    {
        public QuantityTypeConverter() : this(true) { }
        public QuantityTypeConverter(bool quotes) : base(quotes) { }

        protected override List<KeyValuePair<OkxQuantityType, string>> Mapping => new List<KeyValuePair<OkxQuantityType, string>>
        {
            new KeyValuePair<OkxQuantityType, string>(OkxQuantityType.BaseCurrency, "base_ccy"),
            new KeyValuePair<OkxQuantityType, string>(OkxQuantityType.QuoteCurrency, "quote_ccy"),
        };
    }
}