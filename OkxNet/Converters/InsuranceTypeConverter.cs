using CryptoExchange.Net.Converters;
using OkxNet.Enums;
using System.Collections.Generic;

namespace OkxNet.Converters
{
    internal class InsuranceTypeConverter : BaseConverter<OkxInsuranceType>
    {
        public InsuranceTypeConverter() : this(true) { }
        public InsuranceTypeConverter(bool quotes) : base(quotes) { }

        protected override List<KeyValuePair<OkxInsuranceType, string>> Mapping => new List<KeyValuePair<OkxInsuranceType, string>>
        {
            new KeyValuePair<OkxInsuranceType, string>(OkxInsuranceType.All, "all"),
            new KeyValuePair<OkxInsuranceType, string>(OkxInsuranceType.LiquidationBalanceDeposit, "liquidation_balance_deposit"),
            new KeyValuePair<OkxInsuranceType, string>(OkxInsuranceType.BankruptcyLoss, "bankruptcy_loss"),
            new KeyValuePair<OkxInsuranceType, string>(OkxInsuranceType.PlatformRevenue, "platform_revenue"),
        };
    }
}