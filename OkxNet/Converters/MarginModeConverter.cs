using CryptoExchangeNet.Converters;
using OkxNet.Enums;
using System.Collections.Generic;

namespace OkxNet.Converters
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