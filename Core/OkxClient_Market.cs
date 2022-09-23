using Newtonsoft.Json;
using SharpCryptoExchange.Objects;
using SharpCryptoExchange.Okx.Converters;
using SharpCryptoExchange.Okx.Enums;
using SharpCryptoExchange.Okx.Models.Core;
using SharpCryptoExchange.Okx.Models.Market;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SharpCryptoExchange.Okx
{
    public partial class OkxClient
    {
        #region Market API Endpoints
        /// <summary>
        /// Retrieve the latest price snapshot, best bid/ask price, and trading volume in the last 24 hours.
        /// </summary>
        /// <param name="instrumentType">Instrument Type</param>
        /// <param name="underlying">Underlying</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<IEnumerable<OkxTicker>> GetTickers(OkxInstrumentType instrumentType, string underlying = null, CancellationToken ct = default)
            => GetTickersAsync(instrumentType, underlying, ct).Result;
        /// <summary>
        /// Retrieve the latest price snapshot, best bid/ask price, and trading volume in the last 24 hours.
        /// </summary>
        /// <param name="instrumentType">Instrument Type</param>
        /// <param name="underlying">Underlying</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<IEnumerable<OkxTicker>>> GetTickersAsync(OkxInstrumentType instrumentType, string underlying = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>
            {
                { "instType", JsonConvert.SerializeObject(instrumentType, new InstrumentTypeConverter(false)) },
            };
            parameters.AddOptionalParameter("uly", underlying);

            var result = await UnifiedApi.ExecuteAsync<OkxRestApiResponse<IEnumerable<OkxTicker>>>(UnifiedApi.GetUri(Endpoints_V5_Market_Tickers), HttpMethod.Get, ct, parameters).ConfigureAwait(false);
            if (!result.Success) return result.AsError<IEnumerable<OkxTicker>>(new OkxRestApiError(result.Error.Code, result.Error.Message, result.Error.Data));
            if (result.Data.ErrorCode > 0) return result.AsError<IEnumerable<OkxTicker>>(new OkxRestApiError(result.Data.ErrorCode, result.Data.ErrorMessage, null));

            return result.As(result.Data.Data);
        }

        /// <summary>
        /// Retrieve the latest price snapshot, best bid/ask price, and trading volume in the last 24 hours.
        /// </summary>
        /// <param name="instrumentId">Instrument ID</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<OkxTicker> GetTicker(string instrumentId, CancellationToken ct = default)
            => GetTickerAsync(instrumentId, ct).Result;
        /// <summary>
        /// Retrieve the latest price snapshot, best bid/ask price, and trading volume in the last 24 hours.
        /// </summary>
        /// <param name="instrumentId">Instrument ID</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<OkxTicker>> GetTickerAsync(string instrumentId, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>
            {
                { "instId", instrumentId },
            };

            var result = await UnifiedApi.ExecuteAsync<OkxRestApiResponse<IEnumerable<OkxTicker>>>(UnifiedApi.GetUri(Endpoints_V5_Market_Ticker), HttpMethod.Get, ct, parameters).ConfigureAwait(false);
            if (!result.Success) return result.AsError<OkxTicker>(new OkxRestApiError(result.Error.Code, result.Error.Message, result.Error.Data));
            if (result.Data.ErrorCode > 0) return result.AsError<OkxTicker>(new OkxRestApiError(result.Data.ErrorCode, result.Data.ErrorMessage, null));

            return result.As(result.Data.Data.FirstOrDefault());
        }

        /// <summary>
        /// Retrieve index tickers.
        /// </summary>
        /// <param name="quoteCurrency">Quote currency. Currently there is only an index with USD/USDT/BTC as the quote currency.</param>
        /// <param name="instrumentId">Instrument ID</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<IEnumerable<OkxIndexTicker>> GetIndexTickers(string quoteCurrency = null, string instrumentId = null, CancellationToken ct = default)
            => GetIndexTickersAsync(quoteCurrency, instrumentId, ct).Result;
        /// <summary>
        /// Retrieve index tickers.
        /// </summary>
        /// <param name="quoteCurrency">Quote currency. Currently there is only an index with USD/USDT/BTC as the quote currency.</param>
        /// <param name="instrumentId">Instrument ID</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<IEnumerable<OkxIndexTicker>>> GetIndexTickersAsync(string quoteCurrency = null, string instrumentId = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("quoteCcy", quoteCurrency);
            parameters.AddOptionalParameter("instId", instrumentId);

            var result = await UnifiedApi.ExecuteAsync<OkxRestApiResponse<IEnumerable<OkxIndexTicker>>>(UnifiedApi.GetUri(Endpoints_V5_Market_IndexTickers), HttpMethod.Get, ct, parameters).ConfigureAwait(false);
            if (!result.Success) return result.AsError<IEnumerable<OkxIndexTicker>>(new OkxRestApiError(result.Error.Code, result.Error.Message, result.Error.Data));
            if (result.Data.ErrorCode > 0) return result.AsError<IEnumerable<OkxIndexTicker>>(new OkxRestApiError(result.Data.ErrorCode, result.Data.ErrorMessage, null));

            return result.As(result.Data.Data);
        }

        /// <summary>
        /// Retrieve a instrument is order book.
        /// </summary>
        /// <param name="instrumentId">Instrument ID</param>
        /// <param name="depth">Order book depth per side. Maximum 400, e.g. 400 bids + 400 asks. Default returns to 1 depth data</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<OkxOrderBook> GetOrderBook(string instrumentId, int depth = 1, CancellationToken ct = default)
            => GetOrderBookAsync(instrumentId, depth, ct).Result;
        /// <summary>
        /// Retrieve a instrument is order book.
        /// </summary>
        /// <param name="instrumentId">Instrument ID</param>
        /// <param name="depth">Order book depth per side. Maximum 400, e.g. 400 bids + 400 asks. Default returns to 1 depth data</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<OkxOrderBook>> GetOrderBookAsync(string instrumentId, int depth = 1, CancellationToken ct = default)
        {
            if (depth < 1 || depth > 400)
                throw new ArgumentException("Depth can be between 1-400.");

            var parameters = new Dictionary<string, object>
            {
                {"instId", instrumentId},
                {"sz", depth},
            };

            var result = await UnifiedApi.ExecuteAsync<OkxRestApiResponse<IEnumerable<OkxOrderBook>>>(UnifiedApi.GetUri(Endpoints_V5_Market_Books), HttpMethod.Get, ct, parameters).ConfigureAwait(false);
            if (!result.Success || result.Data.Data.Any()) return result.AsError<OkxOrderBook>(new OkxRestApiError(result.Error.Code, result.Error.Message, result.Error.Data));
            if (result.Data.ErrorCode > 0) return result.AsError<OkxOrderBook>(new OkxRestApiError(result.Data.ErrorCode, result.Data.ErrorMessage, null));

            var orderbook = result.Data.Data.FirstOrDefault();
            orderbook.Instrument = instrumentId;
            return result.As(orderbook);
        }

        /// <summary>
        /// Retrieve the candlestick charts. This endpoint can retrieve the latest 1,440 data entries. Charts are returned in groups based on the requested bar.
        /// </summary>
        /// <param name="instrumentId">Instrument ID</param>
        /// <param name="period">Bar size, the default is 1m</param>
        /// <param name="after">Pagination of data to return records earlier than the requested ts, Unix timestamp format in milliseconds, e.g. 1597026383085</param>
        /// <param name="before">Pagination of data to return records newer than the requested ts, Unix timestamp format in milliseconds, e.g. 1597026383085</param>
        /// <param name="limit">Number of results per request. The maximum is 300; the default is 100.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<IEnumerable<OkxCandlestick>> GetCandlesticks(string instrumentId, OkxPeriod period, long? after = null, long? before = null, int limit = 100, CancellationToken ct = default)
            => GetCandlesticksAsync(instrumentId, period, after, before, limit, ct).Result;
        /// <summary>
        /// Retrieve the candlestick charts. This endpoint can retrieve the latest 1,440 data entries. Charts are returned in groups based on the requested bar.
        /// </summary>
        /// <param name="instrumentId">Instrument ID</param>
        /// <param name="period">Bar size, the default is 1m</param>
        /// <param name="after">Pagination of data to return records earlier than the requested ts, Unix timestamp format in milliseconds, e.g. 1597026383085</param>
        /// <param name="before">Pagination of data to return records newer than the requested ts, Unix timestamp format in milliseconds, e.g. 1597026383085</param>
        /// <param name="limit">Number of results per request. The maximum is 300; the default is 100.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<IEnumerable<OkxCandlestick>>> GetCandlesticksAsync(string instrumentId, OkxPeriod period, long? after = null, long? before = null, int limit = 100, CancellationToken ct = default)
        {
            if (limit < 1 || limit > 300)
                throw new ArgumentException("Limit can be between 1-300.");

            var parameters = new Dictionary<string, object>
            {
                { "instId", instrumentId },
                { "bar", JsonConvert.SerializeObject(period, new PeriodConverter(false)) },
            };
            parameters.AddOptionalParameter("after", $"{after}");
            parameters.AddOptionalParameter("before", $"{before}");
            parameters.AddOptionalParameter("limit", $"{limit}");

            var result = await UnifiedApi.ExecuteAsync<OkxRestApiResponse<IEnumerable<OkxCandlestick>>>(UnifiedApi.GetUri(Endpoints_V5_Market_Candles), HttpMethod.Get, ct, parameters).ConfigureAwait(false);
            if (!result.Success) return result.AsError<IEnumerable<OkxCandlestick>>(new OkxRestApiError(result.Error.Code, result.Error.Message, result.Error.Data));
            if (result.Data.ErrorCode > 0) return result.AsError<IEnumerable<OkxCandlestick>>(new OkxRestApiError(result.Data.ErrorCode, result.Data.ErrorMessage, null));

            foreach (var candle in result.Data.Data) candle.Instrument = instrumentId;
            return result.As(result.Data.Data);
        }

        /// <summary>
        /// Retrieve history candlestick charts from recent years.
        /// </summary>
        /// <param name="instrumentId">Instrument ID</param>
        /// <param name="period">Bar size, the default is 1m</param>
        /// <param name="after">Pagination of data to return records earlier than the requested ts, Unix timestamp format in milliseconds, e.g. 1597026383085</param>
        /// <param name="before">Pagination of data to return records newer than the requested ts, Unix timestamp format in milliseconds, e.g. 1597026383085</param>
        /// <param name="limit">Number of results per request. The maximum is 100; the default is 100.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<IEnumerable<OkxCandlestick>> GetCandlesticksHistory(string instrumentId, OkxPeriod period, long? after = null, long? before = null, int limit = 100, CancellationToken ct = default)
            => GetCandlesticksHistoryAsync(instrumentId, period, after, before, limit, ct).Result;
        /// <summary>
        /// Retrieve history candlestick charts from recent years.
        /// </summary>
        /// <param name="instrumentId">Instrument ID</param>
        /// <param name="period">Bar size, the default is 1m</param>
        /// <param name="after">Pagination of data to return records earlier than the requested ts, Unix timestamp format in milliseconds, e.g. 1597026383085</param>
        /// <param name="before">Pagination of data to return records newer than the requested ts, Unix timestamp format in milliseconds, e.g. 1597026383085</param>
        /// <param name="limit">Number of results per request. The maximum is 100; the default is 100.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<IEnumerable<OkxCandlestick>>> GetCandlesticksHistoryAsync(string instrumentId, OkxPeriod period, long? after = null, long? before = null, int limit = 100, CancellationToken ct = default)
        {
            if (limit < 1 || limit > 100)
                throw new ArgumentException("Limit can be between 1-100.");

            var parameters = new Dictionary<string, object>
            {
                { "instId", instrumentId },
                { "bar", JsonConvert.SerializeObject(period, new PeriodConverter(false)) },
            };
            parameters.AddOptionalParameter("after", $"{after}");
            parameters.AddOptionalParameter("before", $"{before}");
            parameters.AddOptionalParameter("limit", $"{limit}");

            var result = await UnifiedApi.ExecuteAsync<OkxRestApiResponse<IEnumerable<OkxCandlestick>>>(UnifiedApi.GetUri(Endpoints_V5_Market_HistoryCandles), HttpMethod.Get, ct, parameters).ConfigureAwait(false);
            if (!result.Success) return result.AsError<IEnumerable<OkxCandlestick>>(new OkxRestApiError(result.Error.Code, result.Error.Message, result.Error.Data));
            if (result.Data.ErrorCode > 0) return result.AsError<IEnumerable<OkxCandlestick>>(new OkxRestApiError(result.Data.ErrorCode, result.Data.ErrorMessage, null));

            foreach (var candle in result.Data.Data) candle.Instrument = instrumentId;
            return result.As(result.Data.Data);
        }

        /// <summary>
        /// Retrieve the candlestick charts of the index. This endpoint can retrieve the latest 1,440 data entries. Charts are returned in groups based on the requested bar.
        /// </summary>
        /// <param name="instrumentId">Instrument ID</param>
        /// <param name="period">Bar size, the default is 1m</param>
        /// <param name="after">Pagination of data to return records earlier than the requested ts, Unix timestamp format in milliseconds, e.g. 1597026383085</param>
        /// <param name="before">Pagination of data to return records newer than the requested ts, Unix timestamp format in milliseconds, e.g. 1597026383085</param>
        /// <param name="limit">Number of results per request. The maximum is 100; the default is 100.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<IEnumerable<OkxCandlestick>> GetIndexCandlesticks(string instrumentId, OkxPeriod period, long? after = null, long? before = null, int limit = 100, CancellationToken ct = default)
            => GetIndexCandlesticksAsync(instrumentId, period, after, before, limit, ct).Result;
        /// <summary>
        /// Retrieve the candlestick charts of the index. This endpoint can retrieve the latest 1,440 data entries. Charts are returned in groups based on the requested bar.
        /// </summary>
        /// <param name="instrumentId">Instrument ID</param>
        /// <param name="period">Bar size, the default is 1m</param>
        /// <param name="after">Pagination of data to return records earlier than the requested ts, Unix timestamp format in milliseconds, e.g. 1597026383085</param>
        /// <param name="before">Pagination of data to return records newer than the requested ts, Unix timestamp format in milliseconds, e.g. 1597026383085</param>
        /// <param name="limit">Number of results per request. The maximum is 100; the default is 100.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<IEnumerable<OkxCandlestick>>> GetIndexCandlesticksAsync(string instrumentId, OkxPeriod period, long? after = null, long? before = null, int limit = 100, CancellationToken ct = default)
        {
            if (limit < 1 || limit > 100)
                throw new ArgumentException("Limit can be between 1-100.");

            var parameters = new Dictionary<string, object>
            {
                { "instId", instrumentId },
                { "bar", JsonConvert.SerializeObject(period, new PeriodConverter(false)) },
            };
            parameters.AddOptionalParameter("after", $"{after}");
            parameters.AddOptionalParameter("before", $"{before}");
            parameters.AddOptionalParameter("limit", $"{limit}");

            var result = await UnifiedApi.ExecuteAsync<OkxRestApiResponse<IEnumerable<OkxCandlestick>>>(UnifiedApi.GetUri(Endpoints_V5_Market_IndexCandles), HttpMethod.Get, ct, parameters).ConfigureAwait(false);
            if (!result.Success) return result.AsError<IEnumerable<OkxCandlestick>>(new OkxRestApiError(result.Error.Code, result.Error.Message, result.Error.Data));
            if (result.Data.ErrorCode > 0) return result.AsError<IEnumerable<OkxCandlestick>>(new OkxRestApiError(result.Data.ErrorCode, result.Data.ErrorMessage, null));

            foreach (var candle in result.Data.Data) candle.Instrument = instrumentId;
            return result.As(result.Data.Data);
        }

        /// <summary>
        /// Retrieve the candlestick charts of mark price. This endpoint can retrieve the latest 1,440 data entries. Charts are returned in groups based on the requested bar.
        /// </summary>
        /// <param name="instrumentId">Instrument ID</param>
        /// <param name="period">Bar size, the default is 1m</param>
        /// <param name="after">Pagination of data to return records earlier than the requested ts, Unix timestamp format in milliseconds, e.g. 1597026383085</param>
        /// <param name="before">Pagination of data to return records newer than the requested ts, Unix timestamp format in milliseconds, e.g. 1597026383085</param>
        /// <param name="limit">Number of results per request. The maximum is 100; the default is 100.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<IEnumerable<OkxCandlestick>> GetMarkPriceCandlesticks(string instrumentId, OkxPeriod period, long? after = null, long? before = null, int limit = 100, CancellationToken ct = default)
            => GetMarkPriceCandlesticksAsync(instrumentId, period, after, before, limit, ct).Result;
        /// <summary>
        /// Retrieve the candlestick charts of mark price. This endpoint can retrieve the latest 1,440 data entries. Charts are returned in groups based on the requested bar.
        /// </summary>
        /// <param name="instrumentId">Instrument ID</param>
        /// <param name="period">Bar size, the default is 1m</param>
        /// <param name="after">Pagination of data to return records earlier than the requested ts, Unix timestamp format in milliseconds, e.g. 1597026383085</param>
        /// <param name="before">Pagination of data to return records newer than the requested ts, Unix timestamp format in milliseconds, e.g. 1597026383085</param>
        /// <param name="limit">Number of results per request. The maximum is 100; the default is 100.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<IEnumerable<OkxCandlestick>>> GetMarkPriceCandlesticksAsync(string instrumentId, OkxPeriod period, long? after = null, long? before = null, int limit = 100, CancellationToken ct = default)
        {
            if (limit < 1 || limit > 100)
                throw new ArgumentException("Limit can be between 1-100.");

            var parameters = new Dictionary<string, object>
            {
                { "instId", instrumentId },
                { "bar", JsonConvert.SerializeObject(period, new PeriodConverter(false)) },
            };
            parameters.AddOptionalParameter("after", $"{after}");
            parameters.AddOptionalParameter("before", $"{before}");
            parameters.AddOptionalParameter("limit", $"{limit}");

            var result = await UnifiedApi.ExecuteAsync<OkxRestApiResponse<IEnumerable<OkxCandlestick>>>(UnifiedApi.GetUri(Endpoints_V5_Market_MarkPriceCandles), HttpMethod.Get, ct, parameters).ConfigureAwait(false);
            if (!result.Success) return result.AsError<IEnumerable<OkxCandlestick>>(new OkxRestApiError(result.Error.Code, result.Error.Message, result.Error.Data));
            if (result.Data.ErrorCode > 0) return result.AsError<IEnumerable<OkxCandlestick>>(new OkxRestApiError(result.Data.ErrorCode, result.Data.ErrorMessage, null));

            foreach (var candle in result.Data.Data) candle.Instrument = instrumentId;
            return result.As(result.Data.Data);
        }

        /// <summary>
        /// Retrieve the recent transactions of an instrument.
        /// </summary>
        /// <param name="instrumentId">Instrument ID</param>
        /// <param name="limit">Number of results per request. The maximum is 100; the default is 100.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<IEnumerable<OkxTrade>> GetTrades(string instrumentId, int limit = 100, CancellationToken ct = default)
            => GetTradesAsync(instrumentId, limit, ct).Result;
        /// <summary>
        /// Retrieve the recent transactions of an instrument.
        /// </summary>
        /// <param name="instrumentId">Instrument ID</param>
        /// <param name="limit">Number of results per request. The maximum is 100; the default is 100.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<IEnumerable<OkxTrade>>> GetTradesAsync(string instrumentId, int limit = 100, CancellationToken ct = default)
        {
            if (limit < 1 || limit > 500)
                throw new ArgumentException("Limit can be between 1-500.");

            var parameters = new Dictionary<string, object>
            {
                { "instId", instrumentId },
            };
            parameters.AddOptionalParameter("limit", $"{limit}");

            var result = await UnifiedApi.ExecuteAsync<OkxRestApiResponse<IEnumerable<OkxTrade>>>(UnifiedApi.GetUri(Endpoints_V5_Market_Trades), HttpMethod.Get, ct, parameters).ConfigureAwait(false);
            if (!result.Success) return result.AsError<IEnumerable<OkxTrade>>(new OkxRestApiError(result.Error.Code, result.Error.Message, result.Error.Data));
            if (result.Data.ErrorCode > 0) return result.AsError<IEnumerable<OkxTrade>>(new OkxRestApiError(result.Data.ErrorCode, result.Data.ErrorMessage, null));

            return result.As(result.Data.Data);
        }

        /// <summary>
        /// Get trades history
        /// Retrieve the recent transactions of an instrument from the last 3 months with pagination.
        /// Rate Limit: 10 requests per 2 seconds
        /// </summary>
        /// <param name="instrumentId">Instrument ID, e.g. BTC-USDT</param>
        /// <param name="type">Pagination Type</param>
        /// <param name="after">Pagination of data to return records earlier than the requested ts, Unix timestamp format in milliseconds, e.g. 1597026383085</param>
        /// <param name="before">Pagination of data to return records newer than the requested ts, Unix timestamp format in milliseconds, e.g. 1597026383085</param>
        /// <param name="limit">Number of results per request. The maximum is 100; the default is 100.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<IEnumerable<OkxTrade>> GetTradesHistory(string instrumentId, OkxTradeHistoryPaginationType type = OkxTradeHistoryPaginationType.TradeId, long? after = null, long? before = null, int limit = 100, CancellationToken ct = default)
            => GetTradesHistoryAsync(instrumentId, type, after, before, limit, ct).Result;
        /// <summary>
        /// Get trades history
        /// Retrieve the recent transactions of an instrument from the last 3 months with pagination.
        /// Rate Limit: 10 requests per 2 seconds
        /// </summary>
        /// <param name="instrumentId">Instrument ID, e.g. BTC-USDT</param>
        /// <param name="type">Pagination Type</param>
        /// <param name="after">Pagination of data to return records earlier than the requested ts, Unix timestamp format in milliseconds, e.g. 1597026383085</param>
        /// <param name="before">Pagination of data to return records newer than the requested ts, Unix timestamp format in milliseconds, e.g. 1597026383085</param>
        /// <param name="limit">Number of results per request. The maximum is 100; the default is 100.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<IEnumerable<OkxTrade>>> GetTradesHistoryAsync(string instrumentId, OkxTradeHistoryPaginationType type = OkxTradeHistoryPaginationType.TradeId, long? after = null, long? before = null, int limit = 100, CancellationToken ct = default)
        {
            if (limit < 1 || limit > 100)
                throw new ArgumentException("Limit can be between 1-100.");

            var parameters = new Dictionary<string, object>
            {
                { "instId", instrumentId },
            };

            parameters.AddOptionalParameter("type", JsonConvert.SerializeObject(type, new TradeHistoryPaginationTypeConverter(false)));
            parameters.AddOptionalParameter("after", $"{after}");
            parameters.AddOptionalParameter("before", $"{before}");
            parameters.AddOptionalParameter("limit", $"{limit}");

            var result = await UnifiedApi.ExecuteAsync<OkxRestApiResponse<IEnumerable<OkxTrade>>>(UnifiedApi.GetUri(Endpoints_V5_Market_TradesHistory), HttpMethod.Get, ct, parameters).ConfigureAwait(false);
            if (!result.Success) return result.AsError<IEnumerable<OkxTrade>>(new OkxRestApiError(result.Error.Code, result.Error.Message, result.Error.Data));
            if (result.Data.ErrorCode > 0) return result.AsError<IEnumerable<OkxTrade>>(new OkxRestApiError(result.Data.ErrorCode, result.Data.ErrorMessage, null));

            return result.As(result.Data.Data);
        }

        /// <summary>
        /// The 24-hour trading volume is calculated on a rolling basis, using USD as the pricing unit.
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<Okx24HourVolume> Get24HourVolume(CancellationToken ct = default)
            => Get24HourVolumeAsync(ct).Result;
        /// <summary>
        /// The 24-hour trading volume is calculated on a rolling basis, using USD as the pricing unit.
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<Okx24HourVolume>> Get24HourVolumeAsync(CancellationToken ct = default)
        {
            var result = await UnifiedApi.ExecuteAsync<OkxRestApiResponse<IEnumerable<Okx24HourVolume>>>(UnifiedApi.GetUri(Endpoints_V5_Market_Platform24Volume), HttpMethod.Get, ct).ConfigureAwait(false);
            if (!result.Success) return result.AsError<Okx24HourVolume>(new OkxRestApiError(result.Error.Code, result.Error.Message, result.Error.Data));
            if (result.Data.ErrorCode > 0) return result.AsError<Okx24HourVolume>(new OkxRestApiError(result.Data.ErrorCode, result.Data.ErrorMessage, null));

            return result.As(result.Data.Data.FirstOrDefault());
        }

        /// <summary>
        /// Get the crypto price of signing using Open Oracle smart contract.
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<OkxOracle> GetOracle(CancellationToken ct = default)
            => GetOracleAsync(ct).Result;
        /// <summary>
        /// Get the crypto price of signing using Open Oracle smart contract.
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<OkxOracle>> GetOracleAsync(CancellationToken ct = default)
        {
            var result = await UnifiedApi.ExecuteAsync<OkxRestApiResponse<IEnumerable<OkxOracle>>>(UnifiedApi.GetUri(Endpoints_V5_Market_OpenOracle), HttpMethod.Get, ct).ConfigureAwait(false);
            if (!result.Success) return result.AsError<OkxOracle>(new OkxRestApiError(result.Error.Code, result.Error.Message, result.Error.Data));
            if (result.Data.ErrorCode > 0) return result.AsError<OkxOracle>(new OkxRestApiError(result.Data.ErrorCode, result.Data.ErrorMessage, null));

            return result.As(result.Data.Data.FirstOrDefault());
        }

        /// <summary>
        /// Get the index component information data on the market
        /// </summary>
        /// <param name="index"></param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<OkxIndexComponents> GetIndexComponents(string index, CancellationToken ct = default)
            => GetIndexComponentsAsync(index, ct).Result;
        /// <summary>
        /// Get the index component information data on the market
        /// </summary>
        /// <param name="index"></param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<OkxIndexComponents>> GetIndexComponentsAsync(string index, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>
            {
                { "index", index },
            };

            var result = await UnifiedApi.ExecuteAsync<OkxRestApiResponse<OkxIndexComponents>>(UnifiedApi.GetUri(Endpoints_V5_Market_IndexComponents), HttpMethod.Get, ct, parameters).ConfigureAwait(false);
            if (!result.Success) return result.AsError<OkxIndexComponents>(new OkxRestApiError(result.Error.Code, result.Error.Message, result.Error.Data));
            if (result.Data.ErrorCode > 0) return result.AsError<OkxIndexComponents>(new OkxRestApiError(result.Data.ErrorCode, result.Data.ErrorMessage, null));

            return result.As(result.Data.Data);
        }

        /// <summary>
        /// Get brick tickers
        /// Retrieve the latest brick trading volume in the last 24 hours.
        /// Rate Limit: 20 requests per 2 seconds
        /// </summary>
        /// <param name="instrumentType">Instrument Type</param>
        /// <param name="underlying">Underlying</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<IEnumerable<OkxBrickTicker>> GetBrickTickers(OkxInstrumentType instrumentType, string underlying = null, CancellationToken ct = default)
            => GetBrickTickersAsync(instrumentType, underlying, ct).Result;
        /// <summary>
        /// Get brick tickers
        /// Retrieve the latest brick trading volume in the last 24 hours.
        /// Rate Limit: 20 requests per 2 seconds
        /// </summary>
        /// <param name="instrumentType">Instrument Type</param>
        /// <param name="underlying">Underlying</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<IEnumerable<OkxBrickTicker>>> GetBrickTickersAsync(OkxInstrumentType instrumentType, string underlying = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>
            {
                { "instType", JsonConvert.SerializeObject(instrumentType, new InstrumentTypeConverter(false)) },
            };
            parameters.AddOptionalParameter("uly", underlying);

            var result = await UnifiedApi.ExecuteAsync<OkxRestApiResponse<IEnumerable<OkxBrickTicker>>>(UnifiedApi.GetUri(Endpoints_V5_Market_BrickTickers), HttpMethod.Get, ct, parameters).ConfigureAwait(false);
            if (!result.Success) return result.AsError<IEnumerable<OkxBrickTicker>>(new OkxRestApiError(result.Error.Code, result.Error.Message, result.Error.Data));
            if (result.Data.ErrorCode > 0) return result.AsError<IEnumerable<OkxBrickTicker>>(new OkxRestApiError(result.Data.ErrorCode, result.Data.ErrorMessage, null));

            return result.As(result.Data.Data);
        }

        /// <summary>
        /// Get brick ticker
        /// Retrieve the latest brick trading volume in the last 24 hours.
        /// Rate Limit: 20 requests per 2 seconds
        /// </summary>
        /// <param name="instrumentId">Instrument ID</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<OkxBrickTicker> GetBrickTicker(string instrumentId, CancellationToken ct = default)
            => GetBrickTickerAsync(instrumentId, ct).Result;
        /// <summary>
        /// Get brick ticker
        /// Retrieve the latest brick trading volume in the last 24 hours.
        /// Rate Limit: 20 requests per 2 seconds
        /// </summary>
        /// <param name="instrumentId">Instrument ID</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<OkxBrickTicker>> GetBrickTickerAsync(string instrumentId, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>
            {
                { "instId", instrumentId },
            };

            var result = await UnifiedApi.ExecuteAsync<OkxRestApiResponse<IEnumerable<OkxBrickTicker>>>(UnifiedApi.GetUri(Endpoints_V5_Market_BrickTicker), HttpMethod.Get, ct, parameters).ConfigureAwait(false);
            if (!result.Success) return result.AsError<OkxBrickTicker>(new OkxRestApiError(result.Error.Code, result.Error.Message, result.Error.Data));
            if (result.Data.ErrorCode > 0) return result.AsError<OkxBrickTicker>(new OkxRestApiError(result.Data.ErrorCode, result.Data.ErrorMessage, null));

            return result.As(result.Data.Data.FirstOrDefault());
        }

        /// <summary>
        /// Get brick trades
        /// Retrieve the recent brick trading transactions of an instrument. Descending order by tradeId.
        /// Rate Limit: 20 requests per 2 seconds
        /// </summary>
        /// <param name="instrumentId">Instrument ID</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<IEnumerable<OkxTrade>> GetBrickTrades(string instrumentId, CancellationToken ct = default)
            => GetBrickTradesAsync(instrumentId, ct).Result;
        /// <summary>
        /// Get brick trades
        /// Retrieve the recent brick trading transactions of an instrument. Descending order by tradeId.
        /// Rate Limit: 20 requests per 2 seconds
        /// </summary>
        /// <param name="instrumentId">Instrument ID</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<IEnumerable<OkxTrade>>> GetBrickTradesAsync(string instrumentId, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>
            {
                { "instId", instrumentId },
            };

            var result = await UnifiedApi.ExecuteAsync<OkxRestApiResponse<IEnumerable<OkxTrade>>>(UnifiedApi.GetUri(Endpoints_V5_Market_BrickTrades), HttpMethod.Get, ct, parameters).ConfigureAwait(false);
            if (!result.Success) return result.AsError<IEnumerable<OkxTrade>>(new OkxRestApiError(result.Error.Code, result.Error.Message, result.Error.Data));
            if (result.Data.ErrorCode > 0) return result.AsError<IEnumerable<OkxTrade>>(new OkxRestApiError(result.Data.ErrorCode, result.Data.ErrorMessage, null));

            return result.As(result.Data.Data);
        }
        #endregion
    }
}