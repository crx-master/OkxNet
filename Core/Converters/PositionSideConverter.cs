using SharpCryptoExchange.Converters;
using SharpCryptoExchange.Okx.Enums;
using System.Collections.Generic;

namespace SharpCryptoExchange.Okx.Converters
{
    internal class PositionSideConverter : BaseConverter<OkxPositionSide>
    {
        public PositionSideConverter() : this(true) { }
        public PositionSideConverter(bool quotes) : base(quotes) { }

        protected override List<KeyValuePair<OkxPositionSide, string>> Mapping => new List<KeyValuePair<OkxPositionSide, string>>
        {
            new KeyValuePair<OkxPositionSide, string>(OkxPositionSide.LongPos, "long"),
            new KeyValuePair<OkxPositionSide, string>(OkxPositionSide.ShortPos, "short"),
            new KeyValuePair<OkxPositionSide, string>(OkxPositionSide.NetPos, "net"),
        };
    }
}