using CryptoExchangeNet.Converters;
using OkxNet.Enums;
using System.Collections.Generic;

namespace OkxNet.Converters
{
    internal class OrderSideConverter : BaseConverter<OkxOrderSide>
    {
        public OrderSideConverter() : this(true) { }
        public OrderSideConverter(bool quotes) : base(quotes) { }

        protected override List<KeyValuePair<OkxOrderSide, string>> Mapping => new List<KeyValuePair<OkxOrderSide, string>>
        {
            new KeyValuePair<OkxOrderSide, string>(OkxOrderSide.Buy, "buy"),
            new KeyValuePair<OkxOrderSide, string>(OkxOrderSide.Sell, "sell"),
        };
    }
}