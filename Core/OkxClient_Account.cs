using Newtonsoft.Json;
using SharpCryptoExchange.Objects;
using SharpCryptoExchange.Okx.Converters;
using SharpCryptoExchange.Okx.Enums;
using SharpCryptoExchange.Okx.Helpers;
using SharpCryptoExchange.Okx.Models.Account;
using SharpCryptoExchange.Okx.Models.Core;
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
        #region Account API Endpoints
        /// <summary>
        /// Retrieve a list of assets (with non-zero balance), remaining balance, and available amount in the account.
        /// </summary>
        /// <param name="currency">Currency</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<OkxAccountBalance> GetAccountBalance(string currency = null, CancellationToken ct = default)
            => GetAccountBalanceAsync(currency, ct).Result;
        /// <summary>
        /// Retrieve a list of assets (with non-zero balance), remaining balance, and available amount in the account.
        /// </summary>
        /// <param name="currency">Currency</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<OkxAccountBalance>> GetAccountBalanceAsync(string currency = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("ccy", currency);

            var result = await UnifiedApi.ExecuteAsync<OkxRestApiResponse<IEnumerable<OkxAccountBalance>>>(UnifiedApi.GetUri(Endpoints.V5.Account.Balance), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
            if (!result.Success) return result.AsError<OkxAccountBalance>(new OkxRestApiError(result.Error.Code, result.Error.Message, result.Error.Data));
            if (result.Data.ErrorCode > 0) return result.AsError<OkxAccountBalance>(new OkxRestApiError(result.Data.ErrorCode, result.Data.ErrorMessage, null));

            return result.As(result.Data.Data.FirstOrDefault());
        }

        /// <summary>
        /// Retrieve information on your positions. When the account is in net mode, net positions will be displayed, and when the account is in long/short mode, long or short positions will be displayed.
        /// </summary>
        /// <param name="instrumentType">Instrument Type</param>
        /// <param name="instrumentId">Instrument ID</param>
        /// <param name="positionId">Position ID</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<IEnumerable<OkxPosition>> GetAccountPositions(
            OkxInstrumentType? instrumentType = null,
            string instrumentId = null,
            string positionId = null,
            CancellationToken ct = default)
            => GetAccountPositionsAsync(instrumentType, instrumentId, positionId, ct).Result;
        /// <summary>
        /// Retrieve information on your positions. When the account is in net mode, net positions will be displayed, and when the account is in long/short mode, long or short positions will be displayed.
        /// </summary>
        /// <param name="instrumentType">Instrument Type</param>
        /// <param name="instrumentId">Instrument ID</param>
        /// <param name="positionId">Position ID</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<IEnumerable<OkxPosition>>> GetAccountPositionsAsync(
            OkxInstrumentType? instrumentType = null,
            string instrumentId = null,
            string positionId = null,
            CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>();
            if (instrumentType.HasValue)
                parameters.AddOptionalParameter("instType", JsonConvert.SerializeObject(instrumentType, new InstrumentTypeConverter(false)));
            parameters.AddOptionalParameter("instId", instrumentId);
            parameters.AddOptionalParameter("posId", positionId);

            var result = await UnifiedApi.ExecuteAsync<OkxRestApiResponse<IEnumerable<OkxPosition>>>(UnifiedApi.GetUri(Endpoints.V5.Account.Positions), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
            if (!result.Success) return result.AsError<IEnumerable<OkxPosition>>(new OkxRestApiError(result.Error.Code, result.Error.Message, result.Error.Data));
            if (result.Data.ErrorCode > 0) return result.AsError<IEnumerable<OkxPosition>>(new OkxRestApiError(result.Data.ErrorCode, result.Data.ErrorMessage, null));

            return result.As(result.Data.Data);
        }


        // TODO: Get positions history


        /// <summary>
        /// Get account and position risk
        /// </summary>
        /// <param name="instrumentType">Instrument Type</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<IEnumerable<OkxPositionRisk>> GetAccountPositionRisk(OkxInstrumentType? instrumentType = null, CancellationToken ct = default)
            => GetAccountPositionRiskAsync(instrumentType, ct).Result;
        /// <summary>
        /// Get account and position risk
        /// </summary>
        /// <param name="instrumentType">Instrument Type</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<IEnumerable<OkxPositionRisk>>> GetAccountPositionRiskAsync(OkxInstrumentType? instrumentType = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>();
            if (instrumentType.HasValue)
                parameters.AddOptionalParameter("instType", JsonConvert.SerializeObject(instrumentType, new InstrumentTypeConverter(false)));

            var result = await UnifiedApi.ExecuteAsync<OkxRestApiResponse<IEnumerable<OkxPositionRisk>>>(UnifiedApi.GetUri(Endpoints.V5.Account.PositionRisk), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
            if (!result.Success) return result.AsError<IEnumerable<OkxPositionRisk>>(new OkxRestApiError(result.Error.Code, result.Error.Message, result.Error.Data));
            if (result.Data.ErrorCode > 0) return result.AsError<IEnumerable<OkxPositionRisk>>(new OkxRestApiError(result.Data.ErrorCode, result.Data.ErrorMessage, null));

            return result.As(result.Data.Data);
        }

        /// <summary>
        /// Retrieve the bills of the account. The bill refers to all transaction records that result in changing the balance of an account. Pagination is supported, and the response is sorted with the most recent first. This endpoint can retrieve data from the last 7 days.
        /// </summary>
        /// <param name="instrumentType">Instrument Type</param>
        /// <param name="currency">Currency</param>
        /// <param name="marginMode">Margin Mode</param>
        /// <param name="contractType">Contract Type</param>
        /// <param name="billType">Bill Type</param>
        /// <param name="billSubType">Bill Sub Type</param>
        /// <param name="after">Pagination of data to return records earlier than the requested ts, Unix timestamp format in milliseconds, e.g. 1597026383085</param>
        /// <param name="before">Pagination of data to return records newer than the requested ts, Unix timestamp format in milliseconds, e.g. 1597026383085</param>
        /// <param name="limit">Number of results per request. The maximum is 100; the default is 100.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<IEnumerable<OkxAccountBill>> GetBillHistory(
            OkxInstrumentType? instrumentType = null,
            string currency = null,
            OkxMarginMode? marginMode = null,
            OkxContractType? contractType = null,
            OkxAccountBillType? billType = null,
            OkxAccountBillSubType? billSubType = null,
            long? after = null,
            long? before = null,
            int limit = 100,
            CancellationToken ct = default)
            => GetBillHistoryAsync(
            instrumentType,
            currency,
            marginMode,
            contractType,
            billType,
            billSubType,
            after,
            before,
            limit,
            ct).Result;
        /// <summary>
        /// Retrieve the bills of the account. The bill refers to all transaction records that result in changing the balance of an account. Pagination is supported, and the response is sorted with the most recent first. This endpoint can retrieve data from the last 7 days.
        /// </summary>
        /// <param name="instrumentType">Instrument Type</param>
        /// <param name="currency">Currency</param>
        /// <param name="marginMode">Margin Mode</param>
        /// <param name="contractType">Contract Type</param>
        /// <param name="billType">Bill Type</param>
        /// <param name="billSubType">Bill Sub Type</param>
        /// <param name="after">Pagination of data to return records earlier than the requested ts, Unix timestamp format in milliseconds, e.g. 1597026383085</param>
        /// <param name="before">Pagination of data to return records newer than the requested ts, Unix timestamp format in milliseconds, e.g. 1597026383085</param>
        /// <param name="limit">Number of results per request. The maximum is 100; the default is 100.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<IEnumerable<OkxAccountBill>>> GetBillHistoryAsync(
            OkxInstrumentType? instrumentType = null,
            string currency = null,
            OkxMarginMode? marginMode = null,
            OkxContractType? contractType = null,
            OkxAccountBillType? billType = null,
            OkxAccountBillSubType? billSubType = null,
            long? after = null,
            long? before = null,
            int limit = 100,
            CancellationToken ct = default)
        {
            if (limit < 1 || limit > 100)
                throw new ArgumentException("Limit can be between 1-100.");

            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("ccy", currency);

            parameters.AddOptionalParameter("after", $"{after}");
            parameters.AddOptionalParameter("before", $"{before}");
            parameters.AddOptionalParameter("limit", $"{limit}");

            if (instrumentType.HasValue)
                parameters.AddOptionalParameter("instType", JsonConvert.SerializeObject(instrumentType, new InstrumentTypeConverter(false)));
            if (marginMode.HasValue)
                parameters.AddOptionalParameter("mgnMode", JsonConvert.SerializeObject(marginMode, new MarginModeConverter(false)));
            if (contractType.HasValue)
                parameters.AddOptionalParameter("ctType", JsonConvert.SerializeObject(contractType, new ContractTypeConverter(false)));
            if (billType.HasValue)
                parameters.AddOptionalParameter("type", JsonConvert.SerializeObject(billType, new AccountBillTypeConverter(false)));
            if (billSubType.HasValue)
                parameters.AddOptionalParameter("subType", JsonConvert.SerializeObject(billSubType, new AccountBillSubTypeConverter(false)));

            var result = await UnifiedApi.ExecuteAsync<OkxRestApiResponse<IEnumerable<OkxAccountBill>>>(UnifiedApi.GetUri(Endpoints.V5.Account.Bills), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
            if (!result.Success) return result.AsError<IEnumerable<OkxAccountBill>>(new OkxRestApiError(result.Error.Code, result.Error.Message, result.Error.Data));
            if (result.Data.ErrorCode > 0) return result.AsError<IEnumerable<OkxAccountBill>>(new OkxRestApiError(result.Data.ErrorCode, result.Data.ErrorMessage, null));

            return result.As(result.Data.Data);
        }

        /// <summary>
        /// Retrieve the account’s bills. The bill refers to all transaction records that result in changing the balance of an account. Pagination is supported, and the response is sorted with most recent first. This endpoint can retrieve data from the last 3 months.
        /// </summary>
        /// <param name="instrumentType">Instrument Type</param>
        /// <param name="currency">Currency</param>
        /// <param name="marginMode">Margin Mode</param>
        /// <param name="contractType">Contract Type</param>
        /// <param name="billType">Bill Type</param>
        /// <param name="billSubType">Bill Sub Type</param>
        /// <param name="after">Pagination of data to return records earlier than the requested ts, Unix timestamp format in milliseconds, e.g. 1597026383085</param>
        /// <param name="before">Pagination of data to return records newer than the requested ts, Unix timestamp format in milliseconds, e.g. 1597026383085</param>
        /// <param name="limit">Number of results per request. The maximum is 100; the default is 100.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<IEnumerable<OkxAccountBill>> GetBillArchive(
            OkxInstrumentType? instrumentType = null,
            string currency = null,
            OkxMarginMode? marginMode = null,
            OkxContractType? contractType = null,
            OkxAccountBillType? billType = null,
            OkxAccountBillSubType? billSubType = null,
            long? after = null,
            long? before = null,
            int limit = 100,
            CancellationToken ct = default)
            => GetBillArchiveAsync(
            instrumentType,
            currency,
            marginMode,
            contractType,
            billType,
            billSubType,
            after,
            before,
            limit,
            ct).Result;
        /// <summary>
        /// Retrieve the account’s bills. The bill refers to all transaction records that result in changing the balance of an account. Pagination is supported, and the response is sorted with most recent first. This endpoint can retrieve data from the last 3 months.
        /// </summary>
        /// <param name="instrumentType">Instrument Type</param>
        /// <param name="currency">Currency</param>
        /// <param name="marginMode">Margin Mode</param>
        /// <param name="contractType">Contract Type</param>
        /// <param name="billType">Bill Type</param>
        /// <param name="billSubType">Bill Sub Type</param>
        /// <param name="after">Pagination of data to return records earlier than the requested ts, Unix timestamp format in milliseconds, e.g. 1597026383085</param>
        /// <param name="before">Pagination of data to return records newer than the requested ts, Unix timestamp format in milliseconds, e.g. 1597026383085</param>
        /// <param name="limit">Number of results per request. The maximum is 100; the default is 100.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<IEnumerable<OkxAccountBill>>> GetBillArchiveAsync(
            OkxInstrumentType? instrumentType = null,
            string currency = null,
            OkxMarginMode? marginMode = null,
            OkxContractType? contractType = null,
            OkxAccountBillType? billType = null,
            OkxAccountBillSubType? billSubType = null,
            long? after = null,
            long? before = null,
            int limit = 100,
            CancellationToken ct = default)
        {
            if (limit < 1 || limit > 100)
                throw new ArgumentException("Limit can be between 1-100.");

            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("ccy", currency);
            parameters.AddOptionalParameter("after", $"{after}");
            parameters.AddOptionalParameter("before", $"{before}");
            parameters.AddOptionalParameter("limit", $"{limit}");

            if (instrumentType.HasValue)
                parameters.AddOptionalParameter("instType", JsonConvert.SerializeObject(instrumentType, new InstrumentTypeConverter(false)));
            if (marginMode.HasValue)
                parameters.AddOptionalParameter("mgnMode", JsonConvert.SerializeObject(marginMode, new MarginModeConverter(false)));
            if (contractType.HasValue)
                parameters.AddOptionalParameter("ctType", JsonConvert.SerializeObject(contractType, new ContractTypeConverter(false)));
            if (billType.HasValue)
                parameters.AddOptionalParameter("type", JsonConvert.SerializeObject(billType, new AccountBillTypeConverter(false)));
            if (billSubType.HasValue)
                parameters.AddOptionalParameter("subType", JsonConvert.SerializeObject(billSubType, new AccountBillSubTypeConverter(false)));

            var result = await UnifiedApi.ExecuteAsync<OkxRestApiResponse<IEnumerable<OkxAccountBill>>>(UnifiedApi.GetUri(Endpoints.V5.Account.BillsArchive), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
            if (!result.Success) return result.AsError<IEnumerable<OkxAccountBill>>(new OkxRestApiError(result.Error.Code, result.Error.Message, result.Error.Data));
            if (result.Data.ErrorCode > 0) return result.AsError<IEnumerable<OkxAccountBill>>(new OkxRestApiError(result.Data.ErrorCode, result.Data.ErrorMessage, null));

            return result.As(result.Data.Data);
        }

        /// <summary>
        /// Retrieve current account configuration.
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<OkxAccountConfiguration> GetAccountConfiguration(CancellationToken ct = default)
            => GetAccountConfigurationAsync(ct).Result;
        /// <summary>
        /// Retrieve current account configuration.
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<OkxAccountConfiguration>> GetAccountConfigurationAsync(CancellationToken ct = default)
        {
            var result = await UnifiedApi.ExecuteAsync<OkxRestApiResponse<IEnumerable<OkxAccountConfiguration>>>(UnifiedApi.GetUri(Endpoints.V5.Account.Config), HttpMethod.Get, ct, null, true).ConfigureAwait(false);
            if (!result.Success) return result.AsError<OkxAccountConfiguration>(new OkxRestApiError(result.Error.Code, result.Error.Message, result.Error.Data));
            if (result.Data.ErrorCode > 0) return result.AsError<OkxAccountConfiguration>(new OkxRestApiError(result.Data.ErrorCode, result.Data.ErrorMessage, null));

            return result.As(result.Data.Data.FirstOrDefault());
        }

        /// <summary>
        /// FUTURES and SWAP support both long/short mode and net mode. In net mode, users can only have positions in one direction; In long/short mode, users can hold positions in long and short directions.
        /// </summary>
        /// <param name="positionMode"></param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<OkxAccountPositionMode> SetAccountPositionMode(Enums.OkxPositionMode positionMode, CancellationToken ct = default)
            => SetAccountPositionModeAsync(positionMode, ct).Result;
        /// <summary>
        /// FUTURES and SWAP support both long/short mode and net mode. In net mode, users can only have positions in one direction; In long/short mode, users can hold positions in long and short directions.
        /// </summary>
        /// <param name="positionMode"></param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<OkxAccountPositionMode>> SetAccountPositionModeAsync(Enums.OkxPositionMode positionMode, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object> {
                {"posMode", JsonConvert.SerializeObject(positionMode, new PositionModeConverter(false)) },
            };

            var result = await UnifiedApi.ExecuteAsync<OkxRestApiResponse<IEnumerable<OkxAccountPositionMode>>>(UnifiedApi.GetUri(Endpoints.V5.Account.SetPositionMode), HttpMethod.Post, ct, parameters, true).ConfigureAwait(false);
            if (!result.Success) return result.AsError<OkxAccountPositionMode>(new OkxRestApiError(result.Error.Code, result.Error.Message, result.Error.Data));
            if (result.Data.ErrorCode > 0) return result.AsError<OkxAccountPositionMode>(new OkxRestApiError(result.Data.ErrorCode, result.Data.ErrorMessage, null));

            return result.As(result.Data.Data.FirstOrDefault());
        }

        /// <summary>
        /// Get Leverage
        /// </summary>
        /// <param name="instrumentIds">Single instrument ID or multiple instrument IDs (no more than 20) separated with comma</param>
        /// <param name="marginMode">Margin Mode</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<IEnumerable<OkxLeverage>> GetAccountLeverage(
            string instrumentIds,
            OkxMarginMode marginMode,
            CancellationToken ct = default)
            => GetAccountLeverageAsync(instrumentIds, marginMode, ct).Result;
        /// <summary>
        /// Get Leverage
        /// </summary>
        /// <param name="instrumentIds">Single instrument ID or multiple instrument IDs (no more than 20) separated with comma</param>
        /// <param name="marginMode">Margin Mode</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<IEnumerable<OkxLeverage>>> GetAccountLeverageAsync(
            string instrumentIds,
            OkxMarginMode marginMode,
            CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object> {
                {"instId", instrumentIds },
                {"mgnMode", JsonConvert.SerializeObject(marginMode, new MarginModeConverter(false)) },
            };

            var result = await UnifiedApi.ExecuteAsync<OkxRestApiResponse<IEnumerable<OkxLeverage>>>(UnifiedApi.GetUri(Endpoints.V5.Account.LeverageInfo), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
            if (!result.Success) return result.AsError<IEnumerable<OkxLeverage>>(new OkxRestApiError(result.Error.Code, result.Error.Message, result.Error.Data));
            if (result.Data.ErrorCode > 0) return result.AsError<IEnumerable<OkxLeverage>>(new OkxRestApiError(result.Data.ErrorCode, result.Data.ErrorMessage, null));

            return result.As(result.Data.Data);
        }

        /// <summary>
        /// The following are the setting leverage cases for an instrument:
        /// Set leverage for isolated MARGIN at pairs level.
        /// Set leverage for cross MARGIN in Single-currency margin at pairs level.
        /// Set leverage for cross MARGIN in Multi-currency margin at currency level.
        /// Set leverage for cross/isolated FUTURES/SWAP at underlying/contract level.
        /// </summary>
        /// <param name="leverage">Leverage</param>
        /// <param name="currency">Currency</param>
        /// <param name="instrumentId">Instrument ID</param>
        /// <param name="marginMode">Margin Mode</param>
        /// <param name="positionSide">Position Side</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<IEnumerable<OkxLeverage>> SetAccountLeverage(
            int leverage,
            string currency = null,
            string instrumentId = null,
            OkxMarginMode? marginMode = null,
            OkxPositionSide? positionSide = null,
            CancellationToken ct = default)
            => SetAccountLeverageAsync(leverage, currency, instrumentId, marginMode, positionSide, ct).Result;
        /// <summary>
        /// The following are the setting leverage cases for an instrument:
        /// Set leverage for isolated MARGIN at pairs level.
        /// Set leverage for cross MARGIN in Single-currency margin at pairs level.
        /// Set leverage for cross MARGIN in Multi-currency margin at currency level.
        /// Set leverage for cross/isolated FUTURES/SWAP at underlying/contract level.
        /// </summary>
        /// <param name="leverage">Leverage</param>
        /// <param name="currency">Currency</param>
        /// <param name="instrumentId">Instrument ID</param>
        /// <param name="marginMode">Margin Mode</param>
        /// <param name="positionSide">Position Side</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<IEnumerable<OkxLeverage>>> SetAccountLeverageAsync(
            int leverage,
            string currency = null,
            string instrumentId = null,
            OkxMarginMode? marginMode = null,
            OkxPositionSide? positionSide = null,
            CancellationToken ct = default)
        {
            if (leverage < 1)
                throw new ArgumentException("Invalid Leverage");

            if (string.IsNullOrEmpty(currency) && string.IsNullOrEmpty(instrumentId))
                throw new ArgumentException("Either instId or ccy is required; if both are passed, instId will be used by default.");

            if (marginMode == null)
                throw new ArgumentException("marginMode is required");

            var parameters = new Dictionary<string, object> {
                {"lever", $"{leverage}" },
                {"mgnMode", JsonConvert.SerializeObject(marginMode, new MarginModeConverter(false)) },
            };
            parameters.AddOptionalParameter("ccy", currency);
            parameters.AddOptionalParameter("instId", instrumentId);
            if (positionSide.HasValue)
                parameters.AddOptionalParameter("posSide", JsonConvert.SerializeObject(positionSide, new PositionSideConverter(false)));

            var result = await UnifiedApi.ExecuteAsync<OkxRestApiResponse<IEnumerable<OkxLeverage>>>(UnifiedApi.GetUri(Endpoints.V5.Account.SetLeverage), HttpMethod.Post, ct, parameters, true).ConfigureAwait(false);
            if (!result.Success) return result.AsError<IEnumerable<OkxLeverage>>(new OkxRestApiError(result.Error.Code, result.Error.Message, result.Error.Data));
            if (result.Data.ErrorCode > 0) return result.AsError<IEnumerable<OkxLeverage>>(new OkxRestApiError(result.Data.ErrorCode, result.Data.ErrorMessage, null));

            return result.As(result.Data.Data);
        }

        /// <summary>
        /// Get maximum buy/sell amount or open amount
        /// </summary>
        /// <param name="instrumentId">Instrument ID</param>
        /// <param name="tradeMode">Trade Mode</param>
        /// <param name="currency">Currency</param>
        /// <param name="price">Price</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<IEnumerable<OkxMaximumAmount>> GetMaximumAmount(
            string instrumentId,
            OkxTradeMode tradeMode,
            string currency = null,
            decimal? price = null,
            CancellationToken ct = default)
            => GetMaximumAmountAsync(instrumentId, tradeMode, currency, price, ct).Result;
        /// <summary>
        /// Get maximum buy/sell amount or open amount
        /// </summary>
        /// <param name="instrumentId">Instrument ID</param>
        /// <param name="tradeMode">Trade Mode</param>
        /// <param name="currency">Currency</param>
        /// <param name="price">Price</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<IEnumerable<OkxMaximumAmount>>> GetMaximumAmountAsync(
            string instrumentId,
            OkxTradeMode tradeMode,
            string currency = null,
            decimal? price = null,
            CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object> {
                {"instId", instrumentId },
                {"tdMode", JsonConvert.SerializeObject(tradeMode, new TradeModeConverter(false)) },
            };
            parameters.AddOptionalParameter("ccy", currency);
            parameters.AddOptionalParameter("px", price?.ToString(OkxGlobals.OkxCultureInfo));

            var result = await UnifiedApi.ExecuteAsync<OkxRestApiResponse<IEnumerable<OkxMaximumAmount>>>(UnifiedApi.GetUri(Endpoints.V5.Account.MaxSize), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
            if (!result.Success) return result.AsError<IEnumerable<OkxMaximumAmount>>(new OkxRestApiError(result.Error.Code, result.Error.Message, result.Error.Data));
            if (result.Data.ErrorCode > 0) return result.AsError<IEnumerable<OkxMaximumAmount>>(new OkxRestApiError(result.Data.ErrorCode, result.Data.ErrorMessage, null));

            return result.As(result.Data.Data);
        }

        /// <summary>
        /// Get Maximum Available Tradable Amount
        /// </summary>
        /// <param name="instrumentId">Instrument ID</param>
        /// <param name="tradeMode">Trade Mode</param>
        /// <param name="currency">Currency</param>
        /// <param name="reduceOnly">Reduce Only</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<IEnumerable<OkxMaximumAvailableAmount>> GetMaximumAvailableAmount(
            string instrumentId,
            OkxTradeMode tradeMode,
            string currency = null,
            bool? reduceOnly = null,
            CancellationToken ct = default)
            => GetMaximumAvailableAmountAsync(instrumentId, tradeMode, currency, reduceOnly, ct).Result;
        /// <summary>
        /// Get Maximum Available Tradable Amount
        /// </summary>
        /// <param name="instrumentId">Instrument ID</param>
        /// <param name="tradeMode">Trade Mode</param>
        /// <param name="currency">Currency</param>
        /// <param name="reduceOnly">Reduce Only</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<IEnumerable<OkxMaximumAvailableAmount>>> GetMaximumAvailableAmountAsync(
            string instrumentId,
            OkxTradeMode tradeMode,
            string currency = null,
            bool? reduceOnly = null,
            CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object> {
                {"instId", instrumentId },
                {"tdMode", JsonConvert.SerializeObject(tradeMode, new TradeModeConverter(false)) },
            };
            parameters.AddOptionalParameter("ccy", currency);
            parameters.AddOptionalParameter("reduceOnly", reduceOnly);

            var result = await UnifiedApi.ExecuteAsync<OkxRestApiResponse<IEnumerable<OkxMaximumAvailableAmount>>>(UnifiedApi.GetUri(Endpoints.V5.Account.MaxAvailSize), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
            if (!result.Success) return result.AsError<IEnumerable<OkxMaximumAvailableAmount>>(new OkxRestApiError(result.Error.Code, result.Error.Message, result.Error.Data));
            if (result.Data.ErrorCode > 0) return result.AsError<IEnumerable<OkxMaximumAvailableAmount>>(new OkxRestApiError(result.Data.ErrorCode, result.Data.ErrorMessage, null));

            return result.As(result.Data.Data);
        }

        /// <summary>
        /// Increase or decrease the margin of the isolated position.
        /// </summary>
        /// <param name="instrumentId">Instrument ID</param>
        /// <param name="positionSide">Position Side</param>
        /// <param name="marginAddReduce">Type</param>
        /// <param name="amount">Amount</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<IEnumerable<OkxMarginAmount>> SetMarginAmount(
            string instrumentId,
            OkxPositionSide positionSide,
            OkxMarginAddReduce marginAddReduce,
            decimal amount,
            CancellationToken ct = default)
            => SetMarginAmountAsync(instrumentId, positionSide, marginAddReduce, amount, ct).Result;
        /// <summary>
        /// Increase or decrease the margin of the isolated position.
        /// </summary>
        /// <param name="instrumentId">Instrument ID</param>
        /// <param name="positionSide">Position Side</param>
        /// <param name="marginAddReduce">Type</param>
        /// <param name="amount">Amount</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<IEnumerable<OkxMarginAmount>>> SetMarginAmountAsync(
            string instrumentId,
            OkxPositionSide positionSide,
            OkxMarginAddReduce marginAddReduce,
            decimal amount,
            CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object> {
                {"instId", instrumentId },
                {"posSide", JsonConvert.SerializeObject(positionSide, new PositionSideConverter(false)) },
                {"type", JsonConvert.SerializeObject(marginAddReduce, new MarginAddReduceConverter(false)) },
                {"amt", amount.ToString(OkxGlobals.OkxCultureInfo) },
            };

            var result = await UnifiedApi.ExecuteAsync<OkxRestApiResponse<IEnumerable<OkxMarginAmount>>>(UnifiedApi.GetUri(Endpoints.V5.Account.PositionMarginBalance), HttpMethod.Post, ct, parameters, true).ConfigureAwait(false);
            if (!result.Success) return result.AsError<IEnumerable<OkxMarginAmount>>(new OkxRestApiError(result.Error.Code, result.Error.Message, result.Error.Data));
            if (result.Data.ErrorCode > 0) return result.AsError<IEnumerable<OkxMarginAmount>>(new OkxRestApiError(result.Data.ErrorCode, result.Data.ErrorMessage, null));

            return result.As(result.Data.Data);
        }

        /// <summary>
        /// Get the maximum loan of instrument
        /// </summary>
        /// <param name="instrumentId">Instrument ID</param>
        /// <param name="marginMode">Margin Mode</param>
        /// <param name="marginCurrency">Margin Currency</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<IEnumerable<OkxMaximumLoanAmount>> GetMaximumLoanAmount(
            string instrumentId,
            OkxMarginMode marginMode,
            string marginCurrency = null,
            CancellationToken ct = default)
            => GetMaximumLoanAmountAsync(instrumentId, marginMode, marginCurrency, ct).Result;
        /// <summary>
        /// Get the maximum loan of instrument
        /// </summary>
        /// <param name="instrumentId">Instrument ID</param>
        /// <param name="marginMode">Margin Mode</param>
        /// <param name="marginCurrency">Margin Currency</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<IEnumerable<OkxMaximumLoanAmount>>> GetMaximumLoanAmountAsync(
            string instrumentId,
            OkxMarginMode marginMode,
            string marginCurrency = null,
            CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object> {
                {"instId", instrumentId },
                {"mgnMode", JsonConvert.SerializeObject(marginMode, new MarginModeConverter(false)) },
            };
            parameters.AddOptionalParameter("mgnCcy", marginCurrency);

            var result = await UnifiedApi.ExecuteAsync<OkxRestApiResponse<IEnumerable<OkxMaximumLoanAmount>>>(UnifiedApi.GetUri(Endpoints.V5.Account.MaxLoan), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
            if (!result.Success) return result.AsError<IEnumerable<OkxMaximumLoanAmount>>(new OkxRestApiError(result.Error.Code, result.Error.Message, result.Error.Data));
            if (result.Data.ErrorCode > 0) return result.AsError<IEnumerable<OkxMaximumLoanAmount>>(new OkxRestApiError(result.Data.ErrorCode, result.Data.ErrorMessage, null));

            return result.As(result.Data.Data);
        }

        /// <summary>
        /// Get Fee Rates
        /// </summary>
        /// <param name="instrumentType">Instrument Type</param>
        /// <param name="instrumentId">Instrument ID</param>
        /// <param name="underlying">Underlying</param>
        /// <param name="category">Category</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<OkxFeeRate> GetFeeRates(
            OkxInstrumentType instrumentType,
            string instrumentId = null,
            string underlying = null,
            OkxFeeRateCategory? category = null,
            CancellationToken ct = default)
            => GetFeeRatesAsync(instrumentType, instrumentId, underlying, category, ct).Result;
        /// <summary>
        /// Get Fee Rates
        /// </summary>
        /// <param name="instrumentType">Instrument Type</param>
        /// <param name="instrumentId">Instrument ID</param>
        /// <param name="underlying">Underlying</param>
        /// <param name="category">Category</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<OkxFeeRate>> GetFeeRatesAsync(
            OkxInstrumentType instrumentType,
            string instrumentId = null,
            string underlying = null,
            OkxFeeRateCategory? category = null,
            CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object> {
                {"instType", JsonConvert.SerializeObject(instrumentType, new InstrumentTypeConverter(false)) },
            };
            parameters.AddOptionalParameter("instId", instrumentId);
            parameters.AddOptionalParameter("uly", underlying);
            parameters.AddOptionalParameter("category", JsonConvert.SerializeObject(category, new FeeRateCategoryConverter(false)));

            var result = await UnifiedApi.ExecuteAsync<OkxRestApiResponse<IEnumerable<OkxFeeRate>>>(UnifiedApi.GetUri(Endpoints.V5.Account.TradeFee), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
            if (!result.Success) return result.AsError<OkxFeeRate>(new OkxRestApiError(result.Error.Code, result.Error.Message, result.Error.Data));
            if (result.Data.ErrorCode > 0) return result.AsError<OkxFeeRate>(new OkxRestApiError(result.Data.ErrorCode, result.Data.ErrorMessage, null));

            return result.As(result.Data.Data.FirstOrDefault());
        }

        /// <summary>
        /// Get interest-accrued
        /// </summary>
        /// <param name="instrumentId">Instrument ID</param>
        /// <param name="currency">Currency</param>
        /// <param name="marginMode">Margin Mode</param>
        /// <param name="after">Pagination of data to return records earlier than the requested ts, Unix timestamp format in milliseconds, e.g. 1597026383085</param>
        /// <param name="before">Pagination of data to return records newer than the requested ts, Unix timestamp format in milliseconds, e.g. 1597026383085</param>
        /// <param name="limit">Number of results per request. The maximum is 100; the default is 100.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<IEnumerable<OkxInterestAccrued>> GetInterestAccrued(
            string instrumentId = null,
            string currency = null,
            OkxMarginMode? marginMode = null,
            long? after = null,
            long? before = null,
            int limit = 100,
            CancellationToken ct = default)
            => GetInterestAccruedAsync(
            instrumentId,
            currency,
            marginMode,
            after,
            before,
            limit,
            ct).Result;
        /// <summary>
        /// Get interest-accrued
        /// </summary>
        /// <param name="instrumentId">Instrument ID</param>
        /// <param name="currency">Currency</param>
        /// <param name="marginMode">Margin Mode</param>
        /// <param name="after">Pagination of data to return records earlier than the requested ts, Unix timestamp format in milliseconds, e.g. 1597026383085</param>
        /// <param name="before">Pagination of data to return records newer than the requested ts, Unix timestamp format in milliseconds, e.g. 1597026383085</param>
        /// <param name="limit">Number of results per request. The maximum is 100; the default is 100.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<IEnumerable<OkxInterestAccrued>>> GetInterestAccruedAsync(
            string instrumentId = null,
            string currency = null,
            OkxMarginMode? marginMode = null,
            long? after = null,
            long? before = null,
            int limit = 100,
            CancellationToken ct = default)
        {
            if (limit < 1 || limit > 100)
                throw new ArgumentException("Limit can be between 1-100.");

            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("instId", instrumentId);
            parameters.AddOptionalParameter("ccy", currency);
            parameters.AddOptionalParameter("after", $"{after}");
            parameters.AddOptionalParameter("before", $"{before}");
            parameters.AddOptionalParameter("limit", $"{limit}");

            if (marginMode.HasValue)
                parameters.AddOptionalParameter("mgnMode", JsonConvert.SerializeObject(marginMode, new MarginModeConverter(false)));

            var result = await UnifiedApi.ExecuteAsync<OkxRestApiResponse<IEnumerable<OkxInterestAccrued>>>(UnifiedApi.GetUri(Endpoints.V5.Account.InterestAccrued), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
            if (!result.Success) return result.AsError<IEnumerable<OkxInterestAccrued>>(new OkxRestApiError(result.Error.Code, result.Error.Message, result.Error.Data));
            if (result.Data.ErrorCode > 0) return result.AsError<IEnumerable<OkxInterestAccrued>>(new OkxRestApiError(result.Data.ErrorCode, result.Data.ErrorMessage, null));

            return result.As(result.Data.Data);
        }

        /// <summary>
        /// Get the user's current leveraged currency borrowing interest rate
        /// </summary>
        /// <param name="currency">Currency</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<IEnumerable<OkxInterestRate>> GetInterestRate(
            string currency = null,
            CancellationToken ct = default)
            => GetInterestRateAsync(
            currency,
            ct).Result;
        /// <summary>
        /// Get the user's current leveraged currency borrowing interest rate
        /// </summary>
        /// <param name="currency">Currency</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<IEnumerable<OkxInterestRate>>> GetInterestRateAsync(
            string currency = null,
            CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("ccy", currency);

            var result = await UnifiedApi.ExecuteAsync<OkxRestApiResponse<IEnumerable<OkxInterestRate>>>(UnifiedApi.GetUri(Endpoints.V5.Account.InterestRate), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
            if (!result.Success) return result.AsError<IEnumerable<OkxInterestRate>>(new OkxRestApiError(result.Error.Code, result.Error.Message, result.Error.Data));
            if (result.Data.ErrorCode > 0) return result.AsError<IEnumerable<OkxInterestRate>>(new OkxRestApiError(result.Data.ErrorCode, result.Data.ErrorMessage, null));

            return result.As(result.Data.Data);
        }

        // TODO: Get Greeks

        /// <summary>
        /// Set the display type of Greeks.
        /// </summary>
        /// <param name="greeksType">Display type of Greeks.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<OkxAccountGreeksType> SetGreeks(Enums.OkxGreeksType greeksType, CancellationToken ct = default)
            => SetGreeksAsync(greeksType, ct).Result;
        /// <summary>
        /// Set the display type of Greeks.
        /// </summary>
        /// <param name="greeksType">Display type of Greeks.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<OkxAccountGreeksType>> SetGreeksAsync(Enums.OkxGreeksType greeksType, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object> {
                {"greeksType", JsonConvert.SerializeObject(greeksType, new GreeksTypeConverter(false)) },
            };

            var result = await UnifiedApi.ExecuteAsync<OkxRestApiResponse<IEnumerable<OkxAccountGreeksType>>>(UnifiedApi.GetUri(Endpoints.V5.Account.SetGreeks), HttpMethod.Post, ct, parameters, true).ConfigureAwait(false);
            if (!result.Success) return result.AsError<OkxAccountGreeksType>(new OkxRestApiError(result.Error.Code, result.Error.Message, result.Error.Data));
            if (result.Data.ErrorCode > 0) return result.AsError<OkxAccountGreeksType>(new OkxRestApiError(result.Data.ErrorCode, result.Data.ErrorMessage, null));

            return result.As(result.Data.Data.FirstOrDefault());
        }

        // TODO: Isolated margin trading settings

        /// <summary>
        /// Retrieve the maximum transferable amount.
        /// </summary>
        /// <param name="currency">Currency</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<IEnumerable<OkxWithdrawalAmount>> GetMaximumWithdrawals(
            string currency = null,
            CancellationToken ct = default)
            => GetMaximumWithdrawalsAsync(
            currency,
            ct).Result;
        /// <summary>
        /// Retrieve the maximum transferable amount.
        /// </summary>
        /// <param name="currency">Currency</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<IEnumerable<OkxWithdrawalAmount>>> GetMaximumWithdrawalsAsync(
            string currency = null,
            CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("ccy", currency);

            var result = await UnifiedApi.ExecuteAsync<OkxRestApiResponse<IEnumerable<OkxWithdrawalAmount>>>(UnifiedApi.GetUri(Endpoints.V5.Account.MaxWithdrawal), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
            if (!result.Success) return result.AsError<IEnumerable<OkxWithdrawalAmount>>(new OkxRestApiError(result.Error.Code, result.Error.Message, result.Error.Data));
            if (result.Data.ErrorCode > 0) return result.AsError<IEnumerable<OkxWithdrawalAmount>>(new OkxRestApiError(result.Data.ErrorCode, result.Data.ErrorMessage, null));

            return result.As(result.Data.Data);
        }


        // TODO: Get account risk state
        // TODO: VIP loans borrow and repay
        // TODO: Get borrow and repay history for VIP loans
        // TODO: Get borrow interest and limit
        // TODO: Position builder
        // TODO: Get PM limitation


        #endregion
    }
}


