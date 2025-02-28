﻿using SharpCryptoExchange.Converters;
using SharpCryptoExchange.Okx.Enums;
using System.Collections.Generic;

namespace SharpCryptoExchange.Okx.Converters
{
    internal class FundingBillTypeConverter : BaseConverter<OkxFundingBillType>
    {
        public FundingBillTypeConverter() : this(true) { }
        public FundingBillTypeConverter(bool quotes) : base(quotes) { }

        protected override List<KeyValuePair<OkxFundingBillType, string>> Mapping => new List<KeyValuePair<OkxFundingBillType, string>>
        {
             new KeyValuePair<OkxFundingBillType, string>(OkxFundingBillType.Deposit, "1"),
             new KeyValuePair<OkxFundingBillType, string>(OkxFundingBillType.Withdrawal, "2"),
             new KeyValuePair<OkxFundingBillType, string>(OkxFundingBillType.CanceledWithdrawal, "13"),
             new KeyValuePair<OkxFundingBillType, string>(OkxFundingBillType.TransferToFutures, "18"),
             new KeyValuePair<OkxFundingBillType, string>(OkxFundingBillType.TransferFromFutures, "19"),
             new KeyValuePair<OkxFundingBillType, string>(OkxFundingBillType.TransferToSubAccount, "20"),
             new KeyValuePair<OkxFundingBillType, string>(OkxFundingBillType.TransferFromSubAccount, "21"),
             new KeyValuePair<OkxFundingBillType, string>(OkxFundingBillType.Claim, "28"),
             new KeyValuePair<OkxFundingBillType, string>(OkxFundingBillType.TransferToMargin, "33"),
             new KeyValuePair<OkxFundingBillType, string>(OkxFundingBillType.TransferFromMargin, "34"),
             new KeyValuePair<OkxFundingBillType, string>(OkxFundingBillType.TransferToSpot, "37"),
             new KeyValuePair<OkxFundingBillType, string>(OkxFundingBillType.TransferFromSpot, "38"),
             new KeyValuePair<OkxFundingBillType, string>(OkxFundingBillType.TradingFeesSettledByLoyaltyPoints, "41"),
             new KeyValuePair<OkxFundingBillType, string>(OkxFundingBillType.LoyaltyPointsPurchase, "42"),
             new KeyValuePair<OkxFundingBillType, string>(OkxFundingBillType.SystemReversal, "47"),
             new KeyValuePair<OkxFundingBillType, string>(OkxFundingBillType.ReceivedFromActivities, "48"),
             new KeyValuePair<OkxFundingBillType, string>(OkxFundingBillType.GivenAwayToActivities, "49"),
             new KeyValuePair<OkxFundingBillType, string>(OkxFundingBillType.ReceivedFromAppointments, "50"),
             new KeyValuePair<OkxFundingBillType, string>(OkxFundingBillType.DeductedFromAppointments, "51"),
             new KeyValuePair<OkxFundingBillType, string>(OkxFundingBillType.RedPacketSent, "52"),
             new KeyValuePair<OkxFundingBillType, string>(OkxFundingBillType.RedPacketSnatched, "53"),
             new KeyValuePair<OkxFundingBillType, string>(OkxFundingBillType.RedPacketRefunded, "54"),
             new KeyValuePair<OkxFundingBillType, string>(OkxFundingBillType.TransferToPerpetual, "55"),
             new KeyValuePair<OkxFundingBillType, string>(OkxFundingBillType.TransferFromPerpetual, "56"),
             new KeyValuePair<OkxFundingBillType, string>(OkxFundingBillType.TransferFromHedgingAccount, "59"),
             new KeyValuePair<OkxFundingBillType, string>(OkxFundingBillType.TransferToHedgingAccount, "60"),
             new KeyValuePair<OkxFundingBillType, string>(OkxFundingBillType.Conversion, "61"),
             new KeyValuePair<OkxFundingBillType, string>(OkxFundingBillType.TransferToOptions, "63"),
             new KeyValuePair<OkxFundingBillType, string>(OkxFundingBillType.TransferFromOptions, "62"),
             new KeyValuePair<OkxFundingBillType, string>(OkxFundingBillType.ClaimRebateCard, "68"),
             new KeyValuePair<OkxFundingBillType, string>(OkxFundingBillType.DistributeRebateCard, "69"),
             new KeyValuePair<OkxFundingBillType, string>(OkxFundingBillType.TokenReceived, "72"),
             new KeyValuePair<OkxFundingBillType, string>(OkxFundingBillType.TokenGivenAway, "73"),
             new KeyValuePair<OkxFundingBillType, string>(OkxFundingBillType.TokenRefunded, "74"),
             new KeyValuePair<OkxFundingBillType, string>(OkxFundingBillType.SubscriptionToSavings, "75"),
             new KeyValuePair<OkxFundingBillType, string>(OkxFundingBillType.RedemptionToSavings, "76"),
             new KeyValuePair<OkxFundingBillType, string>(OkxFundingBillType.Distribute, "77"),
             new KeyValuePair<OkxFundingBillType, string>(OkxFundingBillType.LockUp, "78"),
             new KeyValuePair<OkxFundingBillType, string>(OkxFundingBillType.NodeVoting, "79"),
             new KeyValuePair<OkxFundingBillType, string>(OkxFundingBillType.Staking80, "80"),
             new KeyValuePair<OkxFundingBillType, string>(OkxFundingBillType.VoteRedemption, "81"),
             new KeyValuePair<OkxFundingBillType, string>(OkxFundingBillType.StakingRedemption82, "82"),
             new KeyValuePair<OkxFundingBillType, string>(OkxFundingBillType.StakingYield, "83"),
             new KeyValuePair<OkxFundingBillType, string>(OkxFundingBillType.ViolationFee, "84"),
             new KeyValuePair<OkxFundingBillType, string>(OkxFundingBillType.PowMiningYield, "85"),
             new KeyValuePair<OkxFundingBillType, string>(OkxFundingBillType.CloudMiningPay, "86"),
             new KeyValuePair<OkxFundingBillType, string>(OkxFundingBillType.CloudMiningYield, "87"),
             new KeyValuePair<OkxFundingBillType, string>(OkxFundingBillType.Subsidy, "88"),
             new KeyValuePair<OkxFundingBillType, string>(OkxFundingBillType.Staking89, "89"),
             new KeyValuePair<OkxFundingBillType, string>(OkxFundingBillType.StakingSubscription, "90"),
             new KeyValuePair<OkxFundingBillType, string>(OkxFundingBillType.StakingRedemption91, "91"),
             new KeyValuePair<OkxFundingBillType, string>(OkxFundingBillType.AddCollateral, "92"),
             new KeyValuePair<OkxFundingBillType, string>(OkxFundingBillType.RedeemCollateral, "93"),
             new KeyValuePair<OkxFundingBillType, string>(OkxFundingBillType.Investment, "94"),
             new KeyValuePair<OkxFundingBillType, string>(OkxFundingBillType.BorrowerBorrows, "95"),
             new KeyValuePair<OkxFundingBillType, string>(OkxFundingBillType.PrincipalTransferredIn, "96"),
             new KeyValuePair<OkxFundingBillType, string>(OkxFundingBillType.BorrowerTransferredLoanOut, "97"),
             new KeyValuePair<OkxFundingBillType, string>(OkxFundingBillType.BorrowerTransferredInterestOut, "98"),
             new KeyValuePair<OkxFundingBillType, string>(OkxFundingBillType.InvestorTransferredInterestIn, "99"),
             new KeyValuePair<OkxFundingBillType, string>(OkxFundingBillType.PrepaymentPenaltyTransferredIn, "102"),
             new KeyValuePair<OkxFundingBillType, string>(OkxFundingBillType.PrepaymentPenaltyTransferredOut, "103"),
             new KeyValuePair<OkxFundingBillType, string>(OkxFundingBillType.FeeTransferredIn, "104"),
             new KeyValuePair<OkxFundingBillType, string>(OkxFundingBillType.FeeTransferredOut, "105"),
             new KeyValuePair<OkxFundingBillType, string>(OkxFundingBillType.OverdueFeeTransferredIn, "106"),
             new KeyValuePair<OkxFundingBillType, string>(OkxFundingBillType.OverdueFeeTransferredOut, "107"),
             new KeyValuePair<OkxFundingBillType, string>(OkxFundingBillType.OverdueInterestTransferredOut, "108"),
             new KeyValuePair<OkxFundingBillType, string>(OkxFundingBillType.OverdueInterestTransferredIn, "109"),
             new KeyValuePair<OkxFundingBillType, string>(OkxFundingBillType.CollateralForClosedPositionTransferredIn, "110"),
             new KeyValuePair<OkxFundingBillType, string>(OkxFundingBillType.CollateralForClosedPositionTransferredOut, "111"),
             new KeyValuePair<OkxFundingBillType, string>(OkxFundingBillType.CollateralForLiquidationTransferredIn, "112"),
             new KeyValuePair<OkxFundingBillType, string>(OkxFundingBillType.CollateralForLiquidationTransferredOut, "113"),
             new KeyValuePair<OkxFundingBillType, string>(OkxFundingBillType.InsuranceFundTransferredIn, "114"),
             new KeyValuePair<OkxFundingBillType, string>(OkxFundingBillType.InsuranceFundTransferredOut, "115"),
             new KeyValuePair<OkxFundingBillType, string>(OkxFundingBillType.PlaceAnOrder, "116"),
             new KeyValuePair<OkxFundingBillType, string>(OkxFundingBillType.FulfillAnOrder, "117"),
             new KeyValuePair<OkxFundingBillType, string>(OkxFundingBillType.CancelAnOrder, "118"),
             new KeyValuePair<OkxFundingBillType, string>(OkxFundingBillType.MerchantsUnlockDeposit, "119"),
             new KeyValuePair<OkxFundingBillType, string>(OkxFundingBillType.MerchantsAddDeposit, "120"),
             new KeyValuePair<OkxFundingBillType, string>(OkxFundingBillType.FiatgatewayPlaceAnOrder, "121"),
             new KeyValuePair<OkxFundingBillType, string>(OkxFundingBillType.FiatgatewayCancelAnOrder, "122"),
             new KeyValuePair<OkxFundingBillType, string>(OkxFundingBillType.FiatgatewayFulfillAnOrder, "123"),
             new KeyValuePair<OkxFundingBillType, string>(OkxFundingBillType.JumpstartUnlocking, "124"),
             new KeyValuePair<OkxFundingBillType, string>(OkxFundingBillType.ManualDeposit, "125"),
             new KeyValuePair<OkxFundingBillType, string>(OkxFundingBillType.InterestDeposit, "126"),
             new KeyValuePair<OkxFundingBillType, string>(OkxFundingBillType.InvestmentFeeTransferredIn, "127"),
             new KeyValuePair<OkxFundingBillType, string>(OkxFundingBillType.InvestmentFeeTransferredOut, "128"),
             new KeyValuePair<OkxFundingBillType, string>(OkxFundingBillType.RewardsTransferredIn, "129"),
             new KeyValuePair<OkxFundingBillType, string>(OkxFundingBillType.TransferredFromUnifiedAccount, "130"),
             new KeyValuePair<OkxFundingBillType, string>(OkxFundingBillType.TransferredToUnifiedAccount, "131"),
        };
    }
}