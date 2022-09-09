using CryptoExchangeNet;
using CryptoExchangeNet.Objects;
using Newtonsoft.Json;
using OkxNet.Converters;
using OkxNet.Enums;
using OkxNet.Helpers;
using OkxNet.Objects.Core;
using OkxNet.Objects.Trading;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace OkxNet
{
    public partial class OkxClient
    {
        #region Trading API Endpoints
        /// <summary>
        /// Get the currency supported by the transaction big data interface
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<OkxSupportCoins> GetRubikSupportCoin(CancellationToken ct = default) 
            => GetRubikSupportCoinAsync(ct).Result;
        /// <summary>
        /// Get the currency supported by the transaction big data interface
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<OkxSupportCoins>> GetRubikSupportCoinAsync(CancellationToken ct = default)
        {
            var result = await UnifiedApi.ExecuteAsync<OkxRestApiResponse<OkxSupportCoins>>(UnifiedApi.GetUri(Endpoints_V5_RubikStat_TradingDataSupportCoin), HttpMethod.Get, ct).ConfigureAwait(false);
            if (!result.Success) return result.AsError<OkxSupportCoins>(new OkxRestApiError(result.Error.Code, result.Error.Message, result.Error.Data));
            if (result.Data.ErrorCode > 0) return result.AsError<OkxSupportCoins>(new OkxRestApiError(result.Data.ErrorCode, result.Data.ErrorMessage, null));

            return result.As(result.Data.Data);
        }

        /// <summary>
        /// This is the taker volume for both buyers and sellers. This shows the influx and exit of funds in and out of {coin}.
        /// </summary>
        /// <param name="currency">Currency</param>
        /// <param name="instrumentType">Instrument Type</param>
        /// <param name="period">period, the default is 5m, e.g. [5m/1H/1D]</param>
        /// <param name="begin">begin, e.g. 1597026383085</param>
        /// <param name="end">end, e.g. 1597026383011</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<IEnumerable<OkxTakerVolume>> GetRubikTakerVolume(
            string currency,
            OkxInstrumentType instrumentType,
            OkxPeriod period = OkxPeriod.FiveMinutes,
            long? begin = null,
            long? end = null,
            CancellationToken ct = default) 
            => GetRubikTakerVolumeAsync(currency, instrumentType, period, begin, end, ct).Result;
        /// <summary>
        /// This is the taker volume for both buyers and sellers. This shows the influx and exit of funds in and out of {coin}.
        /// </summary>
        /// <param name="currency">Currency</param>
        /// <param name="instrumentType">Instrument Type</param>
        /// <param name="period">period, the default is 5m, e.g. [5m/1H/1D]</param>
        /// <param name="begin">begin, e.g. 1597026383085</param>
        /// <param name="end">end, e.g. 1597026383011</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<IEnumerable<OkxTakerVolume>>> GetRubikTakerVolumeAsync(
            string currency,
            OkxInstrumentType instrumentType,
            OkxPeriod period = OkxPeriod.FiveMinutes,
            long? begin = null,
            long? end = null,
            CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object> {
                { "ccy", currency},
                { "instType", JsonConvert.SerializeObject(instrumentType, new InstrumentTypeConverter(false)) },
                { "period", JsonConvert.SerializeObject(period, new PeriodConverter(false)) },
            };
            parameters.AddOptionalParameter("begin", begin?.ToString(OkxGlobals.OkxCultureInfo));
            parameters.AddOptionalParameter("end", end?.ToString(OkxGlobals.OkxCultureInfo));

            var result = await UnifiedApi.ExecuteAsync<OkxRestApiResponse<IEnumerable<OkxTakerVolume>>>(UnifiedApi.GetUri(Endpoints_V5_RubikStat_TakerVolume), HttpMethod.Get, ct, parameters).ConfigureAwait(false);
            if (!result.Success) return result.AsError<IEnumerable<OkxTakerVolume>>(new OkxRestApiError(result.Error.Code, result.Error.Message, result.Error.Data));
            if (result.Data.ErrorCode > 0) return result.AsError<IEnumerable<OkxTakerVolume>>(new OkxRestApiError(result.Data.ErrorCode, result.Data.ErrorMessage, null));

            return result.As(result.Data.Data);
        }

        /// <summary>
        /// This indicator shows the ratio of cumulative data value between currency pair leverage quote currency and underlying asset over a given period of time.
        /// </summary>
        /// <param name="currency">Currency</param>
        /// <param name="period">period, the default is 5m, e.g. [5m/1H/1D]</param>
        /// <param name="begin">begin, e.g. 1597026383085</param>
        /// <param name="end">end, e.g. 1597026383085</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<IEnumerable<OkxRatio>> GetRubikMarginLendingRatio(
            string currency,
            OkxPeriod period = OkxPeriod.FiveMinutes,
            long? begin = null,
            long? end = null,
            CancellationToken ct = default) 
            => GetRubikMarginLendingRatioAsync(currency, period, begin, end, ct).Result;
        /// <summary>
        /// This indicator shows the ratio of cumulative data value between currency pair leverage quote currency and underlying asset over a given period of time.
        /// </summary>
        /// <param name="currency">Currency</param>
        /// <param name="period">period, the default is 5m, e.g. [5m/1H/1D]</param>
        /// <param name="begin">begin, e.g. 1597026383085</param>
        /// <param name="end">end, e.g. 1597026383085</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<IEnumerable<OkxRatio>>> GetRubikMarginLendingRatioAsync(
            string currency,
            OkxPeriod period = OkxPeriod.FiveMinutes,
            long? begin = null,
            long? end = null,
            CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object> {
                { "ccy", currency},
                { "period", JsonConvert.SerializeObject(period, new PeriodConverter(false)) },
            };
            parameters.AddOptionalParameter("begin", begin?.ToString(OkxGlobals.OkxCultureInfo));
            parameters.AddOptionalParameter("end", end?.ToString(OkxGlobals.OkxCultureInfo));

            var result = await UnifiedApi.ExecuteAsync<OkxRestApiResponse<IEnumerable<OkxRatio>>>(UnifiedApi.GetUri(Endpoints_V5_RubikStat_MarginLoanRatio), HttpMethod.Get, ct, parameters).ConfigureAwait(false);
            if (!result.Success) return result.AsError<IEnumerable<OkxRatio>>(new OkxRestApiError(result.Error.Code, result.Error.Message, result.Error.Data));
            if (result.Data.ErrorCode > 0) return result.AsError<IEnumerable<OkxRatio>>(new OkxRestApiError(result.Data.ErrorCode, result.Data.ErrorMessage, null));

            return result.As(result.Data.Data);
        }

        /// <summary>
        /// This is the ratio of users with net long vs short positions. It includes data from futures and perpetual swaps.
        /// </summary>
        /// <param name="currency">Currency</param>
        /// <param name="period">period, the default is 5m, e.g. [5m/1H/1D]</param>
        /// <param name="begin">begin, e.g. 1597026383085</param>
        /// <param name="end">end, e.g. 1597026383011</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<IEnumerable<OkxRatio>> GetRubikLongShortRatio(
            string currency,
            OkxPeriod period = OkxPeriod.FiveMinutes,
            long? begin = null,
            long? end = null,
            CancellationToken ct = default) 
            => GetRubikLongShortRatioAsync(currency, period, begin, end, ct).Result;
        /// <summary>
        /// This is the ratio of users with net long vs short positions. It includes data from futures and perpetual swaps.
        /// </summary>
        /// <param name="currency">Currency</param>
        /// <param name="period">period, the default is 5m, e.g. [5m/1H/1D]</param>
        /// <param name="begin">begin, e.g. 1597026383085</param>
        /// <param name="end">end, e.g. 1597026383011</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<IEnumerable<OkxRatio>>> GetRubikLongShortRatioAsync(
            string currency,
            OkxPeriod period = OkxPeriod.FiveMinutes,
            long? begin = null,
            long? end = null,
            CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object> {
                { "ccy", currency},
                { "period", JsonConvert.SerializeObject(period, new PeriodConverter(false)) },
            };
            parameters.AddOptionalParameter("begin", begin?.ToString(OkxGlobals.OkxCultureInfo));
            parameters.AddOptionalParameter("end", end?.ToString(OkxGlobals.OkxCultureInfo));

            var result = await UnifiedApi.ExecuteAsync<OkxRestApiResponse<IEnumerable<OkxRatio>>>(UnifiedApi.GetUri(Endpoints_V5_RubikStat_ContractsLongShortAccountRatio), HttpMethod.Get, ct, parameters).ConfigureAwait(false);
            if (!result.Success) return result.AsError<IEnumerable<OkxRatio>>(new OkxRestApiError(result.Error.Code, result.Error.Message, result.Error.Data));
            if (result.Data.ErrorCode > 0) return result.AsError<IEnumerable<OkxRatio>>(new OkxRestApiError(result.Data.ErrorCode, result.Data.ErrorMessage, null));

            return result.As(result.Data.Data);
        }

        /// <summary>
        /// Open interest is the sum of all long and short futures and perpetual swap positions.
        /// </summary>
        /// <param name="currency">Currency</param>
        /// <param name="period">period, the default is 5m, e.g. [5m/1H/1D]</param>
        /// <param name="begin">begin, e.g. 1597026383085</param>
        /// <param name="end">end, e.g. 1597026383011</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<IEnumerable<OkxInterestVolume>> GetRubikContractSummary(
            string currency,
            OkxPeriod period = OkxPeriod.FiveMinutes,
            long? begin = null,
            long? end = null,
            CancellationToken ct = default) 
            => GetRubikContractSummaryAsync(currency, period, begin, end, ct).Result;
        /// <summary>
        /// Open interest is the sum of all long and short futures and perpetual swap positions.
        /// </summary>
        /// <param name="currency">Currency</param>
        /// <param name="period">period, the default is 5m, e.g. [5m/1H/1D]</param>
        /// <param name="begin">begin, e.g. 1597026383085</param>
        /// <param name="end">end, e.g. 1597026383011</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<IEnumerable<OkxInterestVolume>>> GetRubikContractSummaryAsync(
            string currency,
            OkxPeriod period = OkxPeriod.FiveMinutes,
            long? begin = null,
            long? end = null,
            CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object> {
                { "ccy", currency},
                { "period", JsonConvert.SerializeObject(period, new PeriodConverter(false)) },
            };
            parameters.AddOptionalParameter("begin", begin?.ToString(OkxGlobals.OkxCultureInfo));
            parameters.AddOptionalParameter("end", end?.ToString(OkxGlobals.OkxCultureInfo));

            var result = await UnifiedApi.ExecuteAsync<OkxRestApiResponse<IEnumerable<OkxInterestVolume>>>(UnifiedApi.GetUri(Endpoints_V5_RubikStat_ContractsOpenInterestVolume), HttpMethod.Get, ct, parameters).ConfigureAwait(false);
            if (!result.Success) return result.AsError<IEnumerable<OkxInterestVolume>>(new OkxRestApiError(result.Error.Code, result.Error.Message, result.Error.Data));
            if (result.Data.ErrorCode > 0) return result.AsError<IEnumerable<OkxInterestVolume>>(new OkxRestApiError(result.Data.ErrorCode, result.Data.ErrorMessage, null));

            return result.As(result.Data.Data);
        }

        /// <summary>
        /// This shows the sum of all open positions and how much total trading volume has taken place.
        /// </summary>
        /// <param name="currency">Currency</param>
        /// <param name="period">period, the default is 8H. e.g. [8H/1D]</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<IEnumerable<OkxInterestVolume>> GetRubikOptionsSummary(
            string currency,
            OkxPeriod period = OkxPeriod.FiveMinutes,
            CancellationToken ct = default) 
            => GetRubikOptionsSummaryAsync(currency, period, ct).Result;
        /// <summary>
        /// This shows the sum of all open positions and how much total trading volume has taken place.
        /// </summary>
        /// <param name="currency">Currency</param>
        /// <param name="period">period, the default is 8H. e.g. [8H/1D]</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<IEnumerable<OkxInterestVolume>>> GetRubikOptionsSummaryAsync(
            string currency,
            OkxPeriod period = OkxPeriod.FiveMinutes,
            CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object> {
                { "ccy", currency},
                { "period", JsonConvert.SerializeObject(period, new PeriodConverter(false)) },
            };

            var result = await UnifiedApi.ExecuteAsync<OkxRestApiResponse<IEnumerable<OkxInterestVolume>>>(UnifiedApi.GetUri(Endpoints_V5_RubikStat_OptionOpenInterestVolume), HttpMethod.Get, ct, parameters).ConfigureAwait(false);
            if (!result.Success) return result.AsError<IEnumerable<OkxInterestVolume>>(new OkxRestApiError(result.Error.Code, result.Error.Message, result.Error.Data));
            if (result.Data.ErrorCode > 0) return result.AsError<IEnumerable<OkxInterestVolume>>(new OkxRestApiError(result.Data.ErrorCode, result.Data.ErrorMessage, null));

            return result.As(result.Data.Data);
        }

        /// <summary>
        /// This shows the relative buy/sell volume for calls and puts. It shows whether traders are bullish or bearish on price and volatility.
        /// </summary>
        /// <param name="currency">Currency</param>
        /// <param name="period">period, the default is 8H. e.g. [8H/1D]</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<IEnumerable<OkxPutCallRatio>> GetRubikPutCallRatio(
            string currency,
            OkxPeriod period = OkxPeriod.FiveMinutes,
            CancellationToken ct = default) 
            => GetRubikPutCallRatioAsync(currency, period, ct).Result;
        /// <summary>
        /// This shows the relative buy/sell volume for calls and puts. It shows whether traders are bullish or bearish on price and volatility.
        /// </summary>
        /// <param name="currency">Currency</param>
        /// <param name="period">period, the default is 8H. e.g. [8H/1D]</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<IEnumerable<OkxPutCallRatio>>> GetRubikPutCallRatioAsync(
            string currency,
            OkxPeriod period = OkxPeriod.FiveMinutes,
            CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object> {
                { "ccy", currency},
                { "period", JsonConvert.SerializeObject(period, new PeriodConverter(false)) },
            };

            var result = await UnifiedApi.ExecuteAsync<OkxRestApiResponse<IEnumerable<OkxPutCallRatio>>>(UnifiedApi.GetUri(Endpoints_V5_RubikStat_OptionOpenInterestVolumeRatio), HttpMethod.Get, ct, parameters).ConfigureAwait(false);
            if (!result.Success) return result.AsError<IEnumerable<OkxPutCallRatio>>(new OkxRestApiError(result.Error.Code, result.Error.Message, result.Error.Data));
            if (result.Data.ErrorCode > 0) return result.AsError<IEnumerable<OkxPutCallRatio>>(new OkxRestApiError(result.Data.ErrorCode, result.Data.ErrorMessage, null));

            return result.As(result.Data.Data);
        }

        /// <summary>
        /// This shows the volume and open interest for each upcoming expiration. You can use this to see which expirations are currently the most popular to trade.
        /// </summary>
        /// <param name="currency">Currency</param>
        /// <param name="period">period, the default is 8H. e.g. [8H/1D]</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<IEnumerable<OkxInterestVolumeExpiry>> GetRubikInterestVolumeExpiry(
            string currency,
            OkxPeriod period = OkxPeriod.FiveMinutes,
            CancellationToken ct = default) 
            => GetRubikInterestVolumeExpiryAsync(currency, period, ct).Result;
        /// <summary>
        /// This shows the volume and open interest for each upcoming expiration. You can use this to see which expirations are currently the most popular to trade.
        /// </summary>
        /// <param name="currency">Currency</param>
        /// <param name="period">period, the default is 8H. e.g. [8H/1D]</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<IEnumerable<OkxInterestVolumeExpiry>>> GetRubikInterestVolumeExpiryAsync(
            string currency,
            OkxPeriod period = OkxPeriod.FiveMinutes,
            CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object> {
                { "ccy", currency},
                { "period", JsonConvert.SerializeObject(period, new PeriodConverter(false)) },
            };

            var result = await UnifiedApi.ExecuteAsync<OkxRestApiResponse<IEnumerable<OkxInterestVolumeExpiry>>>(UnifiedApi.GetUri(Endpoints_V5_RubikStat_OptionOpenInterestVolumeExpiry), HttpMethod.Get, ct, parameters).ConfigureAwait(false);
            if (!result.Success) return result.AsError<IEnumerable<OkxInterestVolumeExpiry>>(new OkxRestApiError(result.Error.Code, result.Error.Message, result.Error.Data));
            if (result.Data.ErrorCode > 0) return result.AsError<IEnumerable<OkxInterestVolumeExpiry>>(new OkxRestApiError(result.Data.ErrorCode, result.Data.ErrorMessage, null));

            return result.As(result.Data.Data);
        }

        /// <summary>
        /// This shows what option strikes are the most popular for each expiration.
        /// </summary>
        /// <param name="currency">Currency</param>
        /// <param name="expiryTime">expiry time (Format: YYYYMMdd, for example: "20210623")</param>
        /// <param name="period">period, the default is 8H. e.g. [8H/1D]</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<IEnumerable<OkxInterestVolumeStrike>> GetRubikInterestVolumeStrike(
            string currency,
            string expiryTime,
            OkxPeriod period = OkxPeriod.FiveMinutes,
            CancellationToken ct = default) 
            => GetRubikInterestVolumeStrikeAsync(currency, expiryTime, period, ct).Result;
        /// <summary>
        /// This shows what option strikes are the most popular for each expiration.
        /// </summary>
        /// <param name="currency">Currency</param>
        /// <param name="expiryTime">expiry time (Format: YYYYMMdd, for example: "20210623")</param>
        /// <param name="period">period, the default is 8H. e.g. [8H/1D]</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<IEnumerable<OkxInterestVolumeStrike>>> GetRubikInterestVolumeStrikeAsync(
            string currency,
            string expiryTime,
            OkxPeriod period = OkxPeriod.FiveMinutes,
            CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object> {
                { "ccy", currency},
                { "expTime", expiryTime},
                { "period", JsonConvert.SerializeObject(period, new PeriodConverter(false)) },
            };

            var result = await UnifiedApi.ExecuteAsync<OkxRestApiResponse<IEnumerable<OkxInterestVolumeStrike>>>(UnifiedApi.GetUri(Endpoints_V5_RubikStat_OptionOpenInterestVolumeStrike), HttpMethod.Get, ct, parameters).ConfigureAwait(false);
            if (!result.Success) return result.AsError<IEnumerable<OkxInterestVolumeStrike>>(new OkxRestApiError(result.Error.Code, result.Error.Message, result.Error.Data));
            if (result.Data.ErrorCode > 0) return result.AsError<IEnumerable<OkxInterestVolumeStrike>>(new OkxRestApiError(result.Data.ErrorCode, result.Data.ErrorMessage, null));

            return result.As(result.Data.Data);
        }

        /// <summary>
        /// This shows the relative buy/sell volume for calls and puts. It shows whether traders are bullish or bearish on price and volatility.
        /// </summary>
        /// <param name="currency">Currency</param>
        /// <param name="period">period, the default is 8H. e.g. [8H/1D]</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<OkxTakerFlow> GetRubikTakerFlow(
            string currency,
            OkxPeriod period = OkxPeriod.FiveMinutes,
            CancellationToken ct = default) 
            => GetRubikTakerFlowAsync(currency, period, ct).Result;
        /// <summary>
        /// This shows the relative buy/sell volume for calls and puts. It shows whether traders are bullish or bearish on price and volatility.
        /// </summary>
        /// <param name="currency">Currency</param>
        /// <param name="period">period, the default is 8H. e.g. [8H/1D]</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<OkxTakerFlow>> GetRubikTakerFlowAsync(
            string currency,
            OkxPeriod period = OkxPeriod.FiveMinutes,
            CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object> {
                { "ccy", currency},
                { "period", JsonConvert.SerializeObject(period, new PeriodConverter(false)) },
            };

            var result = await UnifiedApi.ExecuteAsync<OkxRestApiResponse<OkxTakerFlow>>(UnifiedApi.GetUri(Endpoints_V5_RubikStat_OptionTakerBrickVolume), HttpMethod.Get, ct, parameters).ConfigureAwait(false);
            if (!result.Success) return result.AsError<OkxTakerFlow>(new OkxRestApiError(result.Error.Code, result.Error.Message, result.Error.Data));
            if (result.Data.ErrorCode > 0) return result.AsError<OkxTakerFlow>(new OkxRestApiError(result.Data.ErrorCode, result.Data.ErrorMessage, null));

            return result.As(result.Data.Data);
        }
        #endregion
    }
}