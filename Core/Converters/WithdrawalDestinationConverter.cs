using SharpCryptoExchange.Converters;
using SharpCryptoExchange.Okx.Enums;
using System.Collections.Generic;

namespace SharpCryptoExchange.Okx.Converters
{
    internal class WithdrawalDestinationConverter : BaseConverter<OkxWithdrawalDestination>
    {
        public WithdrawalDestinationConverter() : this(true) { }
        public WithdrawalDestinationConverter(bool quotes) : base(quotes) { }

        protected override List<KeyValuePair<OkxWithdrawalDestination, string>> Mapping => new List<KeyValuePair<OkxWithdrawalDestination, string>>
        {
            new KeyValuePair<OkxWithdrawalDestination, string>(OkxWithdrawalDestination.OKEx, "3"),
            new KeyValuePair<OkxWithdrawalDestination, string>(OkxWithdrawalDestination.DigitalCurrencyAddress, "4"),

        };
    }
}