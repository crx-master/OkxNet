using SharpCryptoExchange.Converters;
using SharpCryptoExchange.Okx.Enums;
using System.Collections.Generic;

namespace SharpCryptoExchange.Okx.Converters
{
    internal class MarginModeConverter : BaseConverter<OkxMarginMode>
    {
        public MarginModeConverter() : this(true) { }
        public MarginModeConverter(bool quotes) : base(quotes) { }

        protected override List<KeyValuePair<OkxMarginMode, string>> Mapping => new List<KeyValuePair<OkxMarginMode, string>>
        {
            new KeyValuePair<OkxMarginMode, string>(OkxMarginMode.Isolated, "isolated"),
            new KeyValuePair<OkxMarginMode, string>(OkxMarginMode.Cross, "cross"),
        };
    }
}