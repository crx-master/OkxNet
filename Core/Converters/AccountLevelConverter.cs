using SharpCryptoExchange.Converters;
using SharpCryptoExchange.Okx.Enums;
using System.Collections.Generic;

namespace SharpCryptoExchange.Okx.Converters
{
    internal class AccountLevelConverter : BaseConverter<OkxAccountLevel>
    {
        public AccountLevelConverter() : this(true) { }
        public AccountLevelConverter(bool quotes) : base(quotes) { }

        protected override List<KeyValuePair<OkxAccountLevel, string>> Mapping => new List<KeyValuePair<OkxAccountLevel, string>>
        {
            new KeyValuePair<OkxAccountLevel, string>(OkxAccountLevel.Simple, "1"),
            new KeyValuePair<OkxAccountLevel, string>(OkxAccountLevel.SingleCurrencyMargin, "2"),
            new KeyValuePair<OkxAccountLevel, string>(OkxAccountLevel.MultiCurrencyMargin, "3"),
            new KeyValuePair<OkxAccountLevel, string>(OkxAccountLevel.PortfolioMargin, "4"),
        };
    }
}