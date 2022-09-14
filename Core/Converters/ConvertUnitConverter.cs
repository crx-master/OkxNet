using SharpCryptoExchange.Converters;
using SharpCryptoExchange.Okx.Enums;
using System.Collections.Generic;

namespace SharpCryptoExchange.Okx.Converters
{
    internal class ConvertUnitConverter : BaseConverter<OkxConvertUnit>
    {
        public ConvertUnitConverter() : this(true) { }
        public ConvertUnitConverter(bool quotes) : base(quotes) { }

        protected override List<KeyValuePair<OkxConvertUnit, string>> Mapping => new List<KeyValuePair<OkxConvertUnit, string>>
        {
            new KeyValuePair<OkxConvertUnit, string>(OkxConvertUnit.Coin, "coin"),
            new KeyValuePair<OkxConvertUnit, string>(OkxConvertUnit.Usdt, "usdt"),
        };
    }
}