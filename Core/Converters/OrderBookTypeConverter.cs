using SharpCryptoExchange.Converters;
using SharpCryptoExchange.Okx.Enums;
using System.Collections.Generic;

namespace SharpCryptoExchange.Okx.Converters
{
    internal class OrderBookTypeConverter : BaseConverter<OkxOrderBookType>
    {
        public OrderBookTypeConverter() : this(true) { }
        public OrderBookTypeConverter(bool quotes) : base(quotes) { }

        protected override List<KeyValuePair<OkxOrderBookType, string>> Mapping => new List<KeyValuePair<OkxOrderBookType, string>>
        {
            new KeyValuePair<OkxOrderBookType, string>(OkxOrderBookType.Books, "books"),
            new KeyValuePair<OkxOrderBookType, string>(OkxOrderBookType.Books5, "books5"),
            new KeyValuePair<OkxOrderBookType, string>(OkxOrderBookType.Books50L2Tbt, "books50-l2-tbt"),
            new KeyValuePair<OkxOrderBookType, string>(OkxOrderBookType.BooksL2Tbt, "books-l2-tbt"),
        };
    }
}