using SharpCryptoExchange.Converters;
using SharpCryptoExchange.Okx.Enums;
using System.Collections.Generic;

namespace SharpCryptoExchange.Okx.Converters
{
    internal class AccountConverter : BaseConverter<OkxAccount>
    {
        public AccountConverter() : this(true) { }
        public AccountConverter(bool quotes) : base(quotes) { }

        protected override List<KeyValuePair<OkxAccount, string>> Mapping => new List<KeyValuePair<OkxAccount, string>>
        {
            new KeyValuePair<OkxAccount, string>(OkxAccount.Spot, "1"),
            new KeyValuePair<OkxAccount, string>(OkxAccount.Futures, "3"),
            new KeyValuePair<OkxAccount, string>(OkxAccount.Margin, "5"),
            new KeyValuePair<OkxAccount, string>(OkxAccount.Funding, "6"),
            new KeyValuePair<OkxAccount, string>(OkxAccount.Swap, "9"),
            new KeyValuePair<OkxAccount, string>(OkxAccount.Option, "12"),
            new KeyValuePair<OkxAccount, string>(OkxAccount.Unified, "18"),
        };
    }
}