using SharpCryptoExchange.Converters;
using SharpCryptoExchange.Okx.Enums;
using System.Collections.Generic;

namespace SharpCryptoExchange.Okx.Converters
{
    internal class TradeModeConverter : BaseConverter<OkxTradeMode>
    {
        public TradeModeConverter() : this(true) { }
        public TradeModeConverter(bool quotes) : base(quotes) { }

        protected override List<KeyValuePair<OkxTradeMode, string>> Mapping => new List<KeyValuePair<OkxTradeMode, string>>
        {
            new KeyValuePair<OkxTradeMode, string>(OkxTradeMode.Cash, "cash"),
            new KeyValuePair<OkxTradeMode, string>(OkxTradeMode.Cross, "cross"),
            new KeyValuePair<OkxTradeMode, string>(OkxTradeMode.Isolated, "isolated"),
        };
    }
}