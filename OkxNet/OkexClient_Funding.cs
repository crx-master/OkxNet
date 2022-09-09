using CryptoExchangeNet;
using CryptoExchangeNet.Objects;
using Newtonsoft.Json;
using OkxNet.Converters;
using OkxNet.Enums;
using OkxNet.Helpers;
using OkxNet.Objects.Core;
using OkxNet.Objects.Funding;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace OkxNet
{
    public partial class OkxClient
    {
        #region Funding API Endpoints
        /// <summary>
        /// Retrieve a list of all currencies. Not all currencies can be traded. Currencies that have not been defined in ISO 4217 may use a custom symbol.
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<IEnumerable<OkxCurrency>> GetCurrencies(CancellationToken ct = default)
            => GetCurrenciesAsync(ct).Result;
        /// <summary>
        /// Retrieve a list of all currencies. Not all currencies can be traded. Currencies that have not been defined in ISO 4217 may use a custom symbol.
        /// </summary>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<IEnumerable<OkxCurrency>>> GetCurrenciesAsync(CancellationToken ct = default)
        {
            var result = await UnifiedApi.ExecuteAsync<OkxRestApiResponse<IEnumerable<OkxCurrency>>>(UnifiedApi.GetUri(Endpoints_V5_Asset_Currencies), HttpMethod.Get, ct, null, true).ConfigureAwait(false);
            if (!result.Success) return result.AsError<IEnumerable<OkxCurrency>>(new OkxRestApiError(result.Error.Code, result.Error.Message, result.Error.Data));
            if (result.Data.ErrorCode > 0) return result.AsError<IEnumerable<OkxCurrency>>(new OkxRestApiError(result.Data.ErrorCode, result.Data.ErrorMessage, null));

            return result.As(result.Data.Data);
        }

        /// <summary>
        /// Retrieve the balances of all the assets, and the amount that is available or on hold.
        /// </summary>
        /// <param name="currency">Currency</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<IEnumerable<OkxFundingBalance>> GetFundingBalance(string currency = null, CancellationToken ct = default)
            => GetFundingBalanceAsync(currency, ct).Result;
        /// <summary>
        /// Retrieve the balances of all the assets, and the amount that is available or on hold.
        /// </summary>
        /// <param name="currency">Currency</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<IEnumerable<OkxFundingBalance>>> GetFundingBalanceAsync(string currency = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("ccy", currency);

            var result = await UnifiedApi.ExecuteAsync<OkxRestApiResponse<IEnumerable<OkxFundingBalance>>>(UnifiedApi.GetUri(Endpoints_V5_Asset_Balances), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
            if (!result.Success) return result.AsError<IEnumerable<OkxFundingBalance>>(new OkxRestApiError(result.Error.Code, result.Error.Message, result.Error.Data));
            if (result.Data.ErrorCode > 0) return result.AsError<IEnumerable<OkxFundingBalance>>(new OkxRestApiError(result.Data.ErrorCode, result.Data.ErrorMessage, null));

            return result.As(result.Data.Data);
        }

        // TODO: Get account asset valuation

        /// <summary>
        /// This endpoint supports the transfer of funds between your funding account and trading account, and from the master account to sub-accounts. Direct transfers between sub-accounts are not allowed.
        /// </summary>
        /// <param name="currency">Currency</param>
        /// <param name="amount">Amount</param>
        /// <param name="type">Transfer type</param>
        /// <param name="fromAccount">The remitting account</param>
        /// <param name="toAccount">The beneficiary account</param>
        /// <param name="subAccountName">Sub Account Name</param>
        /// <param name="fromInstrumentId">MARGIN trading pair (e.g. BTC-USDT) or contract underlying (e.g. BTC-USD) to be transferred out.</param>
        /// <param name="toInstrumentId">MARGIN trading pair (e.g. BTC-USDT) or contract underlying (e.g. BTC-USD) to be transferred in.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<OkxTransferResponse> FundTransfer(
            string currency,
            decimal amount,
            OkxTransferType type,
            OkxAccount fromAccount,
            OkxAccount toAccount,
            string subAccountName = null,
            string fromInstrumentId = null,
            string toInstrumentId = null,
            CancellationToken ct = default)
            => FundTransferAsync(
            currency,
            amount,
            type,
            fromAccount,
            toAccount,
            subAccountName,
            fromInstrumentId,
            toInstrumentId,
            ct).Result;
        /// <summary>
        /// This endpoint supports the transfer of funds between your funding account and trading account, and from the master account to sub-accounts. Direct transfers between sub-accounts are not allowed.
        /// </summary>
        /// <param name="currency">Currency</param>
        /// <param name="amount">Amount</param>
        /// <param name="type">Transfer type</param>
        /// <param name="fromAccount">The remitting account</param>
        /// <param name="toAccount">The beneficiary account</param>
        /// <param name="subAccountName">Sub Account Name</param>
        /// <param name="fromInstrumentId">MARGIN trading pair (e.g. BTC-USDT) or contract underlying (e.g. BTC-USD) to be transferred out.</param>
        /// <param name="toInstrumentId">MARGIN trading pair (e.g. BTC-USDT) or contract underlying (e.g. BTC-USD) to be transferred in.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<OkxTransferResponse>> FundTransferAsync(
            string currency,
            decimal amount,
            OkxTransferType type,
            OkxAccount fromAccount,
            OkxAccount toAccount,
            string subAccountName = null,
            string fromInstrumentId = null,
            string toInstrumentId = null,
            CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object> {
                { "ccy",currency},
                { "amt",amount.ToString(OkxGlobals.OkxCultureInfo)},
                { "type", JsonConvert.SerializeObject(type, new TransferTypeConverter(false)) },
                { "from", JsonConvert.SerializeObject(fromAccount, new AccountConverter(false)) },
                { "to", JsonConvert.SerializeObject(toAccount, new AccountConverter(false)) },
            };
            parameters.AddOptionalParameter("subAcct", subAccountName);
            parameters.AddOptionalParameter("instId", fromInstrumentId);
            parameters.AddOptionalParameter("toInstId", toInstrumentId);

            var result = await UnifiedApi.ExecuteAsync<OkxRestApiResponse<IEnumerable<OkxTransferResponse>>>(UnifiedApi.GetUri(Endpoints_V5_Asset_Transfer), HttpMethod.Post, ct, parameters, true).ConfigureAwait(false);
            if (!result.Success) return result.AsError<OkxTransferResponse>(new OkxRestApiError(result.Error.Code, result.Error.Message, result.Error.Data));
            if (result.Data.ErrorCode > 0) return result.AsError<OkxTransferResponse>(new OkxRestApiError(result.Data.ErrorCode, result.Data.ErrorMessage, null));

            return result.As(result.Data.Data.FirstOrDefault());
        }

        // TODO: Get funds transfer state

        /// <summary>
        /// Query the billing record, you can get the latest 1 month historical data
        /// </summary>
        /// <param name="currency">Currency</param>
        /// <param name="type">Bill type</param>
        /// <param name="after">Pagination of data to return records earlier than the requested ts, Unix timestamp format in milliseconds, e.g. 1597026383085</param>
        /// <param name="before">Pagination of data to return records newer than the requested ts, Unix timestamp format in milliseconds, e.g. 1597026383085</param>
        /// <param name="limit">Number of results per request. The maximum is 100; the default is 100.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<IEnumerable<OkxFundingBill>> GetFundingBillDetails(
            string currency = null,
            OkxFundingBillType? type = null,
            long? after = null,
            long? before = null,
            int limit = 100,
            CancellationToken ct = default)
            => GetFundingBillDetailsAsync(
            currency,
            type,
            after,
            before,
            limit,
            ct).Result;
        /// <summary>
        /// Query the billing record, you can get the latest 1 month historical data
        /// </summary>
        /// <param name="currency">Currency</param>
        /// <param name="type">Bill type</param>
        /// <param name="after">Pagination of data to return records earlier than the requested ts, Unix timestamp format in milliseconds, e.g. 1597026383085</param>
        /// <param name="before">Pagination of data to return records newer than the requested ts, Unix timestamp format in milliseconds, e.g. 1597026383085</param>
        /// <param name="limit">Number of results per request. The maximum is 100; the default is 100.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<IEnumerable<OkxFundingBill>>> GetFundingBillDetailsAsync(
            string currency = null,
            OkxFundingBillType? type = null,
            long? after = null,
            long? before = null,
            int limit = 100,
            CancellationToken ct = default)
        {
            if (limit < 1 || limit > 100)
                throw new ArgumentException("Limit can be between 1-100.");

            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("ccy", currency);
            if (type.HasValue)
                parameters.AddOptionalParameter("type", JsonConvert.SerializeObject(type, new FundingBillTypeConverter(false)));
            parameters.AddOptionalParameter("after", after?.ToString(OkxGlobals.OkxCultureInfo));
            parameters.AddOptionalParameter("before", before?.ToString(OkxGlobals.OkxCultureInfo));
            parameters.AddOptionalParameter("limit", limit.ToString(OkxGlobals.OkxCultureInfo));

            var result = await UnifiedApi.ExecuteAsync<OkxRestApiResponse<IEnumerable<OkxFundingBill>>>(UnifiedApi.GetUri(Endpoints_V5_Asset_Bills), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
            if (!result.Success) return result.AsError<IEnumerable<OkxFundingBill>>(new OkxRestApiError(result.Error.Code, result.Error.Message, result.Error.Data));
            if (result.Data.ErrorCode > 0) return result.AsError<IEnumerable<OkxFundingBill>>(new OkxRestApiError(result.Data.ErrorCode, result.Data.ErrorMessage, null));

            return result.As(result.Data.Data);
        }

        /// <summary>
        /// Users can create up to 10,000 different invoices within 24 hours.
        /// </summary>
        /// <param name="currency">Currency</param>
        /// <param name="amount">deposit amount between 0.000001 - 0.1</param>
        /// <param name="account">Receiving account</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<IEnumerable<OkxLightningDeposit>> GetLightningDeposits(
            string currency,
            decimal amount,
            OkxLightningDepositAccount? account = null,
            CancellationToken ct = default)
            => GetLightningDepositsAsync(currency, amount, account, ct).Result;
        /// <summary>
        /// Users can create up to 10,000 different invoices within 24 hours.
        /// </summary>
        /// <param name="currency">Currency</param>
        /// <param name="amount">deposit amount between 0.000001 - 0.1</param>
        /// <param name="account">Receiving account</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<IEnumerable<OkxLightningDeposit>>> GetLightningDepositsAsync(
            string currency,
            decimal amount,
            OkxLightningDepositAccount? account = null,
            CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>
            {
                { "ccy", currency },
                { "amt", amount.ToString(OkxGlobals.OkxCultureInfo) },
            };
            if (account.HasValue)
                parameters.AddOptionalParameter("to", JsonConvert.SerializeObject(account, new LightningDepositAccountConverter(false)));

            var result = await UnifiedApi.ExecuteAsync<OkxRestApiResponse<IEnumerable<OkxLightningDeposit>>>(UnifiedApi.GetUri(Endpoints_V5_Asset_DepositLightning), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
            if (!result.Success) return result.AsError<IEnumerable<OkxLightningDeposit>>(new OkxRestApiError(result.Error.Code, result.Error.Message, result.Error.Data));
            if (result.Data.ErrorCode > 0) return result.AsError<IEnumerable<OkxLightningDeposit>>(new OkxRestApiError(result.Data.ErrorCode, result.Data.ErrorMessage, null));

            return result.As(result.Data.Data);
        }

        /// <summary>
        /// Retrieve the deposit addresses of currencies, including previously-used addresses.
        /// </summary>
        /// <param name="currency">Currency</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<IEnumerable<OkxDepositAddress>> GetDepositAddress(string currency = null, CancellationToken ct = default)
            => GetDepositAddressAsync(currency, ct).Result;
        /// <summary>
        /// Retrieve the deposit addresses of currencies, including previously-used addresses.
        /// </summary>
        /// <param name="currency">Currency</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<IEnumerable<OkxDepositAddress>>> GetDepositAddressAsync(string currency = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object> {
                { "ccy", currency },
            };

            var result = await UnifiedApi.ExecuteAsync<OkxRestApiResponse<IEnumerable<OkxDepositAddress>>>(UnifiedApi.GetUri(Endpoints_V5_Asset_DepositAddress), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
            if (!result.Success) return result.AsError<IEnumerable<OkxDepositAddress>>(new OkxRestApiError(result.Error.Code, result.Error.Message, result.Error.Data));
            if (result.Data.ErrorCode > 0) return result.AsError<IEnumerable<OkxDepositAddress>>(new OkxRestApiError(result.Data.ErrorCode, result.Data.ErrorMessage, null));

            return result.As(result.Data.Data);
        }

        /// <summary>
        /// Retrieve the deposit history of all currencies, up to 100 recent records in a year.
        /// </summary>
        /// <param name="currency">Currency</param>
        /// <param name="transactionId">Transaction ID</param>
        /// <param name="state">State</param>
        /// <param name="after">Pagination of data to return records earlier than the requested ts, Unix timestamp format in milliseconds, e.g. 1597026383085</param>
        /// <param name="before">Pagination of data to return records newer than the requested ts, Unix timestamp format in milliseconds, e.g. 1597026383085</param>
        /// <param name="limit">Number of results per request. The maximum is 100; the default is 100.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<IEnumerable<OkxDepositHistory>> GetDepositHistory(
            string currency = null,
            string transactionId = null,
            OkxDepositState? state = null,
            long? after = null,
            long? before = null,
            int limit = 100,
            CancellationToken ct = default)
            => GetDepositHistoryAsync(currency, transactionId, state, after, before, limit, ct).Result;
        /// <summary>
        /// Retrieve the deposit history of all currencies, up to 100 recent records in a year.
        /// </summary>
        /// <param name="currency">Currency</param>
        /// <param name="transactionId">Transaction ID</param>
        /// <param name="state">State</param>
        /// <param name="after">Pagination of data to return records earlier than the requested ts, Unix timestamp format in milliseconds, e.g. 1597026383085</param>
        /// <param name="before">Pagination of data to return records newer than the requested ts, Unix timestamp format in milliseconds, e.g. 1597026383085</param>
        /// <param name="limit">Number of results per request. The maximum is 100; the default is 100.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<IEnumerable<OkxDepositHistory>>> GetDepositHistoryAsync(
            string currency = null,
            string transactionId = null,
            OkxDepositState? state = null,
            long? after = null,
            long? before = null,
            int limit = 100,
            CancellationToken ct = default)
        {
            if (limit < 1 || limit > 100)
                throw new ArgumentException("Limit can be between 1-100.");

            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("ccy", currency);
            parameters.AddOptionalParameter("txId", transactionId);
            if (state.HasValue)
                parameters.AddOptionalParameter("state", JsonConvert.SerializeObject(state, new DepositStateConverter(false)));
            parameters.AddOptionalParameter("after", after?.ToString(OkxGlobals.OkxCultureInfo));
            parameters.AddOptionalParameter("before", before?.ToString(OkxGlobals.OkxCultureInfo));
            parameters.AddOptionalParameter("limit", limit.ToString(OkxGlobals.OkxCultureInfo));

            var result = await UnifiedApi.ExecuteAsync<OkxRestApiResponse<IEnumerable<OkxDepositHistory>>>(UnifiedApi.GetUri(Endpoints_V5_Asset_DepositHistory), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
            if (!result.Success) return result.AsError<IEnumerable<OkxDepositHistory>>(new OkxRestApiError(result.Error.Code, result.Error.Message, result.Error.Data));
            if (result.Data.ErrorCode > 0) return result.AsError<IEnumerable<OkxDepositHistory>>(new OkxRestApiError(result.Data.ErrorCode, result.Data.ErrorMessage, null));

            return result.As(result.Data.Data);
        }

        /// <summary>
        /// Withdrawal of tokens.
        /// </summary>
        /// <param name="currency">Currency</param>
        /// <param name="amount">Amount</param>
        /// <param name="destination">Withdrawal destination address</param>
        /// <param name="toAddress">Verified digital currency address, email or mobile number. Some digital currency addresses are formatted as 'address+tag', e.g. 'ARDOR-7JF3-8F2E-QUWZ-CAN7F:123456'</param>
        /// <param name="password">Trade password</param>
        /// <param name="fee">Transaction fee</param>
        /// <param name="chain">Chain name. There are multiple chains under some currencies, such as USDT has USDT-ERC20, USDT-TRC20, and USDT-Omni. If this parameter is not filled in because it is not available, it will default to the main chain.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<OkxWithdrawalResponse> Withdraw(
            string currency,
            decimal amount,
            OkxWithdrawalDestination destination,
            string toAddress,
            string password,
            decimal fee,
            string chain = null,
            CancellationToken ct = default)
            => WithdrawAsync(
            currency,
            amount,
            destination,
            toAddress,
            password,
            fee,
            chain,
            ct).Result;
        /// <summary>
        /// Withdrawal of tokens.
        /// </summary>
        /// <param name="currency">Currency</param>
        /// <param name="amount">Amount</param>
        /// <param name="destination">Withdrawal destination address</param>
        /// <param name="toAddress">Verified digital currency address, email or mobile number. Some digital currency addresses are formatted as 'address+tag', e.g. 'ARDOR-7JF3-8F2E-QUWZ-CAN7F:123456'</param>
        /// <param name="password">Trade password</param>
        /// <param name="fee">Transaction fee</param>
        /// <param name="chain">Chain name. There are multiple chains under some currencies, such as USDT has USDT-ERC20, USDT-TRC20, and USDT-Omni. If this parameter is not filled in because it is not available, it will default to the main chain.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<OkxWithdrawalResponse>> WithdrawAsync(
            string currency,
            decimal amount,
            OkxWithdrawalDestination destination,
            string toAddress,
            string password,
            decimal fee,
            string chain = null,
            CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object> {
                { "ccy",currency},
                { "amt",amount.ToString(OkxGlobals.OkxCultureInfo)},
                { "dest", JsonConvert.SerializeObject(destination, new WithdrawalDestinationConverter(false)) },
                { "toAddr",toAddress},
                { "pwd",password},
                { "fee",fee   .ToString(OkxGlobals.OkxCultureInfo)},
            };
            parameters.AddOptionalParameter("chain", chain);

            var result = await UnifiedApi.ExecuteAsync<OkxRestApiResponse<IEnumerable<OkxWithdrawalResponse>>>(UnifiedApi.GetUri(Endpoints_V5_Asset_Withdrawal), HttpMethod.Post, ct, parameters, true).ConfigureAwait(false);
            if (!result.Success) return result.AsError<OkxWithdrawalResponse>(new OkxRestApiError(result.Error.Code, result.Error.Message, result.Error.Data));
            if (result.Data.ErrorCode > 0) return result.AsError<OkxWithdrawalResponse>(new OkxRestApiError(result.Data.ErrorCode, result.Data.ErrorMessage, null));

            return result.As(result.Data.Data.FirstOrDefault());
        }

        /// <summary>
        /// The maximum withdrawal amount is 0.1 BTC per request, and 1 BTC in 24 hours. The minimum withdrawal amount is approximately 0.000001 BTC. Sub-account does not support withdrawal.
        /// </summary>
        /// <param name="currency">Token symbol. Currently only BTC is supported.</param>
        /// <param name="invoice">Invoice text</param>
        /// <param name="memo">Lightning withdrawal memo</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<OkxLightningWithdrawal> GetLightningWithdrawals(
            string currency,
            string invoice,
            string memo = null,
            CancellationToken ct = default)
            => GetLightningWithdrawalsAsync(currency, invoice, memo, ct).Result;
        /// <summary>
        /// The maximum withdrawal amount is 0.1 BTC per request, and 1 BTC in 24 hours. The minimum withdrawal amount is approximately 0.000001 BTC. Sub-account does not support withdrawal.
        /// </summary>
        /// <param name="currency">Token symbol. Currently only BTC is supported.</param>
        /// <param name="invoice">Invoice text</param>
        /// <param name="memo">Lightning withdrawal memo</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<OkxLightningWithdrawal>> GetLightningWithdrawalsAsync(
            string currency,
            string invoice,
            string memo = null,
            CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>
            {
                { "ccy", currency },
                { "invoice", invoice },
            };
            if (!string.IsNullOrEmpty(memo))
                parameters.AddOptionalParameter("memo", memo);

            var result = await UnifiedApi.ExecuteAsync<OkxRestApiResponse<IEnumerable<OkxLightningWithdrawal>>>(UnifiedApi.GetUri(Endpoints_V5_Asset_WithdrawalLightning), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
            if (!result.Success) return result.AsError<OkxLightningWithdrawal>(new OkxRestApiError(result.Error.Code, result.Error.Message, result.Error.Data));
            if (result.Data.ErrorCode > 0) return result.AsError<OkxLightningWithdrawal>(new OkxRestApiError(result.Data.ErrorCode, result.Data.ErrorMessage, null));

            return result.As(result.Data.Data.FirstOrDefault());
        }

        /// <summary>
        /// Cancel withdrawal
        /// You can cancel normal withdrawal requests, but you cannot cancel withdrawal requests on Lightning.
        /// Rate Limit: 6 requests per second
        /// </summary>
        /// <param name="withdrawalId">Withdrawal ID</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<OkxWithdrawalId> CancelWithdrawal(string withdrawalId, CancellationToken ct = default)
            => CancelWithdrawalAsync( withdrawalId, ct).Result;

        /// <summary>
        /// Cancel withdrawal
        /// You can cancel normal withdrawal requests, but you cannot cancel withdrawal requests on Lightning.
        /// Rate Limit: 6 requests per second
        /// </summary>
        /// <param name="withdrawalId">Withdrawal ID</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<OkxWithdrawalId>> CancelWithdrawalAsync(string withdrawalId, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object> {
                { "wdId",withdrawalId},
            };

            var result = await UnifiedApi.ExecuteAsync<OkxRestApiResponse<IEnumerable<OkxWithdrawalId>>>(UnifiedApi.GetUri(Endpoints_V5_Asset_WithdrawalCancel), HttpMethod.Post, ct, parameters, true).ConfigureAwait(false);
            if (!result.Success) return result.AsError<OkxWithdrawalId>(new OkxRestApiError(result.Error.Code, result.Error.Message, result.Error.Data));
            if (result.Data.ErrorCode > 0) return result.AsError<OkxWithdrawalId>(new OkxRestApiError(result.Data.ErrorCode, result.Data.ErrorMessage, null));

            return result.As(result.Data.Data.FirstOrDefault());
        }

        /// <summary>
        /// Retrieve the withdrawal records according to the currency, withdrawal status, and time range in reverse chronological order. The 100 most recent records are returned by default.
        /// </summary>
        /// <param name="currency">Currency</param>
        /// <param name="transactionId">Transaction ID</param>
        /// <param name="state">State</param>
        /// <param name="after">Pagination of data to return records earlier than the requested ts, Unix timestamp format in milliseconds, e.g. 1597026383085</param>
        /// <param name="before">Pagination of data to return records newer than the requested ts, Unix timestamp format in milliseconds, e.g. 1597026383085</param>
        /// <param name="limit">Number of results per request. The maximum is 100; the default is 100.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<IEnumerable<OkxWithdrawalHistory>> GetWithdrawalHistory(
            string currency = null,
            string transactionId = null,
            OkxWithdrawalState? state = null,
            long? after = null,
            long? before = null,
            int limit = 100,
            CancellationToken ct = default)
            => GetWithdrawalHistoryAsync(currency, transactionId, state, after, before, limit, ct).Result;
        /// <summary>
        /// Retrieve the withdrawal records according to the currency, withdrawal status, and time range in reverse chronological order. The 100 most recent records are returned by default.
        /// </summary>
        /// <param name="currency">Currency</param>
        /// <param name="transactionId">Transaction ID</param>
        /// <param name="state">State</param>
        /// <param name="after">Pagination of data to return records earlier than the requested ts, Unix timestamp format in milliseconds, e.g. 1597026383085</param>
        /// <param name="before">Pagination of data to return records newer than the requested ts, Unix timestamp format in milliseconds, e.g. 1597026383085</param>
        /// <param name="limit">Number of results per request. The maximum is 100; the default is 100.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<IEnumerable<OkxWithdrawalHistory>>> GetWithdrawalHistoryAsync(
            string currency = null,
            string transactionId = null,
            OkxWithdrawalState? state = null,
            long? after = null,
            long? before = null,
            int limit = 100,
            CancellationToken ct = default)
        {
            if (limit < 1 || limit > 100)
                throw new ArgumentException("Limit can be between 1-100.");

            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("ccy", currency);
            parameters.AddOptionalParameter("txId", transactionId);
            if (state.HasValue)
                parameters.AddOptionalParameter("state", JsonConvert.SerializeObject(state, new WithdrawalStateConverter(false)));
            parameters.AddOptionalParameter("after", after?.ToString(OkxGlobals.OkxCultureInfo));
            parameters.AddOptionalParameter("before", before?.ToString(OkxGlobals.OkxCultureInfo));
            parameters.AddOptionalParameter("limit", limit.ToString(OkxGlobals.OkxCultureInfo));

            var result = await UnifiedApi.ExecuteAsync<OkxRestApiResponse<IEnumerable<OkxWithdrawalHistory>>>(UnifiedApi.GetUri(Endpoints_V5_Asset_WithdrawalHistory), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
            if (!result.Success) return result.AsError<IEnumerable<OkxWithdrawalHistory>>(new OkxRestApiError(result.Error.Code, result.Error.Message, result.Error.Data));
            if (result.Data.ErrorCode > 0) return result.AsError<IEnumerable<OkxWithdrawalHistory>>(new OkxRestApiError(result.Data.ErrorCode, result.Data.ErrorMessage, null));

            return result.As(result.Data.Data);
        }

        // TODO: Small assets convert

        /// <summary>
        /// Get saving balance
        /// Only the assets in the funding account can be used for saving.
        /// Rate Limit: 6 requests per second
        /// </summary>
        /// <param name="currency">Currency, e.g. BTC</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<IEnumerable<OkxSavingBalance>> GetSavingBalances(string currency = null, CancellationToken ct = default)
            => GetSavingBalancesAsync(currency, ct).Result;
        /// <summary>
        /// Get saving balance
        /// Only the assets in the funding account can be used for saving.
        /// Rate Limit: 6 requests per second
        /// </summary>
        /// <param name="currency">Currency, e.g. BTC</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<IEnumerable<OkxSavingBalance>>> GetSavingBalancesAsync(string currency = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("ccy", currency);

            var result = await UnifiedApi.ExecuteAsync<OkxRestApiResponse<IEnumerable<OkxSavingBalance>>>(UnifiedApi.GetUri(Endpoints_V5_Asset_SavingBalance), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
            if (!result.Success) return result.AsError<IEnumerable<OkxSavingBalance>>(new OkxRestApiError(result.Error.Code, result.Error.Message, result.Error.Data));
            if (result.Data.ErrorCode > 0) return result.AsError<IEnumerable<OkxSavingBalance>>(new OkxRestApiError(result.Data.ErrorCode, result.Data.ErrorMessage, null));

            return result.As(result.Data.Data);
        }

        public virtual WebCallResult<OkxSavingActionResponse> SavingPurchaseRedemption(
            string currency,
            decimal amount,
            OkxSavingActionSide side,
            decimal? rate = null,
            CancellationToken ct = default)
            => SavingPurchaseRedemptionAsync(
            currency,
            amount,
            side,
            rate,
            ct).Result;
        public virtual async Task<WebCallResult<OkxSavingActionResponse>> SavingPurchaseRedemptionAsync(
            string currency,
            decimal amount,
            OkxSavingActionSide side,
            decimal? rate = null,
            CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object> {
                { "ccy",currency},
                { "amt",amount.ToString(OkxGlobals.OkxCultureInfo)},
                { "side", JsonConvert.SerializeObject(side, new SavingActionSideConverter(false)) },
            };
            parameters.AddOptionalParameter("rate", rate?.ToString(OkxGlobals.OkxCultureInfo));

            var result = await UnifiedApi.ExecuteAsync<OkxRestApiResponse<IEnumerable<OkxSavingActionResponse>>>(UnifiedApi.GetUri(Endpoints_V5_Asset_SavingPurchaseRedempt), HttpMethod.Post, ct, parameters, true).ConfigureAwait(false);
            if (!result.Success) return result.AsError<OkxSavingActionResponse>(new OkxRestApiError(result.Error.Code, result.Error.Message, result.Error.Data));
            if (result.Data.ErrorCode > 0) return result.AsError<OkxSavingActionResponse>(new OkxRestApiError(result.Data.ErrorCode, result.Data.ErrorMessage, null));

            return result.As(result.Data.Data.FirstOrDefault());
        }

        // TODO: Set lending rate
        // TODO: Get lending history
        // TODO: Get public borrow info (public)
        // TODO: Get public borrow history (public)
        #endregion
    }
}