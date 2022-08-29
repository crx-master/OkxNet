﻿using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Sockets;
using Newtonsoft.Json;
using OkxNet.Converters;
using OkxNet.Enums;
using OkxNet.Objects.Core;
using OkxNet.Objects.Market;
using OkxNet.Objects.Public;
using OkxNet.Objects.System;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace OkxNet
{
    public partial class OkxSocketClient
    {
        #region Public Channels
        /// <summary>
        /// The full instrument list will be pushed for the first time after subscription. Subsequently, the instruments will be pushed if there's any change to the instrument’s state (such as delivery of FUTURES, exercise of OPTION, listing of new contracts / trading pairs, trading suspension, etc.).
        /// </summary>
        /// <param name="instrumentType">Instrument Type</param>
        /// <param name="onData">On Data Handler</param>
        /// <returns></returns>
        public virtual CallResult<UpdateSubscription> SubscribeToInstruments(OkxInstrumentType instrumentType, Action<OkxInstrument> onData)
            => SubscribeToInstrumentsAsync(instrumentType, onData).Result;
        /// <summary>
        /// The full instrument list will be pushed for the first time after subscription. Subsequently, the instruments will be pushed if there's any change to the instrument’s state (such as delivery of FUTURES, exercise of OPTION, listing of new contracts / trading pairs, trading suspension, etc.).
        /// </summary>
        /// <param name="instrumentType">Instrument Type</param>
        /// <param name="onData">On Data Handler</param>
        /// <returns></returns>
        public virtual async Task<CallResult<UpdateSubscription>> SubscribeToInstrumentsAsync(OkxInstrumentType instrumentType, Action<OkxInstrument> onData, CancellationToken ct = default)
        {
            var internalHandler = new Action<DataEvent<OkxSocketUpdateResponse<IEnumerable<OkxInstrument>>>>(data =>
            {
                foreach (var d in data.Data.Data)
                    onData(d);
            });

            var request = new OkxSocketRequest(OkxSocketOperation.Subscribe, "instruments", instrumentType);
            return await UnifiedSubscribeAsync(request, null, false, internalHandler, ct).ConfigureAwait(false);
        }

        /// <summary>
        /// Retrieve the last traded price, bid price, ask price and 24-hour trading volume of instruments. Data will be pushed every 100 ms.
        /// </summary>
        /// <param name="intrumentId">Instrument ID</param>
        /// <param name="onData">On Data Handler</param>
        /// <returns></returns>
        public virtual CallResult<UpdateSubscription> SubscribeToTickers(string intrumentId, Action<OkxTicker> onData)
            => SubscribeToTickersAsync(intrumentId, onData).Result;
        /// Retrieve the last traded price, bid price, ask price and 24-hour trading volume of instruments. Data will be pushed every 100 ms.
        /// </summary>
        /// <param name="intrumentId">Instrument ID</param>
        /// <param name="onData">On Data Handler</param>
        /// <returns></returns>
        public virtual async Task<CallResult<UpdateSubscription>> SubscribeToTickersAsync(string intrumentId, Action<OkxTicker> onData, CancellationToken ct = default)
        {
            var internalHandler = new Action<DataEvent<OkxSocketUpdateResponse<IEnumerable<OkxTicker>>>>(data =>
            {
                foreach (var d in data.Data.Data)
                    onData(d);
            });

            var request = new OkxSocketRequest(OkxSocketOperation.Subscribe, "tickers", intrumentId);
            return await UnifiedSubscribeAsync(request, null, false, internalHandler, ct).ConfigureAwait(false);
        }

        /// <summary>
        /// Retrieve the open interest. Data will by pushed every 3 seconds.
        /// </summary>
        /// <param name="intrumentId">Instrument ID</param>
        /// <param name="onData">On Data Handler</param>
        /// <returns></returns>
        public virtual CallResult<UpdateSubscription> SubscribeToInterests(string intrumentId, Action<OkxOpenInterest> onData)
            => SubscribeToInterestsAsync(intrumentId, onData).Result;
        /// <summary>
        /// Retrieve the open interest. Data will by pushed every 3 seconds.
        /// </summary>
        /// <param name="intrumentId">Instrument ID</param>
        /// <param name="onData">On Data Handler</param>
        /// <returns></returns>
        public virtual async Task<CallResult<UpdateSubscription>> SubscribeToInterestsAsync(string intrumentId, Action<OkxOpenInterest> onData, CancellationToken ct = default)
        {
            var internalHandler = new Action<DataEvent<OkxSocketUpdateResponse<IEnumerable<OkxOpenInterest>>>>(data =>
            {
                foreach (var d in data.Data.Data)
                    onData(d);
            });

            var request = new OkxSocketRequest(OkxSocketOperation.Subscribe, "open-interest", intrumentId);
            return await UnifiedSubscribeAsync(request, null, false, internalHandler, ct).ConfigureAwait(false);
        }

        /// <summary>
        /// Retrieve the candlesticks data of an instrument. Data will be pushed every 500 ms.
        /// </summary>
        /// <param name="intrumentId">Instrument ID</param>
        /// <param name="period"></param>
        /// <param name="onData">On Data Handler</param>
        /// <returns></returns>
        public virtual CallResult<UpdateSubscription> SubscribeToCandlesticks(string intrumentId, OkxPeriod period, Action<OkxCandlestick> onData)
            => SubscribeToCandlesticksAsync(intrumentId, period, onData).Result;
        /// <summary>
        /// Retrieve the candlesticks data of an instrument. Data will be pushed every 500 ms.
        /// </summary>
        /// <param name="intrumentId">Instrument ID</param>
        /// <param name="period"></param>
        /// <param name="onData">On Data Handler</param>
        /// <returns></returns>
        public virtual async Task<CallResult<UpdateSubscription>> SubscribeToCandlesticksAsync(string intrumentId, OkxPeriod period, Action<OkxCandlestick> onData, CancellationToken ct = default)
        {
            var internalHandler = new Action<DataEvent<OkxSocketUpdateResponse<IEnumerable<OkxCandlestick>>>>(data =>
            {
                foreach (var d in data.Data.Data)
                {
                    d.Instrument = intrumentId;
                    onData(d);
                }
            });

            var jc = JsonConvert.SerializeObject(period, new PeriodConverter(false));
            var request = new OkxSocketRequest(OkxSocketOperation.Subscribe, "candle" + jc, intrumentId);
            return await UnifiedSubscribeAsync(request, null, false, internalHandler, ct).ConfigureAwait(false);
        }

        /// <summary>
        /// Retrieve the recent trades data. Data will be pushed whenever there is a trade.
        /// </summary>
        /// <param name="intrumentId">Instrument ID</param>
        /// <param name="onData">On Data Handler</param>
        /// <returns></returns>
        public virtual CallResult<UpdateSubscription> SubscribeToTrades(string intrumentId, Action<OkxTrade> onData)
            => SubscribeToTradesAsync(intrumentId, onData).Result;
        /// <summary>
        /// Retrieve the recent trades data. Data will be pushed whenever there is a trade.
        /// </summary>
        /// <param name="intrumentId">Instrument ID</param>
        /// <param name="onData">On Data Handler</param>
        /// <returns></returns>
        public virtual async Task<CallResult<UpdateSubscription>> SubscribeToTradesAsync(string intrumentId, Action<OkxTrade> onData, CancellationToken ct = default)
        {
            var internalHandler = new Action<DataEvent<OkxSocketUpdateResponse<IEnumerable<OkxTrade>>>>(data =>
            {
                foreach (var d in data.Data.Data)
                    onData(d);
            });

            var request = new OkxSocketRequest(OkxSocketOperation.Subscribe, "trades", intrumentId);
            return await UnifiedSubscribeAsync(request, null, false, internalHandler, ct).ConfigureAwait(false);
        }

        /// <summary>
        /// Retrieve the estimated delivery/exercise price of FUTURES contracts and OPTION.
        /// Only the estimated delivery/exercise price will be pushed an hour before delivery/exercise, and will be pushed if there is any price change.
        /// </summary>
        /// <param name="instrumentType">Instrument Type</param>
        /// <param name="underlying">Underlying</param>
        /// <param name="onData">On Data Handler</param>
        /// <returns></returns>
        public virtual CallResult<UpdateSubscription> SubscribeToEstimatedPrice(OkxInstrumentType instrumentType, string underlying, Action<OkxEstimatedPrice> onData)
            => SubscribeToEstimatedPriceAsync(instrumentType, underlying, onData).Result;
        /// <summary>
        /// Retrieve the estimated delivery/exercise price of FUTURES contracts and OPTION.
        /// Only the estimated delivery/exercise price will be pushed an hour before delivery/exercise, and will be pushed if there is any price change.
        /// </summary>
        /// <param name="instrumentType">Instrument Type</param>
        /// <param name="underlying">Underlying</param>
        /// <param name="onData">On Data Handler</param>
        /// <returns></returns>
        public virtual async Task<CallResult<UpdateSubscription>> SubscribeToEstimatedPriceAsync(OkxInstrumentType instrumentType, string underlying, Action<OkxEstimatedPrice> onData, CancellationToken ct = default)
        {
            var internalHandler = new Action<DataEvent<OkxSocketUpdateResponse<IEnumerable<OkxEstimatedPrice>>>>(data =>
            {
                foreach (var d in data.Data.Data)
                    onData(d);
            });

            var request = new OkxSocketRequest(OkxSocketOperation.Subscribe, "estimated-price", instrumentType, underlying);
            return await UnifiedSubscribeAsync(request, null, false, internalHandler, ct).ConfigureAwait(false);
        }

        /// <summary>
        /// Retrieve the mark price. Data will be pushed every 200 ms when the mark price changes, and will be pushed every 10 seconds when the mark price does not change.
        /// </summary>
        /// <param name="intrumentId">Instrument ID</param>
        /// <param name="onData">On Data Handler</param>
        /// <returns></returns>
        public virtual CallResult<UpdateSubscription> SubscribeToMarkPrice(string intrumentId, Action<OkxMarkPrice> onData)
            => SubscribeToMarkPriceAsync(intrumentId, onData).Result;
        /// <summary>
        /// Retrieve the mark price. Data will be pushed every 200 ms when the mark price changes, and will be pushed every 10 seconds when the mark price does not change.
        /// </summary>
        /// <param name="intrumentId">Instrument ID</param>
        /// <param name="onData">On Data Handler</param>
        /// <returns></returns>
        public virtual async Task<CallResult<UpdateSubscription>> SubscribeToMarkPriceAsync(string intrumentId, Action<OkxMarkPrice> onData, CancellationToken ct = default)
        {
            var internalHandler = new Action<DataEvent<OkxSocketUpdateResponse<IEnumerable<OkxMarkPrice>>>>(data =>
            {
                foreach (var d in data.Data.Data)
                    onData(d);
            });

            var request = new OkxSocketRequest(OkxSocketOperation.Subscribe, "mark-price", intrumentId);
            return await UnifiedSubscribeAsync(request, null, false, internalHandler, ct).ConfigureAwait(false);
        }

        /// <summary>
        /// Retrieve the candlesticks data of the mark price. Data will be pushed every 500 ms.
        /// </summary>
        /// <param name="intrumentId">Instrument ID</param>
        /// <param name="period">Period</param>
        /// <param name="onData">On Data Handler</param>
        /// <returns></returns>
        public virtual CallResult<UpdateSubscription> SubscribeToMarkPriceCandlesticks(string intrumentId, OkxPeriod period, Action<OkxCandlestick> onData)
            => SubscribeToMarkPriceCandlesticksAsync(intrumentId, period, onData).Result;
        /// <summary>
        /// Retrieve the candlesticks data of the mark price. Data will be pushed every 500 ms.
        /// </summary>
        /// <param name="intrumentId">Instrument ID</param>
        /// <param name="period">Period</param>
        /// <param name="onData">On Data Handler</param>
        /// <returns></returns>
        public virtual async Task<CallResult<UpdateSubscription>> SubscribeToMarkPriceCandlesticksAsync(string intrumentId, OkxPeriod period, Action<OkxCandlestick> onData, CancellationToken ct = default)
        {
            var internalHandler = new Action<DataEvent<OkxSocketUpdateResponse<IEnumerable<OkxCandlestick>>>>(data =>
            {
                foreach (var d in data.Data.Data)
                {
                    d.Instrument = intrumentId;
                    onData(d);
                }
            });

            var jc = JsonConvert.SerializeObject(period, new PeriodConverter(false));
            var request = new OkxSocketRequest(OkxSocketOperation.Subscribe, "mark-price-candle" + jc, intrumentId);
            return await UnifiedSubscribeAsync(request, null, false, internalHandler, ct).ConfigureAwait(false);
        }

        /// <summary>
        /// Retrieve the maximum buy price and minimum sell price of the instrument. Data will be pushed every 5 seconds when there are changes in limits, and will not be pushed when there is no changes on limit.
        /// </summary>
        /// <param name="intrumentId">Instrument ID</param>
        /// <param name="onData">On Data Handler</param>
        /// <returns></returns>
        public virtual CallResult<UpdateSubscription> SubscribeToPriceLimit(string intrumentId, Action<OkxLimitPrice> onData)
            => SubscribeToPriceLimitAsync(intrumentId, onData).Result;
        /// <summary>
        /// Retrieve the maximum buy price and minimum sell price of the instrument. Data will be pushed every 5 seconds when there are changes in limits, and will not be pushed when there is no changes on limit.
        /// </summary>
        /// <param name="intrumentId">Instrument ID</param>
        /// <param name="onData">On Data Handler</param>
        /// <returns></returns>
        public virtual async Task<CallResult<UpdateSubscription>> SubscribeToPriceLimitAsync(string intrumentId, Action<OkxLimitPrice> onData, CancellationToken ct = default)
        {
            var internalHandler = new Action<DataEvent<OkxSocketUpdateResponse<IEnumerable<OkxLimitPrice>>>>(data =>
            {
                foreach (var d in data.Data.Data)
                    onData(d);
            });

            var request = new OkxSocketRequest(OkxSocketOperation.Subscribe, "price-limit", intrumentId);
            return await UnifiedSubscribeAsync(request, null, false, internalHandler, ct).ConfigureAwait(false);
        }

        /// <summary>
        /// Retrieve order book data.
        /// Use books for 400 depth levels, book5 for 5 depth levels, books50-l2-tbt tick-by-tick 50 depth levels, and books-l2-tbt for tick-by-tick 400 depth levels.
        /// books: 400 depth levels will be pushed in the initial full snapshot. Incremental data will be pushed every 100 ms when there is change in order book.
        /// books5: 5 depth levels will be pushed every time.Data will be pushed every 200 ms when there is change in order book.
        /// books50-l2-tbt: 50 depth levels will be pushed in the initial full snapshot. Incremental data will be pushed tick by tick, i.e.whenever there is change in order book.
        /// books-l2-tbt: 400 depth levels will be pushed in the initial full snapshot. Incremental data will be pushed tick by tick, i.e.whenever there is change in order book.
        /// </summary>
        /// <param name="intrumentId">Instrument ID</param>
        /// <param name="orderBookType">Order Book Type</param>
        /// <param name="onData">On Data Handler</param>
        /// <returns></returns>
        public virtual CallResult<UpdateSubscription> SubscribeToOrderBook(string intrumentId, OkxOrderBookType orderBookType, Action<OkxOrderBook> onData)
            => SubscribeToOrderBookAsync(intrumentId, orderBookType, onData).Result;
        /// <summary>
        /// Retrieve order book data.
        /// Use books for 400 depth levels, book5 for 5 depth levels, books50-l2-tbt tick-by-tick 50 depth levels, and books-l2-tbt for tick-by-tick 400 depth levels.
        /// books: 400 depth levels will be pushed in the initial full snapshot. Incremental data will be pushed every 100 ms when there is change in order book.
        /// books5: 5 depth levels will be pushed every time.Data will be pushed every 200 ms when there is change in order book.
        /// books50-l2-tbt: 50 depth levels will be pushed in the initial full snapshot. Incremental data will be pushed tick by tick, i.e.whenever there is change in order book.
        /// books-l2-tbt: 400 depth levels will be pushed in the initial full snapshot. Incremental data will be pushed tick by tick, i.e.whenever there is change in order book.
        /// </summary>
        /// <param name="intrumentId">Instrument ID</param>
        /// <param name="orderBookType">Order Book Type</param>
        /// <param name="onData">On Data Handler</param>
        /// <returns></returns>
        public virtual async Task<CallResult<UpdateSubscription>> SubscribeToOrderBookAsync(string intrumentId, OkxOrderBookType orderBookType, Action<OkxOrderBook> onData, CancellationToken ct = default)
        {
            var internalHandler = new Action<DataEvent<OkxOrderBookUpdate>>(data =>
            {
                foreach (var d in data.Data.Data)
                {
                    d.Instrument = intrumentId;
                    d.Action = data.Data.Action;
                    onData(d);
                }
            });

            var jc = JsonConvert.SerializeObject(orderBookType, new OrderBookTypeConverter(false));
            var request = new OkxSocketRequest(OkxSocketOperation.Subscribe, jc, intrumentId);
            return await UnifiedSubscribeAsync(request, null, false, internalHandler, ct).ConfigureAwait(false);
        }

        /// <summary>
        /// Retrieve detailed pricing information of all OPTION contracts. Data will be pushed at once.
        /// </summary>
        /// <param name="underlying">Underlying</param>
        /// <param name="onData">On Data Handler</param>
        /// <returns></returns>
        public virtual CallResult<UpdateSubscription> SubscribeToOptionSummary(string underlying, Action<OkxOptionSummary> onData)
            => SubscribeToOptionSummaryAsync(underlying, onData).Result;
        /// <summary>
        /// Retrieve detailed pricing information of all OPTION contracts. Data will be pushed at once.
        /// </summary>
        /// <param name="underlying">Underlying</param>
        /// <param name="onData">On Data Handler</param>
        /// <returns></returns>
        public virtual async Task<CallResult<UpdateSubscription>> SubscribeToOptionSummaryAsync(string underlying, Action<OkxOptionSummary> onData, CancellationToken ct = default)
        {
            var internalHandler = new Action<DataEvent<OkxSocketUpdateResponse<IEnumerable<OkxOptionSummary>>>>(data =>
            {
                foreach (var d in data.Data.Data)
                    onData(d);
            });

            var request = new OkxSocketRequest(OkxSocketOperation.Subscribe, "opt-summary", string.Empty, underlying);
            return await UnifiedSubscribeAsync(request, null, false, internalHandler, ct).ConfigureAwait(false);
        }

        /// <summary>
        /// Retrieve funding rate. Data will be pushed every minute.
        /// </summary>
        /// <param name="intrumentId">Instrument ID</param>
        /// <param name="onData">On Data Handler</param>
        /// <returns></returns>
        public virtual CallResult<UpdateSubscription> SubscribeToFundingRates(string intrumentId, Action<OkxFundingRate> onData)
            => SubscribeToFundingRatesAsync(intrumentId, onData).Result;
        /// <summary>
        /// Retrieve funding rate. Data will be pushed every minute.
        /// </summary>
        /// <param name="intrumentId">Instrument ID</param>
        /// <param name="onData">On Data Handler</param>
        /// <returns></returns>
        public virtual async Task<CallResult<UpdateSubscription>> SubscribeToFundingRatesAsync(string intrumentId, Action<OkxFundingRate> onData, CancellationToken ct = default)
        {
            var internalHandler = new Action<DataEvent<OkxSocketUpdateResponse<IEnumerable<OkxFundingRate>>>>(data =>
            {
                foreach (var d in data.Data.Data)
                    onData(d);
            });

            var request = new OkxSocketRequest(OkxSocketOperation.Subscribe, "funding-rate", intrumentId);
            return await UnifiedSubscribeAsync(request, null, false, internalHandler, ct).ConfigureAwait(false);
        }

        /// <summary>
        /// Retrieve the candlesticks data of the index. Data will be pushed every 500 ms.
        /// </summary>
        /// <param name="intrumentId">Instrument ID</param>
        /// <param name="period">Period</param>
        /// <param name="onData">On Data Handler</param>
        /// <returns></returns>
        public virtual CallResult<UpdateSubscription> SubscribeToIndexCandlesticks(string intrumentId, OkxPeriod period, Action<OkxCandlestick> onData)
            => SubscribeToIndexCandlesticksAsync(intrumentId, period, onData).Result;
        /// <summary>
        /// Retrieve the candlesticks data of the index. Data will be pushed every 500 ms.
        /// </summary>
        /// <param name="intrumentId">Instrument ID</param>
        /// <param name="period">Period</param>
        /// <param name="onData">On Data Handler</param>
        /// <returns></returns>
        public virtual async Task<CallResult<UpdateSubscription>> SubscribeToIndexCandlesticksAsync(string intrumentId, OkxPeriod period, Action<OkxCandlestick> onData, CancellationToken ct = default)
        {
            var internalHandler = new Action<DataEvent<OkxSocketUpdateResponse<IEnumerable<OkxCandlestick>>>>(data =>
            {
                foreach (var d in data.Data.Data)
                {
                    d.Instrument = intrumentId;
                    onData(d);
                }
            });

            var jc = JsonConvert.SerializeObject(period, new PeriodConverter(false));
            var request = new OkxSocketRequest(OkxSocketOperation.Subscribe, "index-candle" + jc, intrumentId);
            return await UnifiedSubscribeAsync(request, null, false, internalHandler, ct).ConfigureAwait(false);
        }

        /// <summary>
        /// Retrieve index tickers data
        /// </summary>
        /// <param name="intrumentId">Instrument ID</param>
        /// <param name="onData">On Data Handler</param>
        /// <returns></returns>
        public virtual CallResult<UpdateSubscription> SubscribeToIndexTickers(string intrumentId, Action<OkxIndexTicker> onData)
            => SubscribeToIndexTickersAsync(intrumentId, onData).Result;
        /// <summary>
        /// Retrieve index tickers data
        /// </summary>
        /// <param name="intrumentId">Instrument ID</param>
        /// <param name="onData">On Data Handler</param>
        /// <returns></returns>
        public virtual async Task<CallResult<UpdateSubscription>> SubscribeToIndexTickersAsync(string intrumentId, Action<OkxIndexTicker> onData, CancellationToken ct = default)
        {
            var internalHandler = new Action<DataEvent<OkxSocketUpdateResponse<IEnumerable<OkxIndexTicker>>>>(data =>
            {
                foreach (var d in data.Data.Data)
                    onData(d);
            });

            var request = new OkxSocketRequest(OkxSocketOperation.Subscribe, "index-tickers", intrumentId);
            return await UnifiedSubscribeAsync(request, null, false, internalHandler, ct).ConfigureAwait(false);
        }

        /// <summary>
        /// Get the status of system maintenance and push when the system maintenance status changes. First subscription: "Push the latest change data"; every time there is a state change, push the changed content
        /// </summary>
        /// <param name="onData">On Data Handler</param>
        /// <returns></returns>
        public virtual CallResult<UpdateSubscription> SubscribeToSystemStatus(Action<OkxStatus> onData)
            => SubscribeToSystemStatusAsync(onData).Result;
        /// <summary>
        /// Get the status of system maintenance and push when the system maintenance status changes. First subscription: "Push the latest change data"; every time there is a state change, push the changed content
        /// </summary>
        /// <param name="onData">On Data Handler</param>
        /// <returns></returns>
        public virtual async Task<CallResult<UpdateSubscription>> SubscribeToSystemStatusAsync(Action<OkxStatus> onData, CancellationToken ct = default)
        {
            var internalHandler = new Action<DataEvent<OkxSocketUpdateResponse<IEnumerable<OkxStatus>>>>(data =>
            {
                foreach (var d in data.Data.Data)
                    onData(d);
            });

            var request = new OkxSocketRequest(OkxSocketOperation.Subscribe, "status", string.Empty, string.Empty);
            return await UnifiedSubscribeAsync(request, null, false, internalHandler, ct).ConfigureAwait(false);
        }

        // TODO: Public structure block trades channel
        // TODO: Block tickers channel
        #endregion
    }
}
