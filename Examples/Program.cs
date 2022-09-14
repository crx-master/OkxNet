using Microsoft.Extensions.Logging;
using SharpCryptoExchange.Okx;
using SharpCryptoExchange.Okx.Enums;
using SharpCryptoExchange.Okx.Objects.Core;
using SharpCryptoExchange.Sockets;
using System;
using System.Collections.Generic;

namespace OkxNet.Examples
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            #region Rest Api Client
            // OKEx Rest Api Client
            OkxClient api = new(new OkxClientOptions { LogLevel = LogLevel.Debug, OutputOriginalData=true });
            api.SetApiCredentials("XXXXXXXX-API-KEY-XXXXXXXX", "XXXXXXXX-API-SECRET-XXXXXXXX", "XXXXXXXX-API-PASSPHRASE-XXXXXXXX");
            var market_05 = api.GetCandlesticks("BTC-USDT", OkxPeriod.OneHour);

#if FALSE
            /* Public Endpoints (Unsigned) */
            var public_01 = api.GetInstruments(OkxInstrumentType.Spot);
            var public_02 = api.GetInstruments(OkxInstrumentType.Margin);
            var public_03 = api.GetInstruments(OkxInstrumentType.Swap);
            var public_04 = api.GetInstruments(OkxInstrumentType.Futures);
            var public_05 = api.GetInstruments(OkxInstrumentType.Option, "USD");
            var public_06 = api.GetDeliveryExerciseHistory(OkxInstrumentType.Futures, "BTC-USD");
            var public_07 = api.GetDeliveryExerciseHistory(OkxInstrumentType.Option, "BTC-USD");
            var public_08 = api.GetOpenInterests(OkxInstrumentType.Futures);
            var public_09 = api.GetOpenInterests(OkxInstrumentType.Option, "BTC-USD");
            var public_10 = api.GetOpenInterests(OkxInstrumentType.Swap, "BTC-USD");
            var public_11 = api.GetFundingRates("BTC-USD-SWAP");
            var public_12 = api.GetFundingRateHistory("BTC-USD-SWAP");
            var public_13 = api.GetLimitPrice("BTC-USD-SWAP");
            var public_14 = api.GetOptionMarketData("BTC-USD");
            var public_15 = api.GetEstimatedPrice("BTC-USD-211004-41000-C");
            var public_16 = api.GetDiscountInfo();
            var public_17 = api.GetSystemTime();
            var public_18 = api.GetLiquidationOrders(OkxInstrumentType.Futures, underlying: "BTC-USD", alias: OkxInstrumentAlias.Quarter, state: OkxLiquidationState.Unfilled);
            var public_19 = api.GetMarkPrices(OkxInstrumentType.Futures);
            var public_20 = api.GetPositionTiers(OkxInstrumentType.Futures, OkxMarginMode.Isolated, "BTC-USD");
            var public_21 = api.GetInterestRates();
            var public_22 = api.GetVIPInterestRates();
            var public_23 = api.GetUnderlying(OkxInstrumentType.Futures);
            var public_24 = api.GetUnderlying(OkxInstrumentType.Option);
            var public_25 = api.GetUnderlying(OkxInstrumentType.Swap);
            var public_26 = api.GetInsuranceFund(OkxInstrumentType.Margin, currency:"BTC");
            var public_27 = api.UnitConvert( OkxConvertType.CurrencyToContract, instrumentId:"BTC-USD-SWAP", price:35000, size:0.888m);
#endif

#if FALSE
            /* Market Endpoints (Unsigned) */
            var market_01 = api.GetTickers(OkxInstrumentType.Spot);
            var market_02 = api.GetTicker("BTC-USDT");
            var market_03 = api.GetIndexTickers(instrumentId: "BTC-USDT");
            var market_04 = api.GetOrderBook("BTC-USDT", 40);
            var market_05 = api.GetCandlesticks("BTC-USDT", OkxPeriod.OneHour);
            var market_06 = api.GetCandlesticksHistory("BTC-USDT", OkxPeriod.OneHour);
            var market_07 = api.GetIndexCandlesticks("BTC-USDT", OkxPeriod.OneHour);
            var market_08 = api.GetMarkPriceCandlesticks("BTC-USDT", OkxPeriod.OneHour);
            var market_09 = api.GetTrades("BTC-USDT");
            var market_10 = api.GetTradesHistory("BTC-USDT");
            var market_11 = api.Get24HourVolume();
            var market_12 = api.GetOracle();
            var market_13 = api.GetIndexComponents("BTC-USDT");
            var market_14 = api.GetBlockTickers(OkxInstrumentType.Spot);
            var market_15 = api.GetBlockTickers(OkxInstrumentType.Futures);
            var market_16 = api.GetBlockTickers(OkxInstrumentType.Option);
            var market_17 = api.GetBlockTickers(OkxInstrumentType.Swap);
            var market_18 = api.GetBlockTicker("BTC-USDT");
            var market_19 = api.GetBlockTrades("BTC-USDT");
#endif

#if FALSE
            /* Trading Endpoints (Unsigned) */
            var rubik_01 = api.GetRubikSupportCoin();
            var rubik_02 = api.GetRubikTakerVolume("BTC", OkxInstrumentType.Spot);
            var rubik_03 = api.GetRubikMarginLendingRatio("BTC", OkxPeriod.OneDay);
            var rubik_04 = api.GetRubikLongShortRatio("BTC", OkxPeriod.OneDay);
            var rubik_05 = api.GetRubikContractSummary("BTC", OkxPeriod.OneDay);
            var rubik_06 = api.GetRubikOptionsSummary("BTC", OkxPeriod.OneDay);
            var rubik_07 = api.GetRubikPutCallRatio("BTC", OkxPeriod.OneDay);
            var rubik_08 = api.GetRubikInterestVolumeExpiry("BTC", OkxPeriod.OneDay);
            var rubik_09 = api.GetRubikInterestVolumeStrike("BTC", "20210623", OkxPeriod.OneDay);
            var rubik_10 = api.GetRubikTakerFlow("BTC", OkxPeriod.OneDay);
#endif

#if FALSE
            /* Account Endpoints (Signed) */
            var account_01 = api.GetAccountBalance();
            var account_02 = api.GetAccountPositions();
            var account_03 = api.GetAccountPositionRisk();
            var account_04 = api.GetBillHistory();
            var account_05 = api.GetBillArchive();
            var account_06 = api.GetAccountConfiguration();
            var account_07 = api.SetAccountPositionMode(OkxPositionMode.LongShortMode);
            var account_08 = api.GetAccountLeverage("BTC-USD-211008", OkxMarginMode.Isolated);
            var account_09 = api.SetAccountLeverage(30, null, "BTC-USD-211008", OkxMarginMode.Isolated, OkxPositionSide.Long);
            var account_10 = api.GetMaximumAmount("BTC-USDT", OkxTradeMode.Isolated);
            var account_11 = api.GetMaximumAvailableAmount("BTC-USDT", OkxTradeMode.Isolated);
            var account_12 = api.SetMarginAmount("BTC-USDT", OkxPositionSide.Long, OkxMarginAddReduce.Add, 100.0m);
            var account_13 = api.GetMaximumLoanAmount("BTC-USDT", OkxMarginMode.Cross);
            var account_14 = api.GetFeeRates(OkxInstrumentType.Spot, category: OkxFeeRateCategory.ClassA);
            var account_15 = api.GetFeeRates(OkxInstrumentType.Futures, category: OkxFeeRateCategory.ClassA);
            var account_16 = api.GetInterestAccrued();
            var account_17 = api.GetInterestRate();
            var account_18 = api.SetGreeks(OkxGreeksType.GreeksInCoins);
            var account_19 = api.GetMaximumWithdrawals();
#endif

#if FALSE
            /* SubAccount Endpoints (Signed) */
            var subaccount_01 = api.GetSubAccounts();
            var subaccount_02 = api.ResetSubAccountApiKey("subAccountName", "apiKey", "apiLabel", true, true, "");
            var subaccount_03 = api.GetSubAccountTradingBalances("subAccountName");
            var subaccount_04 = api.GetSubAccountFundingBalances("subAccountName");
            var subaccount_05 = api.GetSubAccountBills();
            var subaccount_06 = api.TransferBetweenSubAccounts("BTC", 0.5m, OkxAccount.Funding, OkxAccount.Unified, "fromSubAccountName", "toSubAccountName");
#endif

#if FALSE
            /* Funding Endpoints (Signed) */
            var funding_01 = api.GetCurrencies();
            var funding_02 = api.GetFundingBalance();
            var funding_03 = api.FundTransfer("BTC", 0.5m, OkxTransferType.TransferWithinAccount, OkxAccount.Margin, OkxAccount.Spot);
            var funding_04 = api.GetFundingBillDetails("BTC");
            var funding_05 = api.GetLightningDeposits("BTC", 0.001m);
            var funding_06 = api.GetDepositAddress("BTC");
            var funding_07 = api.GetDepositAddress("USDT");
            var funding_08 = api.GetDepositHistory("USDT");
            var funding_09 = api.Withdraw("USDT", 100.0m, OkxWithdrawalDestination.DigitalCurrencyAddress, "toAddress", "password", 1.0m, "USDT-TRC20");
            var funding_10 = api.GetLightningWithdrawals("BTC", "invoice", "password");
            var funding_11 = api.GetWithdrawalHistory("USDT");
            var funding_12 = api.GetSavingBalances();
            var funding_13 = api.SavingPurchaseRedemption("USDT", 10.0m, OkxSavingActionSide.Purchase);
#endif

#if FALSE
            /* Trade Endpoints (Signed) */
            var trade_01 = api.PlaceOrder("BTC-USDT", OkxTradeMode.Cash, OkxOrderSide.Buy, OkxPositionSide.Long, OkxOrderType.MarketOrder, 0.1m);
            var trade_02 = api.PlaceMultipleOrders(new List<OkxOrderPlaceRequest>());
            var trade_03 = api.CancelOrder("BTC-USDT");
            var trade_04 = api.CancelMultipleOrders(new List<OkxOrderCancelRequest>());
            var trade_05 = api.AmendOrder("BTC-USDT");
            var trade_06 = api.AmendMultipleOrders(new List<OkxOrderAmendRequest>());
            var trade_07 = api.ClosePosition("BTC-USDT", OkxMarginMode.Isolated);
            var trade_08 = api.GetOrderDetails("BTC-USDT");
            var trade_09 = api.GetOrderList();
            var trade_10 = api.GetOrderHistory(OkxInstrumentType.Swap);
            var trade_11 = api.GetOrderArchive(OkxInstrumentType.Futures);
            var trade_12 = api.GetTransactionHistory();
            var trade_13 = api.GetTransactionArchive(OkxInstrumentType.Futures);
            var trade_14 = api.PlaceAlgoOrder("BTC-USDT", OkxTradeMode.Isolated, OkxOrderSide.Sell, OkxAlgoOrderType.Conditional, 0.1m);
            var trade_15 = api.CancelAlgoOrder(new List<OkxAlgoOrderRequest>());
            var trade_16 = api.CancelAdvanceAlgoOrder(new List<OkxAlgoOrderRequest>());
            var trade_17 = api.GetAlgoOrderList(OkxAlgoOrderType.OCO);
            var trade_18 = api.GetAlgoOrderHistory(OkxAlgoOrderType.Conditional);
#endif
            #endregion

            #region Socket Api Client
            /* OKEx Socket Client */
            var ws = new OkxSocketClient();
            ws.SetApiCredentials("XXXXXXXX-API-KEY-XXXXXXXX", "XXXXXXXX-API-SECRET-XXXXXXXX", "XXXXXXXX-API-PASSPHRASE-XXXXXXXX");

            /* Sample Pairs */
            var sample_pairs = new List<string> { "BTC-USDT", "LTC-USDT", "ETH-USDT", "XRP-USDT", "BCH-USDT", "EOS-USDT", "OKB-USDT", "ETC-USDT", "TRX-USDT", "BSV-USDT", "DASH-USDT", "NEO-USDT", "QTUM-USDT", "XLM-USDT", "ADA-USDT", "AE-USDT", "BLOC-USDT", "EGT-USDT", "IOTA-USDT", "SC-USDT", "WXT-USDT", "ZEC-USDT", };

            /* WS Subscriptions */
            var subs = new List<UpdateSubscription>();

#if FALSE
            /* Instruments (Public) */
            ws.SubscribeToInstruments(OkxInstrumentType.Spot, (data) =>
            {
                if (data != null)
                {
                    // ... Your logic is here
                    Console.WriteLine($"Instrument {data.Instrument} BaseCurrency:{data.BaseCurrency} Category:{data.Category}");
                }
            });
#endif

#if FALSE
            /* Tickers (Public) */
            foreach (var pair in sample_pairs)
            {
                var subscription = ws.SubscribeToTickers(pair, (data) =>
                {
                    if (data != null)
                    {
                        // ... Your logic is here
                        Console.WriteLine($"Ticker {data.Instrument} Ask:{data.AskPrice} Bid:{data.BidPrice}");
                    }
                });
                subs.Add(subscription.Data);
            }
#endif

#if FALSE
            /* Unsubscribe */
            foreach (var sub in subs)
            {
                _ = ws.UnsubscribeAsync(sub);
            }
#endif

#if FALSE
            /* Interests (Public) */
            foreach (var pair in sample_pairs)
            {
                ws.SubscribeToInterests(pair, (data) =>
                {
                    if (data != null)
                    {
                        // ... Your logic is here
                    }
                });
            }
#endif

#if FALSE
            /* Candlesticks (Public) */
            foreach (var pair in sample_pairs)
            {
                ws.SubscribeToCandlesticks(pair, OkxPeriod.FiveMinutes, (data) =>
                {
                    if (data != null)
                    {
                        // ... Your logic is here
                    }
                });
            }
#endif

#if FALSE
            /* Trades (Public) */
            foreach (var pair in sample_pairs)
            {
                ws.SubscribeToTrades(pair, (data) =>
                {
                    if (data != null)
                    {
                        // ... Your logic is here
                    }
                });
            }
#endif

#if FALSE
            /* Estimated Price (Public) */
            foreach (var pair in sample_pairs)
            {
                ws.SubscribeToTrades(pair, (data) =>
                {
                    if (data != null)
                    {
                        // ... Your logic is here
                    }
                });
            }
#endif

#if FALSE
            /* Mark Price (Public) */
            foreach (var pair in sample_pairs)
            {
                ws.SubscribeToMarkPrice(pair, (data) =>
                {
                    if (data != null)
                    {
                        // ... Your logic is here
                    }
                });
            }
#endif

#if FALSE
            /* Mark Price Candlesticks (Public) */
            foreach (var pair in sample_pairs)
            {
                ws.SubscribeToMarkPriceCandlesticks(pair, OkxPeriod.FiveMinutes, (data) =>
                {
                    if (data != null)
                    {
                        // ... Your logic is here
                    }
                });
            }
#endif

#if FALSE
            /* Limit Price (Public) */
            foreach (var pair in sample_pairs)
            {
                ws.SubscribeToPriceLimit(pair, (data) =>
                {
                    if (data != null)
                    {
                        // ... Your logic is here
                    }
                });
            }
#endif

#if FALSE
            /* Order Book (Public) */
            foreach (var pair in sample_pairs)
            {
                ws.SubscribeToOrderBook(pair, OkxOrderBookType.OrderBook, (data) =>
                {
                    if (data != null && data.Asks != null && data.Asks.Count() > 0 && data.Bids != null && data.Bids.Count() > 0)
                    {
                        // ... Your logic is here
                    }
                });
            }
#endif

#if FALSE
            /* Option Summary (Public) */
            ws.SubscribeToOptionSummary("USD", (data) =>
            {
                if (data != null)
                {
                    // ... Your logic is here
                }
            });
#endif

#if FALSE
            /* Funding Rates (Public) */
            foreach (var pair in sample_pairs)
            {
                ws.SubscribeToFundingRates(pair, (data) =>
                {
                    if (data != null)
                    {
                        // ... Your logic is here
                    }
                });
            }
#endif

#if FALSE
            /* Index Candlesticks (Public) */
            foreach (var pair in sample_pairs)
            {
                ws.SubscribeToIndexCandlesticks(pair, OkxPeriod.FiveMinutes, (data) =>
                {
                    if (data != null)
                    {
                        // ... Your logic is here
                    }
                });
            }
#endif

#if FALSE
            /* Index Tickers (Public) */
            foreach (var pair in sample_pairs)
            {
                ws.SubscribeToIndexTickers(pair, (data) =>
                {
                    if (data != null)
                    {
                        // ... Your logic is here
                    }
                });
            }
#endif

#if FALSE
            /* System Status (Public) */
            ws.SubscribeToSystemStatus((data) =>
            {
                if (data != null)
                {
                    // ... Your logic is here
                }
            });
#endif

#if FALSE
            /* Account Updates (Private) */
            ws.SubscribeToAccountUpdates((data) =>
            {
                if (data != null)
                {
                    // ... Your logic is here
                }
            });
#endif

#if FALSE
            /* Position Updates (Private) */
            ws.SubscribeToPositionUpdates(OkxInstrumentType.Futures, "INSTRUMENT", "UNDERLYING", (data) =>
            {
                if (data != null)
                {
                    // ... Your logic is here
                }
            });
#endif

#if FALSE
            /* Balance And Position Updates (Private) */
            ws.SubscribeToBalanceAndPositionUpdates((data) =>
            {
                if (data != null)
                {
                    // ... Your logic is here
                }
            });
#endif

#if FALSE
            /* Order Updates (Private) */
            ws.SubscribeToOrderUpdates(OkxInstrumentType.Futures, "INSTRUMENT", "UNDERLYING", (data) =>
            {
                if (data != null)
                {
                    // ... Your logic is here
                }
            });
#endif

#if FALSE
            /* Algo Order Updates (Private) */
            ws.SubscribeToAlgoOrderUpdates(OkxInstrumentType.Futures, "INSTRUMENT", "UNDERLYING", (data) =>
            {
                if (data != null)
                {
                    // ... Your logic is here
                }
            });
#endif

#if FALSE
            /* Advance Algo Order Updates (Private) */
            ws.SubscribeToAdvanceAlgoOrderUpdates(OkxInstrumentType.Futures, "INSTRUMENT", "UNDERLYING", (data) =>
            {
                if (data != null)
                {
                    // ... Your logic is here
                }
            });
#endif
            #endregion

            // Stop Here
            Console.ReadLine();
            return;
        }
    }
}