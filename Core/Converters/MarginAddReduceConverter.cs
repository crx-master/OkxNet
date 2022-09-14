using SharpCryptoExchange.Converters;
using SharpCryptoExchange.Okx.Enums;
using System.Collections.Generic;

namespace SharpCryptoExchange.Okx.Converters
{
    internal class MarginAddReduceConverter : BaseConverter<OkxMarginAddReduce>
    {
        public MarginAddReduceConverter() : this(true) { }
        public MarginAddReduceConverter(bool quotes) : base(quotes) { }

        protected override List<KeyValuePair<OkxMarginAddReduce, string>> Mapping => new List<KeyValuePair<OkxMarginAddReduce, string>>
        {
            new KeyValuePair<OkxMarginAddReduce, string>(OkxMarginAddReduce.Add, "add"),
            new KeyValuePair<OkxMarginAddReduce, string>(OkxMarginAddReduce.Reduce, "reduce"),
        };
    }
}