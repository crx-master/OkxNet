using Newtonsoft.Json;
using SharpCryptoExchange.Objects;
using SharpCryptoExchange.Okx.Converters;
using SharpCryptoExchange.Okx.Enums;
using SharpCryptoExchange.Okx.Helpers;
using SharpCryptoExchange.Okx.Objects.Core;
using SharpCryptoExchange.Okx.Objects.SubAccount;
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
        #region Sub-Account API Endpoints
        /// <summary>
        /// applies to master accounts only
        /// </summary>
        /// <param name="enable">Sub-account status，true: Normal ; false: Frozen</param>
        /// <param name="subAccountName">Sub Account Name</param>
        /// <param name="after">Pagination of data to return records earlier than the requested ts, Unix timestamp format in milliseconds, e.g. 1597026383085</param>
        /// <param name="before">Pagination of data to return records newer than the requested ts, Unix timestamp format in milliseconds, e.g. 1597026383085</param>
        /// <param name="limit">Number of results per request. The maximum is 100; the default is 100.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<IEnumerable<OkxSubAccount>> GetSubAccounts(
            bool? enable = null,
            string subAccountName = null,
            long? after = null,
            long? before = null,
            int limit = 100,
            CancellationToken ct = default)
            => GetSubAccountsAsync(enable, subAccountName, after, before, limit, ct).Result;
        /// <summary>
        /// applies to master accounts only
        /// </summary>
        /// <param name="enable">Sub-account status，true: Normal ; false: Frozen</param>
        /// <param name="subAccountName">Sub Account Name</param>
        /// <param name="after">Pagination of data to return records earlier than the requested ts, Unix timestamp format in milliseconds, e.g. 1597026383085</param>
        /// <param name="before">Pagination of data to return records newer than the requested ts, Unix timestamp format in milliseconds, e.g. 1597026383085</param>
        /// <param name="limit">Number of results per request. The maximum is 100; the default is 100.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<IEnumerable<OkxSubAccount>>> GetSubAccountsAsync(
            bool? enable = null,
            string subAccountName = null,
            long? after = null,
            long? before = null,
            int limit = 100,
            CancellationToken ct = default)
        {
            if (limit < 1 || limit > 100)
                throw new ArgumentException("Limit can be between 1-100.");

            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("enable", enable);
            parameters.AddOptionalParameter("subAcct", subAccountName);
            parameters.AddOptionalParameter("after", after?.ToString(OkxGlobals.OkxCultureInfo));
            parameters.AddOptionalParameter("before", before?.ToString(OkxGlobals.OkxCultureInfo));
            parameters.AddOptionalParameter("limit", limit.ToString(OkxGlobals.OkxCultureInfo));

            var result = await UnifiedApi.ExecuteAsync<OkxRestApiResponse<IEnumerable<OkxSubAccount>>>(UnifiedApi.GetUri(Endpoints_V5_SubAccount_List), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
            if (!result.Success) return result.AsError<IEnumerable<OkxSubAccount>>(new OkxRestApiError(result.Error.Code, result.Error.Message, result.Error.Data));
            if (result.Data.ErrorCode > 0) return result.AsError<IEnumerable<OkxSubAccount>>(new OkxRestApiError(result.Data.ErrorCode, result.Data.ErrorMessage, null));

            return result.As(result.Data.Data);
        }

        /// <summary>
        /// applies to master accounts only
        /// </summary>
        /// <param name="password">Funds password of the master account</param>
        /// <param name="subAccountName">Sub Account Name</param>
        /// <param name="apiKey">APIKey note</param>
        /// <param name="apiLabel">APIKey note</param>
        /// <param name="permission">APIKey access</param>
        /// <param name="ipAddresses">Link IP addresses, separate with commas if more than one. Support up to 20 IP addresses.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<OkxSubAccountApiKey> ResetSubAccountApiKey(
            string subAccountName,
            string apiKey,
            string apiLabel = null,
            bool? readPermission = null,
            bool? tradePermission = null,
            string ipAddresses = null,
            CancellationToken ct = default)
            => ResetSubAccountApiKeyAsync(subAccountName, apiKey, apiLabel, readPermission, tradePermission, ipAddresses, ct).Result;
        /// <summary>
        /// applies to master accounts only
        /// </summary>
        /// <param name="password">Funds password of the master account</param>
        /// <param name="subAccountName">Sub Account Name</param>
        /// <param name="apiKey">APIKey note</param>
        /// <param name="apiLabel">APIKey note</param>
        /// <param name="permission">APIKey access</param>
        /// <param name="ipAddresses">Link IP addresses, separate with commas if more than one. Support up to 20 IP addresses.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<OkxSubAccountApiKey>> ResetSubAccountApiKeyAsync(
            string subAccountName,
            string apiKey,
            string apiLabel = null,
            bool? readPermission = null,
            bool? tradePermission = null,
            string ipAddresses = null,
            CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>
            {
                { "subAcct", subAccountName },
                { "apiKey", apiKey},
                { "label", apiLabel },
            };
            if (!string.IsNullOrEmpty(ipAddresses))
                parameters.AddOptionalParameter("ip", ipAddresses);

            var permissions = new List<string>();
            if (readPermission.HasValue && readPermission.Value) permissions.Add("read_only");
            if (tradePermission.HasValue && tradePermission.Value) permissions.Add("trade");
            if (permissions.Count > 0)
                parameters.AddOptionalParameter("perm", string.Join(",", permissions));

            var result = await UnifiedApi.ExecuteAsync<OkxRestApiResponse<IEnumerable<OkxSubAccountApiKey>>>(UnifiedApi.GetUri(Endpoints_V5_SubAccount_ResetApiKey), HttpMethod.Post, ct, parameters, true).ConfigureAwait(false);
            if (!result.Success) return result.AsError<OkxSubAccountApiKey>(new OkxRestApiError(result.Error.Code, result.Error.Message, result.Error.Data));
            if (result.Data.ErrorCode > 0) return result.AsError<OkxSubAccountApiKey>(new OkxRestApiError(result.Data.ErrorCode, result.Data.ErrorMessage, null));

            return result.As(result.Data.Data.FirstOrDefault());
        }

        /// <summary>
        /// Query detailed balance info of Trading Account of a sub-account via the master account (applies to master accounts only)
        /// </summary>
        /// <param name="subAccountName">Sub Account Name</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<OkxSubAccountTradingBalance> GetSubAccountTradingBalances(
            string subAccountName,
            CancellationToken ct = default)
            => GetSubAccountTradingBalancesAsync(subAccountName, ct).Result;
        /// <summary>
        /// Query detailed balance info of Trading Account of a sub-account via the master account (applies to master accounts only)
        /// </summary>
        /// <param name="subAccountName">Sub Account Name</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<OkxSubAccountTradingBalance>> GetSubAccountTradingBalancesAsync(
            string subAccountName,
            CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>
            {
                {"subAcct", subAccountName },
            };

            var result = await UnifiedApi.ExecuteAsync<OkxRestApiResponse<IEnumerable<OkxSubAccountTradingBalance>>>(UnifiedApi.GetUri(Endpoints_V5_SubAccount_TradingBalances), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
            if (!result.Success) return result.AsError<OkxSubAccountTradingBalance>(new OkxRestApiError(result.Error.Code, result.Error.Message, result.Error.Data));
            if (result.Data.ErrorCode > 0) return result.AsError<OkxSubAccountTradingBalance>(new OkxRestApiError(result.Data.ErrorCode, result.Data.ErrorMessage, null));

            return result.As(result.Data.Data.FirstOrDefault());
        }

        /// <summary>
        /// Get sub-account funding balance
        /// Query detailed balance info of Funding Account of a sub-account via the master account (applies to master accounts only)
        /// </summary>
        /// <param name="subAccountName">Sub Account Name</param>
        /// <param name="currency">Single currency or multiple currencies (no more than 20) separated with comma, e.g. BTC or BTC,ETH.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<OkxSubAccountFundingBalance> GetSubAccountFundingBalances(
            string subAccountName,
            string currency = null,
            CancellationToken ct = default)
            => GetSubAccountFundingBalancesAsync(subAccountName, currency, ct).Result;
        /// <summary>
        /// Get sub-account funding balance
        /// Query detailed balance info of Funding Account of a sub-account via the master account (applies to master accounts only)
        /// </summary>
        /// <param name="subAccountName">Sub Account Name</param>
        /// <param name="currency">Single currency or multiple currencies (no more than 20) separated with comma, e.g. BTC or BTC,ETH.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<OkxSubAccountFundingBalance>> GetSubAccountFundingBalancesAsync(
            string subAccountName,
            string currency = null,
            CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>
            {
                {"subAcct", subAccountName },
            };

            if (!string.IsNullOrEmpty(currency))
                parameters.AddOptionalParameter("ccy", currency);

            var result = await UnifiedApi.ExecuteAsync<OkxRestApiResponse<IEnumerable<OkxSubAccountFundingBalance>>>(UnifiedApi.GetUri(Endpoints_V5_SubAccount_FundingBalances), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
            if (!result.Success) return result.AsError<OkxSubAccountFundingBalance>(new OkxRestApiError(result.Error.Code, result.Error.Message, result.Error.Data));
            if (result.Data.ErrorCode > 0) return result.AsError<OkxSubAccountFundingBalance>(new OkxRestApiError(result.Data.ErrorCode, result.Data.ErrorMessage, null));

            return result.As(result.Data.Data.FirstOrDefault());
        }

        /// <summary>
        /// applies to master accounts only
        /// </summary>
        /// <param name="subAccountName">Sub Account Name</param>
        /// <param name="currency">Currency</param>
        /// <param name="type">0: Transfers from master account to sub-account ;1 : Transfers from sub-account to master account.</param>
        /// <param name="after">Pagination of data to return records earlier than the requested ts, Unix timestamp format in milliseconds, e.g. 1597026383085</param>
        /// <param name="before">Pagination of data to return records newer than the requested ts, Unix timestamp format in milliseconds, e.g. 1597026383085</param>
        /// <param name="limit">Number of results per request. The maximum is 100; the default is 100.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<IEnumerable<OkxSubAccountBill>> GetSubAccountBills(
            string subAccountName = null,
            string currency = null,
            OkxSubAccountTransferType? type = null,
            long? after = null,
            long? before = null,
            int limit = 100,
            CancellationToken ct = default)
            => GetSubAccountBillsAsync(subAccountName, currency, type, after, before, limit, ct).Result;
        /// <summary>
        /// applies to master accounts only
        /// </summary>
        /// <param name="subAccountName">Sub Account Name</param>
        /// <param name="currency">Currency</param>
        /// <param name="type">0: Transfers from master account to sub-account ;1 : Transfers from sub-account to master account.</param>
        /// <param name="after">Pagination of data to return records earlier than the requested ts, Unix timestamp format in milliseconds, e.g. 1597026383085</param>
        /// <param name="before">Pagination of data to return records newer than the requested ts, Unix timestamp format in milliseconds, e.g. 1597026383085</param>
        /// <param name="limit">Number of results per request. The maximum is 100; the default is 100.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<IEnumerable<OkxSubAccountBill>>> GetSubAccountBillsAsync(
            string subAccountName = null,
            string currency = null,
            OkxSubAccountTransferType? type = null,
            long? after = null,
            long? before = null,
            int limit = 100,
            CancellationToken ct = default)
        {
            if (limit < 1 || limit > 100)
                throw new ArgumentException("Limit can be between 1-100.");

            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("subAcct", subAccountName);
            parameters.AddOptionalParameter("ccy", currency);
            parameters.AddOptionalParameter("type", JsonConvert.SerializeObject(type, new SubAccountTransferTypeConverter(false)));
            parameters.AddOptionalParameter("after", after?.ToString(OkxGlobals.OkxCultureInfo));
            parameters.AddOptionalParameter("before", before?.ToString(OkxGlobals.OkxCultureInfo));
            parameters.AddOptionalParameter("limit", limit.ToString(OkxGlobals.OkxCultureInfo));


            var result = await UnifiedApi.ExecuteAsync<OkxRestApiResponse<IEnumerable<OkxSubAccountBill>>>(UnifiedApi.GetUri(Endpoints_V5_SubAccount_Bills), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
            if (!result.Success) return result.AsError<IEnumerable<OkxSubAccountBill>>(new OkxRestApiError(result.Error.Code, result.Error.Message, result.Error.Data));
            if (result.Data.ErrorCode > 0) return result.AsError<IEnumerable<OkxSubAccountBill>>(new OkxRestApiError(result.Data.ErrorCode, result.Data.ErrorMessage, null));

            return result.As(result.Data.Data);
        }

        /// <summary>
        /// applies to master accounts only
        /// </summary>
        /// <param name="currency">Currency</param>
        /// <param name="amount">Amount</param>
        /// <param name="fromAccount">6:Funding Account 18:Unified Account</param>
        /// <param name="toAccount">6:Funding Account 18:Unified Account</param>
        /// <param name="fromSubAccountName">Sub-account name of the account that transfers funds out.</param>
        /// <param name="toSubAccountName">Sub-account name of the account that transfers funds in.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<OkxSubAccountTransfer> TransferBetweenSubAccounts(
            string currency,
            decimal amount,
            OkxAccount fromAccount,
            OkxAccount toAccount,
            string fromSubAccountName,
            string toSubAccountName,
            CancellationToken ct = default)
            => TransferBetweenSubAccountsAsync(currency, amount, fromAccount, toAccount, fromSubAccountName, toSubAccountName, ct).Result;
        /// <summary>
        /// applies to master accounts only
        /// </summary>
        /// <param name="currency">Currency</param>
        /// <param name="amount">Amount</param>
        /// <param name="fromAccount">6:Funding Account 18:Unified Account</param>
        /// <param name="toAccount">6:Funding Account 18:Unified Account</param>
        /// <param name="fromSubAccountName">Sub-account name of the account that transfers funds out.</param>
        /// <param name="toSubAccountName">Sub-account name of the account that transfers funds in.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<OkxSubAccountTransfer>> TransferBetweenSubAccountsAsync(
            string currency,
            decimal amount,
            OkxAccount fromAccount,
            OkxAccount toAccount,
            string fromSubAccountName,
            string toSubAccountName,
            CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>
            {
                {"ccy", currency },
                {"amt", amount.ToString(OkxGlobals.OkxCultureInfo) },
                {"from", JsonConvert.SerializeObject(fromAccount, new AccountConverter(false)) },
                {"to", JsonConvert.SerializeObject(toAccount, new AccountConverter(false)) },
                {"fromSubAccount", fromSubAccountName },
                {"toSubAccount", toSubAccountName },
            };

            var result = await UnifiedApi.ExecuteAsync<OkxRestApiResponse<IEnumerable<OkxSubAccountTransfer>>>(UnifiedApi.GetUri(Endpoints_V5_SubAccount_Transfer), HttpMethod.Post, ct, parameters, true).ConfigureAwait(false);
            if (!result.Success) return result.AsError<OkxSubAccountTransfer>(new OkxRestApiError(result.Error.Code, result.Error.Message, result.Error.Data));
            if (result.Data.ErrorCode > 0) return result.AsError<OkxSubAccountTransfer>(new OkxRestApiError(result.Data.ErrorCode, result.Data.ErrorMessage, null));

            return result.As(result.Data.Data.FirstOrDefault());
        }

        // TODO: Set Permission Of Transfer Out
        // TODO: Get custody trading sub-account list
        #endregion
    }
}