using CryptoExchangeNet.Converters;
using OkxNet.Enums;
using System.Collections.Generic;

namespace OkxNet.Converters
{
    internal class ConvertTypeConverter : BaseConverter<OkxConvertType>
    {
        public ConvertTypeConverter() : this(true) { }
        public ConvertTypeConverter(bool quotes) : base(quotes) { }

        protected override List<KeyValuePair<OkxConvertType, string>> Mapping => new List<KeyValuePair<OkxConvertType, string>>
        {
            new KeyValuePair<OkxConvertType, string>(OkxConvertType.CurrencyToContract, "1"),
            new KeyValuePair<OkxConvertType, string>(OkxConvertType.ContractToCurrency, "2"),
        };
    }
}