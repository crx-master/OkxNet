using SharpCryptoExchange.Converters;
using SharpCryptoExchange.Okx.Enums;
using System.Collections.Generic;

namespace SharpCryptoExchange.Okx.Converters
{
    internal class OrderCategoryConverter : BaseConverter<OkxOrderCategory>
    {
        public OrderCategoryConverter() : this(true) { }
        public OrderCategoryConverter(bool quotes) : base(quotes) { }

        protected override List<KeyValuePair<OkxOrderCategory, string>> Mapping => new List<KeyValuePair<OkxOrderCategory, string>>
        {
            new KeyValuePair<OkxOrderCategory, string>(OkxOrderCategory.TWAP, "twap"),
            new KeyValuePair<OkxOrderCategory, string>(OkxOrderCategory.ADL, "adl"),
            new KeyValuePair<OkxOrderCategory, string>(OkxOrderCategory.FullLiquidation, "full_liquidation"),
            new KeyValuePair<OkxOrderCategory, string>(OkxOrderCategory.PartialLiquidation, "partial_liquidation"),
            new KeyValuePair<OkxOrderCategory, string>(OkxOrderCategory.Delivery, "delivery"),
        };
    }
}