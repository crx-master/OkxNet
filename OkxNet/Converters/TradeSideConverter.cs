using CryptoExchangeNet.Converters;
using OkxNet.Enums;
using System.Collections.Generic;

namespace OkxNet.Converters
{
    internal class TradeSideConverter : BaseConverter<OkxTradeSide>
    {
        public TradeSideConverter() : this(true) { }
        public TradeSideConverter(bool quotes) : base(quotes) { }

        protected override List<KeyValuePair<OkxTradeSide, string>> Mapping => new List<KeyValuePair<OkxTradeSide, string>>
        {
            new KeyValuePair<OkxTradeSide, string>(OkxTradeSide.Buy, "buy"),
            new KeyValuePair<OkxTradeSide, string>(OkxTradeSide.Sell, "sell"),
        };
    }
}