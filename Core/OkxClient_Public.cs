using Newtonsoft.Json;
using SharpCryptoExchange.Objects;
using SharpCryptoExchange.Okx.Converters;
using SharpCryptoExchange.Okx.Enums;
using SharpCryptoExchange.Okx.Helpers;
using SharpCryptoExchange.Okx.Models;
using SharpCryptoExchange.Okx.Models.Core;
using SharpCryptoExchange.Okx.Models.PublicInfo;
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
        #region Public API Endpoints
        /// <summary>
        /// Retrieve a list of instruments with open contracts.
        /// </summary>
        /// <param name="instrumentType">Instrument Type</param>
        /// <param name="underlying">Underlying</param>
        /// <param name="instrumentId">Instrument ID</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<IEnumerable<OkxInstrument>> GetInstruments(OkxInstrumentType instrumentType, string underlying = null, string instrumentId = null, CancellationToken ct = default)
            => GetInstrumentsAsync(instrumentType, underlying, instrumentId, ct).Result;
        /// <summary>
        /// Retrieve a list of instruments with open contracts.
        /// </summary>
        /// <param name="instrumentType">Instrument Type</param>
        /// <param name="underlying">Underlying</param>
        /// <param name="instrumentId">Instrument ID</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<IEnumerable<OkxInstrument>>> GetInstrumentsAsync(OkxInstrumentType instrumentType, string underlying = null, string instrumentId = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>
            {
                { "instType", JsonConvert.SerializeObject(instrumentType, new InstrumentTypeConverter(false)) },
            };
            if (!string.IsNullOrEmpty(underlying)) parameters.AddOptionalParameter("uly", underlying);
            if (!string.IsNullOrEmpty(instrumentId)) parameters.AddOptionalParameter("instId", instrumentId);

            var result = await UnifiedApi.ExecuteAsync<OkxRestApiResponse<IEnumerable<OkxInstrument>>>(UnifiedApi.GetUri(Endpoints_V5_Public_Instruments), HttpMethod.Get, ct, parameters).ConfigureAwait(false);
            if (!result.Success) return result.AsError<IEnumerable<OkxInstrument>>(new OkxRestApiError(result.Error.Code, result.Error.Message, result.Error.Data));
            if (result.Data.ErrorCode > 0) return result.AsError<IEnumerable<OkxInstrument>>(new OkxRestApiError(result.Data.ErrorCode, result.Data.ErrorMessage, null));

            return result.As(result.Data.Data);
        }

        /// <summary>
        /// Retrieve the estimated delivery price, which will only have a return value one hour before the delivery/exercise.
        /// </summary>
        /// <param name="instrumentType">Instrument Type</param>
        /// <param name="underlying">Underlying</param>
        /// <param name="after">Pagination of data to return records earlier than the requested ts, Unix timestamp format in milliseconds, e.g. 1597026383085</param>
        /// <param name="before">Pagination of data to return records newer than the requested ts, Unix timestamp format in milliseconds, e.g. 1597026383085</param>
        /// <param name="limit">Number of results per request. The maximum is 100; the default is 100.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<IEnumerable<OkxDeliveryExerciseHistory>> GetDeliveryExerciseHistory(OkxInstrumentType instrumentType, string underlying, long? after = null, long? before = null, int limit = 100, CancellationToken ct = default)
            => GetDeliveryExerciseHistoryAsync(instrumentType, underlying, after, before, limit, ct).Result;
        /// <summary>
        /// Retrieve the estimated delivery price, which will only have a return value one hour before the delivery/exercise.
        /// </summary>
        /// <param name="instrumentType">Instrument Type</param>
        /// <param name="underlying">Underlying</param>
        /// <param name="after">Pagination of data to return records earlier than the requested ts, Unix timestamp format in milliseconds, e.g. 1597026383085</param>
        /// <param name="before">Pagination of data to return records newer than the requested ts, Unix timestamp format in milliseconds, e.g. 1597026383085</param>
        /// <param name="limit">Number of results per request. The maximum is 100; the default is 100.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<IEnumerable<OkxDeliveryExerciseHistory>>> GetDeliveryExerciseHistoryAsync(OkxInstrumentType instrumentType, string underlying, long? after = null, long? before = null, int limit = 100, CancellationToken ct = default)
        {
            if (instrumentType.IsNotIn(OkxInstrumentType.Futures, OkxInstrumentType.Option))
                throw new ArgumentException("Instrument Type can be only Futures or Option.");

            if (limit < 1 || limit > 100)
                throw new ArgumentException("Limit can be between 1-100.");

            var parameters = new Dictionary<string, object>
            {
                { "instType", JsonConvert.SerializeObject(instrumentType, new InstrumentTypeConverter(false)) },
                { "uly", underlying },
            };
            parameters.AddOptionalParameter("after", $"{after}");
            parameters.AddOptionalParameter("before", $"{before}");
            parameters.AddOptionalParameter("limit", $"{limit}");

            var result = await UnifiedApi.ExecuteAsync<OkxRestApiResponse<IEnumerable<OkxDeliveryExerciseHistory>>>(UnifiedApi.GetUri(Endpoints_V5_Public_DeliveryExerciseHistory), HttpMethod.Get, ct, parameters).ConfigureAwait(false);
            if (!result.Success) return result.AsError<IEnumerable<OkxDeliveryExerciseHistory>>(new OkxRestApiError(result.Error.Code, result.Error.Message, result.Error.Data));
            if (result.Data.ErrorCode > 0) return result.AsError<IEnumerable<OkxDeliveryExerciseHistory>>(new OkxRestApiError(result.Data.ErrorCode, result.Data.ErrorMessage, null));

            return result.As(result.Data.Data);
        }

        /// <summary>
        /// Retrieve the total open interest for contracts on OKEx.
        /// </summary>
        /// <param name="instrumentType">Instrument Type</param>
        /// <param name="underlying">Underlying</param>
        /// <param name="instrumentId">Instrument ID</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<IEnumerable<OkxOpenInterest>> GetOpenInterests(OkxInstrumentType instrumentType, string underlying = null, string instrumentId = null, CancellationToken ct = default)
            => GetOpenInterestsAsync(instrumentType, underlying, instrumentId, ct).Result;
        /// <summary>
        /// Retrieve the total open interest for contracts on OKEx.
        /// </summary>
        /// <param name="instrumentType">Instrument Type</param>
        /// <param name="underlying">Underlying</param>
        /// <param name="instrumentId">Instrument ID</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<IEnumerable<OkxOpenInterest>>> GetOpenInterestsAsync(OkxInstrumentType instrumentType, string underlying = null, string instrumentId = null, CancellationToken ct = default)
        {
            if (instrumentType.IsNotIn(OkxInstrumentType.Futures, OkxInstrumentType.Option, OkxInstrumentType.Swap))
                throw new ArgumentException("Instrument Type can be only Futures, Option or Swap.");

            if (instrumentType == OkxInstrumentType.Swap && string.IsNullOrEmpty(underlying))
                throw new ArgumentException("Underlying is required for Option.");

            var parameters = new Dictionary<string, object>
            {
                { "instType", JsonConvert.SerializeObject(instrumentType, new InstrumentTypeConverter(false)) },
            };
            if (!string.IsNullOrEmpty(underlying)) parameters.AddOptionalParameter("uly", underlying);
            if (!string.IsNullOrEmpty(instrumentId)) parameters.AddOptionalParameter("instId", instrumentId);

            var result = await UnifiedApi.ExecuteAsync<OkxRestApiResponse<IEnumerable<OkxOpenInterest>>>(UnifiedApi.GetUri(Endpoints_V5_Public_OpenInterest), HttpMethod.Get, ct, parameters).ConfigureAwait(false);
            if (!result.Success) return result.AsError<IEnumerable<OkxOpenInterest>>(new OkxRestApiError(result.Error.Code, result.Error.Message, result.Error.Data));
            if (result.Data.ErrorCode > 0) return result.AsError<IEnumerable<OkxOpenInterest>>(new OkxRestApiError(result.Data.ErrorCode, result.Data.ErrorMessage, null));

            return result.As(result.Data.Data);
        }

        /// <summary>
        /// Retrieve funding rate.
        /// </summary>
        /// <param name="instrumentId">Instrument ID</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<IEnumerable<OkxFundingRate>> GetFundingRates(string instrumentId, CancellationToken ct = default)
            => GetFundingRatesAsync(instrumentId, ct).Result;
        /// <summary>
        /// Retrieve funding rate.
        /// </summary>
        /// <param name="instrumentId">Instrument ID</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<IEnumerable<OkxFundingRate>>> GetFundingRatesAsync(string instrumentId, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>
            {
                { "instId", instrumentId },
            };

            var result = await UnifiedApi.ExecuteAsync<OkxRestApiResponse<IEnumerable<OkxFundingRate>>>(UnifiedApi.GetUri(Endpoints_V5_Public_FundingRate), HttpMethod.Get, ct, parameters).ConfigureAwait(false);
            if (!result.Success) return result.AsError<IEnumerable<OkxFundingRate>>(new OkxRestApiError(result.Error.Code, result.Error.Message, result.Error.Data));
            if (result.Data.ErrorCode > 0) return result.AsError<IEnumerable<OkxFundingRate>>(new OkxRestApiError(result.Data.ErrorCode, result.Data.ErrorMessage, null));

            return result.As(result.Data.Data);
        }

        /// <summary>
        /// Retrieve funding rate history. This endpoint can retrieve data from the last 3 months.
        /// </summary>
        /// <param name="instrumentId">Instrument ID</param>
        /// <param name="after">Pagination of data to return records earlier than the requested ts, Unix timestamp format in milliseconds, e.g. 1597026383085</param>
        /// <param name="before">Pagination of data to return records newer than the requested ts, Unix timestamp format in milliseconds, e.g. 1597026383085</param>
        /// <param name="limit">Number of results per request. The maximum is 100; the default is 100.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<IEnumerable<OkxFundingRateHistory>> GetFundingRateHistory(string instrumentId, long? after = null, long? before = null, int limit = 100, CancellationToken ct = default)
            => GetFundingRateHistoryAsync(instrumentId, after, before, limit, ct).Result;
        /// <summary>
        /// Retrieve funding rate history. This endpoint can retrieve data from the last 3 months.
        /// </summary>
        /// <param name="instrumentId">Instrument ID</param>
        /// <param name="after">Pagination of data to return records earlier than the requested ts, Unix timestamp format in milliseconds, e.g. 1597026383085</param>
        /// <param name="before">Pagination of data to return records newer than the requested ts, Unix timestamp format in milliseconds, e.g. 1597026383085</param>
        /// <param name="limit">Number of results per request. The maximum is 100; the default is 100.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<IEnumerable<OkxFundingRateHistory>>> GetFundingRateHistoryAsync(string instrumentId,
                                                                                                                long? after = null,
                                                                                                                long? before = null,
                                                                                                                int limit = 100,
                                                                                                                CancellationToken ct = default)
        {
            if (limit < 1 || limit > 100) throw new ArgumentException("Limit can be between 1-100.");

            var parameters = new Dictionary<string, object>
            {
                { "instId", instrumentId },
            };
            parameters.AddOptionalParameter("after", $"{after}");
            parameters.AddOptionalParameter("before", $"{before}");
            parameters.AddOptionalParameter("limit", $"{limit}");

            var result = await UnifiedApi.ExecuteAsync<OkxRestApiResponse<IEnumerable<OkxFundingRateHistory>>>(UnifiedApi.GetUri(Endpoints_V5_Public_FundingRateHistory), HttpMethod.Get, ct, parameters).ConfigureAwait(false);
            if (!result.Success) return result.AsError<IEnumerable<OkxFundingRateHistory>>(new OkxRestApiError(result.Error.Code, result.Error.Message, result.Error.Data));
            if (result.Data.ErrorCode > 0) return result.AsError<IEnumerable<OkxFundingRateHistory>>(new OkxRestApiError(result.Data.ErrorCode, result.Data.ErrorMessage, null));

            return result.As(result.Data.Data);
        }

        /// <summary>
        /// Retrieve the highest buy limit and lowest sell limit of the instrument.
        /// </summary>
        /// <param name="instrumentId">Instrument ID</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<OkxLimitPrice> GetLimitPrice(string instrumentId, CancellationToken ct = default)
            => GetLimitPriceAsync(instrumentId, ct).Result;
        /// <summary>
        /// Retrieve the highest buy limit and lowest sell limit of the instrument.
        /// </summary>
        /// <param name="instrumentId">Instrument ID</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<OkxLimitPrice>> GetLimitPriceAsync(string instrumentId, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>
            {
                { "instId", instrumentId },
            };

            var result = await UnifiedApi.ExecuteAsync<OkxRestApiResponse<IEnumerable<OkxLimitPrice>>>(UnifiedApi.GetUri(Endpoints_V5_Public_PriceLimit), HttpMethod.Get, ct, parameters).ConfigureAwait(false);
            if (!result.Success) return result.AsError<OkxLimitPrice>(new OkxRestApiError(result.Error.Code, result.Error.Message, result.Error.Data));
            if (result.Data.ErrorCode > 0) return result.AsError<OkxLimitPrice>(new OkxRestApiError(result.Data.ErrorCode, result.Data.ErrorMessage, null));

            return result.As(result.Data.Data.FirstOrDefault());
        }

        /// <summary>
        /// Retrieve option market data.
        /// </summary>
        /// <param name="underlying">Underlying</param>
        /// <param name="expiryDate">Contract expiry date, the format is "YYMMDD", e.g. "200527"</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<IEnumerable<OkxOptionSummary>> GetOptionMarketData(string underlying, DateTime? expiryDate = null, CancellationToken ct = default)
            => GetOptionMarketDataAsync(underlying, expiryDate, ct).Result;
        /// <summary>
        /// Retrieve option market data.
        /// </summary>
        /// <param name="underlying">Underlying</param>
        /// <param name="expiryDate">Contract expiry date, the format is "YYMMDD", e.g. "200527"</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<IEnumerable<OkxOptionSummary>>> GetOptionMarketDataAsync(string underlying, DateTime? expiryDate = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>
            {
                { "uly", underlying },
            };
            parameters.AddOptionalParameter("expTime", expiryDate?.ToString("yyMMdd", OkxGlobals.OkxCultureInfo));

            var result = await UnifiedApi.ExecuteAsync<OkxRestApiResponse<IEnumerable<OkxOptionSummary>>>(UnifiedApi.GetUri(Endpoints_V5_Public_OptionSummary), HttpMethod.Get, ct, parameters).ConfigureAwait(false);
            if (!result.Success) return result.AsError<IEnumerable<OkxOptionSummary>>(new OkxRestApiError(result.Error.Code, result.Error.Message, result.Error.Data));
            if (result.Data.ErrorCode > 0) return result.AsError<IEnumerable<OkxOptionSummary>>(new OkxRestApiError(result.Data.ErrorCode, result.Data.ErrorMessage, null));

            return result.As(result.Data.Data);
        }

        /// <summary>
        /// Retrieve the estimated delivery price which will only have a return value one hour before the delivery/exercise.
        /// </summary>
        /// <param name="instrumentId">Instrument ID</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<OkxEstimatedPrice> GetEstimatedPrice(string instrumentId, CancellationToken ct = default)
            => GetEstimatedPriceAsync(instrumentId, ct).Result;
        /// <summary>
        /// Retrieve the estimated delivery price which will only have a return value one hour before the delivery/exercise.
        /// </summary>
        /// <param name="instrumentId">Instrument ID</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<OkxEstimatedPrice>> GetEstimatedPriceAsync(string instrumentId, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>
            {
                { "instId", instrumentId },
            };

            var result = await UnifiedApi.ExecuteAsync<OkxRestApiResponse<IEnumerable<OkxEstimatedPrice>>>(UnifiedApi.GetUri(Endpoints_V5_Public_EstimatedPrice), HttpMethod.Get, ct, parameters).ConfigureAwait(false);
            if (!result.Success) return result.AsError<OkxEstimatedPrice>(new OkxRestApiError(result.Error.Code, result.Error.Message, result.Error.Data));
            if (result.Data.ErrorCode > 0) return result.AsError<OkxEstimatedPrice>(new OkxRestApiError(result.Data.ErrorCode, result.Data.ErrorMessage, null));

            return result.As(result.Data.Data.FirstOrDefault());
        }

        /// <summary>
        /// Retrieve discount rate level and interest-free quota.
        /// </summary>
        /// <param name="discountLevel">Discount level</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<IEnumerable<OkxDiscountInfo>> GetDiscountInfo(int? discountLevel = null, CancellationToken ct = default)
            => GetDiscountInfoAsync(discountLevel, ct).Result;
        /// <summary>
        /// Retrieve discount rate level and interest-free quota.
        /// </summary>
        /// <param name="discountLevel">Discount level</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<IEnumerable<OkxDiscountInfo>>> GetDiscountInfoAsync(int? discountLevel = null, CancellationToken ct = default)
        {

            if (discountLevel.HasValue && (discountLevel < 1 || discountLevel > 5))
                throw new ArgumentException("Limit can be between 1-5.");

            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("discountLv", $"{discountLevel}");

            var result = await UnifiedApi.ExecuteAsync<OkxRestApiResponse<IEnumerable<OkxDiscountInfo>>>(UnifiedApi.GetUri(Endpoints_V5_Public_DiscountRateInterestFreeQuota), HttpMethod.Get, ct, parameters).ConfigureAwait(false);
            if (!result.Success) return result.AsError<IEnumerable<OkxDiscountInfo>>(new OkxRestApiError(result.Error.Code, result.Error.Message, result.Error.Data));
            if (result.Data.ErrorCode > 0) return result.AsError<IEnumerable<OkxDiscountInfo>>(new OkxRestApiError(result.Data.ErrorCode, result.Data.ErrorMessage, null));

            return result.As(result.Data.Data);
        }

        /// <summary>
        /// Retrieve API server time.
        /// </summary>
        /// <remarks>
        /// Rate Limit: 10 requests per 2 seconds
        /// Rate limit rule: IP
        /// </remarks>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<DateTimeOffset> GetSystemTime(CancellationToken ct = default)
            => GetSystemTimeAsync(ct).Result;
        /// <summary>
        /// Asynchronously retrieve API server time.
        /// </summary>
        /// <remarks>
        /// Rate Limit: 10 requests per 2 seconds
        /// Rate limit rule: IP
        /// </remarks>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<DateTimeOffset>> GetSystemTimeAsync(CancellationToken ct = default)
        {
            var result = await UnifiedApi.ExecuteAsync<OkxRestApiResponse<IEnumerable<OkxTime>>>(UnifiedApi.GetUri(Endpoints_V5_Public_Time), HttpMethod.Get, ct).ConfigureAwait(false);
            if (!result.Success) return result.AsError<DateTimeOffset>(new OkxRestApiError(result.Error.Code, result.Error.Message, result.Error.Data));
            if (result.Data.ErrorCode > 0) return result.AsError<DateTimeOffset>(new OkxRestApiError(result.Data.ErrorCode, result.Data.ErrorMessage, null));

            return result.As(result.Data.Data.FirstOrDefault().Time);
        }

        /// <summary>
        /// Retrieve information on liquidation orders in the last 1 days.
        /// </summary>
        /// <param name="instrumentType">Instrument Type</param>
        /// <param name="marginMode">Margin Mode</param>
        /// <param name="instrumentId">Instrument ID</param>
        /// <param name="currency">Currency</param>
        /// <param name="underlying">Underlying</param>
        /// <param name="instrumentAlias">Alias</param>
        /// <param name="state">State</param>
        /// <param name="after">Pagination of data to return records earlier than the requested ts, Unix timestamp format in milliseconds, e.g. 1597026383085</param>
        /// <param name="before">Pagination of data to return records newer than the requested ts, Unix timestamp format in milliseconds, e.g. 1597026383085</param>
        /// <param name="limit">Number of results per request. The maximum is 100; the default is 100.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<IEnumerable<OkxLiquidationInfo>> GetLiquidationOrders(
            OkxInstrumentType instrumentType,
            OkxMarginMode? marginMode = null,
            string instrumentId = null,
            string currency = null,
            string underlying = null,
            OkxInstrumentAlias? instrumentAlias = null,
            OkxLiquidationState? state = null,
            long? after = null, long? before = null, int limit = 100,
            CancellationToken ct = default)
            => GetLiquidationOrdersAsync(
            instrumentType,
            marginMode,
            instrumentId,
            currency,
            underlying,
            instrumentAlias,
            state,
            after, before, limit,
            ct).Result;
        /// <summary>
        /// Retrieve information on liquidation orders in the last 1 days.
        /// </summary>
        /// <param name="instrumentType">Instrument Type</param>
        /// <param name="marginMode">Margin Mode</param>
        /// <param name="instrumentId">Instrument ID</param>
        /// <param name="currency">Currency</param>
        /// <param name="underlying">Underlying</param>
        /// <param name="intrumentAlias">Instrument alias</param>
        /// <param name="state">State</param>
        /// <param name="after">Pagination of data to return records earlier than the requested ts, Unix timestamp format in milliseconds, e.g. 1597026383085</param>
        /// <param name="before">Pagination of data to return records newer than the requested ts, Unix timestamp format in milliseconds, e.g. 1597026383085</param>
        /// <param name="limit">Number of results per request. The maximum is 100; the default is 100.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<IEnumerable<OkxLiquidationInfo>>> GetLiquidationOrdersAsync(
            OkxInstrumentType instrumentType,
            OkxMarginMode? marginMode = null,
            string instrumentId = null,
            string currency = null,
            string underlying = null,
            OkxInstrumentAlias? intrumentAlias = null,
            OkxLiquidationState? state = null,
            long? after = null, long? before = null, int limit = 100,
            CancellationToken ct = default)
        {
            if (instrumentType.IsIn(OkxInstrumentType.Futures, OkxInstrumentType.Swap, OkxInstrumentType.Option) && string.IsNullOrEmpty(underlying))
                throw new ArgumentException("Underlying is required.");

            if (instrumentType.IsIn(OkxInstrumentType.Futures, OkxInstrumentType.Swap) && state == null)
                throw new ArgumentException("State is required.");

            if (instrumentType.IsIn(OkxInstrumentType.Futures) && intrumentAlias == null)
                throw new ArgumentException("Alias is required.");

            if (limit < 1 || limit > 100)
                throw new ArgumentException("Limit can be between 1-100.");

            var parameters = new Dictionary<string, object>
            {
                { "instType", JsonConvert.SerializeObject(instrumentType, new InstrumentTypeConverter(false)) },
            };
            if (marginMode != null)
                parameters.AddOptionalParameter("mgnMode", JsonConvert.SerializeObject(marginMode, new MarginModeConverter(false)));
            if (!string.IsNullOrEmpty(instrumentId))
                parameters.AddOptionalParameter("instId", instrumentId);
            if (!string.IsNullOrEmpty(currency))
                parameters.AddOptionalParameter("ccy", currency);
            if (!string.IsNullOrEmpty(underlying))
                parameters.AddOptionalParameter("uly", underlying);
            if (intrumentAlias != null)
                parameters.AddOptionalParameter("alias", JsonConvert.SerializeObject(intrumentAlias, new InstrumentAliasConverter(false)));
            if (state != null)
                parameters.AddOptionalParameter("state", JsonConvert.SerializeObject(state, new LiquidationStateConverter(false)));

            parameters.AddOptionalParameter("after", $"{after}");
            parameters.AddOptionalParameter("before", $"{before}");
            parameters.AddOptionalParameter("limit", $"{limit}");

            var result = await UnifiedApi.ExecuteAsync<OkxRestApiResponse<IEnumerable<OkxLiquidationInfo>>>(UnifiedApi.GetUri(Endpoints_V5_Public_LiquidationOrders), HttpMethod.Get, ct, parameters).ConfigureAwait(false);
            if (!result.Success) return result.AsError<IEnumerable<OkxLiquidationInfo>>(new OkxRestApiError(result.Error.Code, result.Error.Message, result.Error.Data));
            if (result.Data.ErrorCode > 0) return result.AsError<IEnumerable<OkxLiquidationInfo>>(new OkxRestApiError(result.Data.ErrorCode, result.Data.ErrorMessage, null));

            return result.As(result.Data.Data);
        }

        /// <summary>
        /// Retrieve mark price.
        /// We set the mark price based on the SPOT index and at a reasonable basis to prevent individual users from manipulating the market and causing the contract price to fluctuate.
        /// </summary>
        /// <param name="instrumentType">Instrument Type</param>
        /// <param name="underlying">Underlying</param>
        /// <param name="instrumentId">Instrument ID</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<IEnumerable<OkxMarkPrice>> GetMarkPrices(OkxInstrumentType instrumentType, string underlying = null, string instrumentId = null, CancellationToken ct = default)
            => GetMarkPricesAsync(instrumentType, underlying, instrumentId, ct).Result;
        /// <summary>
        /// Retrieve mark price.
        /// We set the mark price based on the SPOT index and at a reasonable basis to prevent individual users from manipulating the market and causing the contract price to fluctuate.
        /// </summary>
        /// <param name="instrumentType">Instrument Type</param>
        /// <param name="underlying">Underlying</param>
        /// <param name="instrumentId">Instrument ID</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<IEnumerable<OkxMarkPrice>>> GetMarkPricesAsync(OkxInstrumentType instrumentType, string underlying = null, string instrumentId = null, CancellationToken ct = default)
        {
            if (instrumentType.IsNotIn(OkxInstrumentType.Margin, OkxInstrumentType.Futures, OkxInstrumentType.Option, OkxInstrumentType.Swap))
                throw new ArgumentException("Instrument Type can be only Margin, Futures, Option or Swap.");

            var parameters = new Dictionary<string, object>
            {
                { "instType", JsonConvert.SerializeObject(instrumentType, new InstrumentTypeConverter(false)) },
            };
            if (!string.IsNullOrEmpty(underlying))
                parameters.AddOptionalParameter("uly", underlying);
            if (!string.IsNullOrEmpty(instrumentId))
                parameters.AddOptionalParameter("instId", instrumentId);

            var result = await UnifiedApi.ExecuteAsync<OkxRestApiResponse<IEnumerable<OkxMarkPrice>>>(UnifiedApi.GetUri(Endpoints_V5_Public_MarkPrice), HttpMethod.Get, ct, parameters).ConfigureAwait(false);
            if (!result.Success) return result.AsError<IEnumerable<OkxMarkPrice>>(new OkxRestApiError(result.Error.Code, result.Error.Message, result.Error.Data));
            if (result.Data.ErrorCode > 0) return result.AsError<IEnumerable<OkxMarkPrice>>(new OkxRestApiError(result.Data.ErrorCode, result.Data.ErrorMessage, null));

            return result.As(result.Data.Data);
        }

        /// <summary>
        /// Position information，Maximum leverage depends on your borrowings and margin ratio.
        /// </summary>
        /// <param name="instrumentType">Instrument Type</param>
        /// <param name="marginMode">Margin Mode</param>
        /// <param name="underlying">Underlying</param>
        /// <param name="instrumentId">Instrument ID</param>
        /// <param name="tier"></param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<IEnumerable<OkxPositionTier>> GetPositionTiers(
            OkxInstrumentType instrumentType,
            OkxMarginMode marginMode,
            string underlying,
            string instrumentId = null,
            string tier = null,
            CancellationToken ct = default)
            => GetPositionTiersAsync(instrumentType, marginMode, underlying, instrumentId, tier, ct).Result;
        /// <summary>
        /// Position information，Maximum leverage depends on your borrowings and margin ratio.
        /// </summary>
        /// <param name="instrumentType">Instrument Type</param>
        /// <param name="marginMode">Margin Mode</param>
        /// <param name="underlying">Underlying</param>
        /// <param name="instrumentId">Instrument ID</param>
        /// <param name="tier"></param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<IEnumerable<OkxPositionTier>>> GetPositionTiersAsync(
            OkxInstrumentType instrumentType,
            OkxMarginMode marginMode,
            string underlying,
            string instrumentId = null,
            string tier = null,
            CancellationToken ct = default)
        {
            if (instrumentType.IsNotIn(OkxInstrumentType.Margin, OkxInstrumentType.Futures, OkxInstrumentType.Option, OkxInstrumentType.Swap))
                throw new ArgumentException("Instrument Type can be only Margin, Futures, Option or Swap.");

            var parameters = new Dictionary<string, object>
            {
                { "instType", JsonConvert.SerializeObject(instrumentType, new InstrumentTypeConverter(false)) },
                { "tdMode", JsonConvert.SerializeObject(marginMode, new MarginModeConverter(false)) },
            };
            if (!string.IsNullOrEmpty(underlying))
                parameters.AddOptionalParameter("uly", underlying);
            if (!string.IsNullOrEmpty(instrumentId))
                parameters.AddOptionalParameter("instId", instrumentId);
            if (!string.IsNullOrEmpty(tier))
                parameters.AddOptionalParameter("tier", tier);

            var result = await UnifiedApi.ExecuteAsync<OkxRestApiResponse<IEnumerable<OkxPositionTier>>>(UnifiedApi.GetUri(Endpoints_V5_Public_PositionTiers), HttpMethod.Get, ct, parameters).ConfigureAwait(false);
            if (!result.Success) return result.AsError<IEnumerable<OkxPositionTier>>(new OkxRestApiError(result.Error.Code, result.Error.Message, result.Error.Data));
            if (result.Data.ErrorCode > 0) return result.AsError<IEnumerable<OkxPositionTier>>(new OkxRestApiError(result.Data.ErrorCode, result.Data.ErrorMessage, null));

            return result.As(result.Data.Data);
        }

        /// <summary>
        /// Get margin interest rate
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<OkxInterestRate> GetInterestRates(CancellationToken ct = default)
            => GetInterestRatesAsync(ct).Result;
        /// <summary>
        /// Get margin interest rate
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<OkxInterestRate>> GetInterestRatesAsync(CancellationToken ct = default)
        {
            var result = await UnifiedApi.ExecuteAsync<OkxRestApiResponse<IEnumerable<OkxInterestRate>>>(UnifiedApi.GetUri(Endpoints_V5_Public_InterestRateLoanQuota), HttpMethod.Get, ct).ConfigureAwait(false);
            if (!result.Success) return result.AsError<OkxInterestRate>(new OkxRestApiError(result.Error.Code, result.Error.Message, result.Error.Data));
            if (result.Data.ErrorCode > 0) return result.AsError<OkxInterestRate>(new OkxRestApiError(result.Data.ErrorCode, result.Data.ErrorMessage, null));

            return result.As(result.Data.Data.FirstOrDefault());
        }

        /// <summary>
        /// Get interest rate and loan quota for VIP loans
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<IEnumerable<OkxVipInterestRate>> GetVIPInterestRates(CancellationToken ct = default)
            => GetVIPInterestRatesAsync(ct).Result;
        /// <summary>
        /// Get interest rate and loan quota for VIP loans
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<IEnumerable<OkxVipInterestRate>>> GetVIPInterestRatesAsync(CancellationToken ct = default)
        {
            var result = await UnifiedApi.ExecuteAsync<OkxRestApiResponse<IEnumerable<OkxVipInterestRate>>>(UnifiedApi.GetUri(Endpoints_V5_Public_VIPInterestRateLoanQuota), HttpMethod.Get, ct).ConfigureAwait(false);
            if (!result.Success) return result.AsError<IEnumerable<OkxVipInterestRate>>(new OkxRestApiError(result.Error.Code, result.Error.Message, result.Error.Data));
            if (result.Data.ErrorCode > 0) return result.AsError<IEnumerable<OkxVipInterestRate>>(new OkxRestApiError(result.Data.ErrorCode, result.Data.ErrorMessage, null));

            return result.As(result.Data.Data);
        }

        /// <summary>
        /// Get Underlying
        /// </summary>
        /// <param name="instrumentType">Instrument Type</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<IEnumerable<string>> GetUnderlying(OkxInstrumentType instrumentType, CancellationToken ct = default)
            => GetUnderlyingAsync(instrumentType, ct).Result;
        /// <summary>
        /// Get Underlying
        /// </summary>
        /// <param name="instrumentType">Instrument Type</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<IEnumerable<string>>> GetUnderlyingAsync(OkxInstrumentType instrumentType, CancellationToken ct = default)
        {
            if (instrumentType.IsNotIn(OkxInstrumentType.Futures, OkxInstrumentType.Option, OkxInstrumentType.Swap))
                throw new ArgumentException("Instrument Type can be only Futures, Option or Swap.");

            var parameters = new Dictionary<string, object>
            {
                { "instType", JsonConvert.SerializeObject(instrumentType, new InstrumentTypeConverter(false)) },
            };
            var result = await UnifiedApi.ExecuteAsync<OkxRestApiResponse<IEnumerable<IEnumerable<string>>>>(UnifiedApi.GetUri(Endpoints_V5_Public_Underlying), HttpMethod.Get, ct, parameters).ConfigureAwait(false);
            if (!result.Success) return result.AsError<IEnumerable<string>>(new OkxRestApiError(result.Error.Code, result.Error.Message, result.Error.Data));
            if (result.Data.ErrorCode > 0) return result.AsError<IEnumerable<string>>(new OkxRestApiError(result.Data.ErrorCode, result.Data.ErrorMessage, null));

            return result.As(result.Data.Data.FirstOrDefault());
        }

        /// <summary>
        /// Get insurance fund
        /// Get insurance fund balance information
        /// Rate Limit: 10 requests per 2 seconds
        /// </summary>
        /// <param name="instrumentType"></param>
        /// <param name="type"></param>
        /// <param name="underlying"></param>
        /// <param name="currency"></param>
        /// <param name="after"></param>
        /// <param name="before"></param>
        /// <param name="limit"></param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<OkxInsuranceFund> GetInsuranceFund(
            OkxInstrumentType instrumentType,
            OkxInsuranceType type = OkxInsuranceType.All,
            string underlying = "",
            string currency = "",
            long? after = null,
            long? before = null,
            int limit = 100,
            CancellationToken ct = default)
            => GetInsuranceFundAsync(instrumentType, type, underlying, currency, after, before, limit, ct).Result;

        /// <summary>
        /// Get insurance fund
        /// Get insurance fund balance information
        /// Rate Limit: 10 requests per 2 seconds
        /// </summary>
        /// <param name="instrumentType"></param>
        /// <param name="type"></param>
        /// <param name="underlying"></param>
        /// <param name="currency"></param>
        /// <param name="after"></param>
        /// <param name="before"></param>
        /// <param name="limit"></param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<OkxInsuranceFund>> GetInsuranceFundAsync(
            OkxInstrumentType instrumentType,
            OkxInsuranceType type = OkxInsuranceType.All,
            string underlying = "",
            string currency = "",
            long? after = null,
            long? before = null,
            int limit = 100,
            CancellationToken ct = default)
        {
            if (instrumentType.IsNotIn(OkxInstrumentType.Margin, OkxInstrumentType.Swap, OkxInstrumentType.Futures, OkxInstrumentType.Option))
                throw new ArgumentException("Instrument Type can be only Margin, Swap, Futures or Option.");

            var parameters = new Dictionary<string, object>
            {
                { "instType", JsonConvert.SerializeObject(instrumentType, new InstrumentTypeConverter(false)) },
            };

            if (type != OkxInsuranceType.All)
                parameters.AddOptionalParameter("type", JsonConvert.SerializeObject(type, new InsuranceTypeConverter(false)));

            if (!string.IsNullOrEmpty(underlying))
                parameters.AddOptionalParameter("uly", underlying);
            if (!string.IsNullOrEmpty(currency))
                parameters.AddOptionalParameter("ccy", currency);

            parameters.AddOptionalParameter("after", $"{after}");
            parameters.AddOptionalParameter("before", $"{before}");
            parameters.AddOptionalParameter("limit", $"{limit}");

            var result = await UnifiedApi.ExecuteAsync<OkxRestApiResponse<IEnumerable<OkxInsuranceFund>>>(UnifiedApi.GetUri(Endpoints_V5_Public_InsuranceFund), HttpMethod.Get, ct, parameters).ConfigureAwait(false);
            if (!result.Success) return result.AsError<OkxInsuranceFund>(new OkxRestApiError(result.Error.Code, result.Error.Message, result.Error.Data));
            if (result.Data.ErrorCode > 0) return result.AsError<OkxInsuranceFund>(new OkxRestApiError(result.Data.ErrorCode, result.Data.ErrorMessage, null));

            return result.As(result.Data.Data.FirstOrDefault());
        }

        /// <summary>
        /// Unit Convert
        /// Convert currency to contract, or contract to currency.
        /// Rate Limit: 10 requests per 2 seconds
        /// </summary>
        /// <param name="instrumentType">Instrument Type</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<OkxUnitConvert> UnitConvert(
            OkxConvertType? type = null,
            OkxConvertUnit? unit = null,
            string instrumentId = "",
            decimal? price = null,
            decimal? size = null,
            CancellationToken ct = default)
            => UnitConvertAsync(type, unit, instrumentId, price, size, ct).Result;
        /// <summary>
        /// Get Underlying
        /// </summary>
        /// <param name="instrumentType">Instrument Type</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<OkxUnitConvert>> UnitConvertAsync(
            OkxConvertType? type = OkxConvertType.CurrencyToContract,
            OkxConvertUnit? unit = null,
            string instrumentId = "",
            decimal? price = null,
            decimal? size = null,
            CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>();
            if (type != null) parameters.AddOptionalParameter("type", JsonConvert.SerializeObject(type, new ConvertTypeConverter(false)));
            if (unit != null) parameters.AddOptionalParameter("unit", JsonConvert.SerializeObject(type, new ConvertUnitConverter(false)));
            if (!string.IsNullOrEmpty(instrumentId)) parameters.AddOptionalParameter("instId", instrumentId);
            parameters.AddOptionalParameter("px", price?.ToString(OkxGlobals.OkxCultureInfo));
            parameters.AddOptionalParameter("sz", size?.ToString(OkxGlobals.OkxCultureInfo));

            var result = await UnifiedApi.ExecuteAsync<OkxRestApiResponse<IEnumerable<OkxUnitConvert>>>(UnifiedApi.GetUri(Endpoints_V5_Public_ConvertContractCoin), HttpMethod.Get, ct, parameters).ConfigureAwait(false);
            if (!result.Success) return result.AsError<OkxUnitConvert>(new OkxRestApiError(result.Error.Code, result.Error.Message, result.Error.Data));
            if (result.Data.ErrorCode > 0) return result.AsError<OkxUnitConvert>(new OkxRestApiError(result.Data.ErrorCode, result.Data.ErrorMessage, null));

            return result.As(result.Data.Data.FirstOrDefault());
        }
        #endregion
    }
}