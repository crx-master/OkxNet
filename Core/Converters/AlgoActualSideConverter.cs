using SharpCryptoExchange.Converters;
using SharpCryptoExchange.Okx.Enums;
using System.Collections.Generic;

namespace SharpCryptoExchange.Okx.Converters
{
    internal class AlgoActualSideConverter : BaseConverter<OkxAlgoActualSide>
    {
        public AlgoActualSideConverter() : this(true) { }
        public AlgoActualSideConverter(bool quotes) : base(quotes) { }

        protected override List<KeyValuePair<OkxAlgoActualSide, string>> Mapping => new List<KeyValuePair<OkxAlgoActualSide, string>>
        {
            new KeyValuePair<OkxAlgoActualSide, string>(OkxAlgoActualSide.StopLoss, "sl"),
            new KeyValuePair<OkxAlgoActualSide, string>(OkxAlgoActualSide.TakeProfit, "tp"),
        };
    }
}