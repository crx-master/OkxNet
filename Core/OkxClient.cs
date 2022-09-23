using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SharpCryptoExchange.Authentication;
using SharpCryptoExchange.Interfaces;
using SharpCryptoExchange.Logging;
using SharpCryptoExchange.Objects;
using SharpCryptoExchange.Okx.Models.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SharpCryptoExchange.Okx
{
    public partial class OkxClient : BaseRestClient
    {
        internal class Endpoints
        {
            internal class V5
            {
                internal class Trade
                {
                    internal const string Order = "api/v5/trade/order";
                    internal const string BatchOrders = "api/v5/trade/batch-orders";
                    internal const string CancelOrder = "api/v5/trade/cancel-order";
                    internal const string CancelBatchOrders = "api/v5/trade/cancel-batch-orders";
                    internal const string AmendOrder = "api/v5/trade/amend-order";
                    internal const string AmendBatchOrders = "api/v5/trade/amend-batch-orders";
                    internal const string ClosePosition = "api/v5/trade/close-position";
                    internal const string OrdersPending = "api/v5/trade/orders-pending";
                    internal const string OrdersHistory = "api/v5/trade/orders-history";
                    internal const string OrdersHistoryArchive = "api/v5/trade/orders-history-archive";
                    internal const string Fills = "api/v5/trade/fills";
                    internal const string FillsHistory = "api/v5/trade/fills-history";
                    internal const string OrderAlgo = "api/v5/trade/order-algo";
                    internal const string CancelAlgos = "api/v5/trade/cancel-algos";
                    internal const string CancelAdvanceAlgos = "api/v5/trade/cancel-advance-algos";
                    internal const string OrdersAlgoPending = "api/v5/trade/orders-algo-pending";
                    internal const string OrdersAlgoHistory = "api/v5/trade/orders-algo-history";
                }

                internal class Asset
                {
                    internal const string Currencies = "api/v5/asset/currencies";
                    internal const string Balances = "api/v5/asset/balances";
                    internal const string Transfer = "api/v5/asset/transfer";
                    internal const string Bills = "api/v5/asset/bills";
                    internal const string DepositLightning = "api/v5/asset/deposit-lightning";
                    internal const string DepositAddress = "api/v5/asset/deposit-address";
                    internal const string DepositHistory = "api/v5/asset/deposit-history";
                    internal const string Withdrawal = "api/v5/asset/withdrawal";
                    internal const string WithdrawalLightning = "api/v5/asset/withdrawal-lightning";
                    internal const string WithdrawalCancel = "api/v5/asset/cancel-withdrawal";
                    internal const string WithdrawalHistory = "api/v5/asset/withdrawal-history";
                    internal const string SavingBalance = "api/v5/asset/saving-balance";
                    internal const string SavingPurchaseRedempt = "api/v5/asset/purchase_redempt";
                }

                internal class Account
                {
                    internal const string Balance = "api/v5/account/balance";
                    internal const string Positions = "api/v5/account/positions";
                    internal const string PositionRisk = "api/v5/account/account-position-risk";
                    internal const string Bills = "api/v5/account/bills";
                    internal const string BillsArchive = "api/v5/account/bills-archive";
                    internal const string Config = "api/v5/account/config";
                    internal const string SetPositionMode = "api/v5/account/set-position-mode";
                    internal const string SetLeverage = "api/v5/account/set-leverage";
                    internal const string MaxSize = "api/v5/account/max-size";
                    internal const string MaxAvailSize = "api/v5/account/max-avail-size";
                    internal const string PositionMarginBalance = "api/v5/account/position/margin-balance";
                    internal const string LeverageInfo = "api/v5/account/leverage-info";
                    internal const string MaxLoan = "api/v5/account/max-loan";
                    internal const string TradeFee = "api/v5/account/trade-fee";
                    internal const string InterestAccrued = "api/v5/account/interest-accrued";
                    internal const string InterestRate = "api/v5/account/interest-rate";
                    internal const string SetGreeks = "api/v5/account/set-greeks";
                    internal const string MaxWithdrawal = "api/v5/account/max-withdrawal";
                }

                internal class SubAccount
                {
                    internal const string List = "api/v5/users/subaccount/list";
                    internal const string ResetApiKey = "api/v5/users/subaccount/modify-apikey";
                    internal const string TradingBalances = "api/v5/account/subaccount/balances";
                    internal const string FundingBalances = "api/v5/asset/subaccount/balances";
                    internal const string Bills = "api/v5/asset/subaccount/bills";
                    internal const string Transfer = "api/v5/asset/subaccount/transfer";
                }
            }

        }

        #region Internal Fields
        internal OkxClientOptions Options { get; }
        internal OkxClientUnifiedApi UnifiedApi { get; }
        internal const string BodyParameterKey = "<BODY>";
        #endregion

        #region Rest Api Endpoints

        #region Market Data
        internal const string Endpoints_V5_Market_Tickers = "api/v5/market/tickers";
        internal const string Endpoints_V5_Market_Ticker = "api/v5/market/ticker";
        internal const string Endpoints_V5_Market_IndexTickers = "api/v5/market/index-tickers";
        internal const string Endpoints_V5_Market_Books = "api/v5/market/books";
        internal const string Endpoints_V5_Market_Candles = "api/v5/market/candles";
        internal const string Endpoints_V5_Market_HistoryCandles = "api/v5/market/history-candles";
        internal const string Endpoints_V5_Market_IndexCandles = "api/v5/market/index-candles";
        internal const string Endpoints_V5_Market_MarkPriceCandles = "api/v5/market/mark-price-candles";
        internal const string Endpoints_V5_Market_Trades = "api/v5/market/trades";
        internal const string Endpoints_V5_Market_TradesHistory = "api/v5/market/history-trades";
        internal const string Endpoints_V5_Market_Platform24Volume = "api/v5/market/platform-24-volume";
        internal const string Endpoints_V5_Market_OpenOracle = "api/v5/market/open-oracle";
        internal const string Endpoints_V5_Market_IndexComponents = "api/v5/market/index-components";
        internal const string Endpoints_V5_Market_BrickTickers = "api/v5/market/brick-tickers";
        internal const string Endpoints_V5_Market_BrickTicker = "api/v5/market/brick-ticker";
        internal const string Endpoints_V5_Market_BrickTrades = "api/v5/market/brick-trades";
        #endregion

        #region Public Data
        internal const string Endpoints_V5_Public_Instruments = "api/v5/public/instruments";
        internal const string Endpoints_V5_Public_DeliveryExerciseHistory = "api/v5/public/delivery-exercise-history";
        internal const string Endpoints_V5_Public_OpenInterest = "api/v5/public/open-interest";
        internal const string Endpoints_V5_Public_FundingRate = "api/v5/public/funding-rate";
        internal const string Endpoints_V5_Public_FundingRateHistory = "api/v5/public/funding-rate-history";
        internal const string Endpoints_V5_Public_PriceLimit = "api/v5/public/price-limit";
        internal const string Endpoints_V5_Public_OptionSummary = "api/v5/public/opt-summary";
        internal const string Endpoints_V5_Public_EstimatedPrice = "api/v5/public/estimated-price";
        internal const string Endpoints_V5_Public_DiscountRateInterestFreeQuota = "api/v5/public/discount-rate-interest-free-quota";
        internal const string Endpoints_V5_Public_Time = "api/v5/public/time";
        internal const string Endpoints_V5_Public_LiquidationOrders = "api/v5/public/liquidation-orders";
        internal const string Endpoints_V5_Public_MarkPrice = "api/v5/public/mark-price";
        internal const string Endpoints_V5_Public_PositionTiers = "api/v5/public/position-tiers";
        internal const string Endpoints_V5_Public_InterestRateLoanQuota = "api/v5/public/interest-rate-loan-quota";
        internal const string Endpoints_V5_Public_VIPInterestRateLoanQuota = "api/v5/public/vip-interest-rate-loan-quota";
        internal const string Endpoints_V5_Public_Underlying = "api/v5/public/underlying";
        internal const string Endpoints_V5_Public_InsuranceFund = "api/v5/public/insurance-fund";
        internal const string Endpoints_V5_Public_ConvertContractCoin = "api/v5/public/convert-contract-coin";
        #endregion

        #region Trading Data
        internal const string Endpoints_V5_RubikStat_TradingDataSupportCoin = "api/v5/rubik/stat/trading-data/support-coin";
        internal const string Endpoints_V5_RubikStat_TakerVolume = "api/v5/rubik/stat/taker-volume";
        internal const string Endpoints_V5_RubikStat_MarginLoanRatio = "api/v5/rubik/stat/margin/loan-ratio";
        internal const string Endpoints_V5_RubikStat_ContractsLongShortAccountRatio = "api/v5/rubik/stat/contracts/long-short-account-ratio";
        internal const string Endpoints_V5_RubikStat_ContractsOpenInterestVolume = "api/v5/rubik/stat/contracts/open-interest-volume";
        internal const string Endpoints_V5_RubikStat_OptionOpenInterestVolume = "api/v5/rubik/stat/option/open-interest-volume";
        internal const string Endpoints_V5_RubikStat_OptionOpenInterestVolumeRatio = "api/v5/rubik/stat/option/open-interest-volume-ratio";
        internal const string Endpoints_V5_RubikStat_OptionOpenInterestVolumeExpiry = "api/v5/rubik/stat/option/open-interest-volume-expiry";
        internal const string Endpoints_V5_RubikStat_OptionOpenInterestVolumeStrike = "api/v5/rubik/stat/option/open-interest-volume-strike";
        internal const string Endpoints_V5_RubikStat_OptionTakerBrickVolume = "api/v5/rubik/stat/option/taker-brick-volume";
        #endregion

        #region System
        internal const string Endpoints_V5_System_Status = "api/v5/system/status";
        #endregion

        #endregion

        #region Constructor/Destructor
        public OkxClient() : this(OkxClientOptions.Default)
        {
        }

        public OkxClient(OkxClientOptions options) : base("OKX REST API", options)
        {
            Options = options;
            UnifiedApi = AddApiClient(new OkxClientUnifiedApi(Log, this, options));
        }
        #endregion

        #region Common Methods
        /// <summary>
        /// Sets the default options to use for new clients
        /// </summary>
        /// <param name="options">The options to use for new clients</param>
        public static void SetDefaultOptions(OkxClientOptions options)
        {
            OkxClientOptions.Default = options;
        }

        /// <summary>
        /// Sets the API Credentials
        /// </summary>
        /// <param name="credentials">API Credentials Object</param>
        public void SetApiCredentials(OkxApiCredentials credentials)
        {
            ((OkxClientUnifiedApi)UnifiedApi).SetApiCredentials(credentials);
        }

        /// <summary>
        /// Sets the API Credentials
        /// </summary>
        /// <param name="apiKey">The api key</param>
        /// <param name="apiSecret">The api secret</param>
        /// <param name="passPhrase">The passphrase you specified when creating the API key</param>
        public virtual void SetApiCredentials(string apiKey, string apiSecret, string passPhrase)
        {
            ((OkxClientUnifiedApi)UnifiedApi).SetApiCredentials(new OkxApiCredentials(apiKey, apiSecret, passPhrase));
        }
        #endregion

        #region Core Methods
        protected override void WriteParamBody(BaseApiClient apiClient, IRequest request, SortedDictionary<string, object> parameters, string contentType)
        {
            OkxWriteParamBody(apiClient, request, parameters, contentType);
        }

        internal virtual void OkxWriteParamBody(BaseApiClient apiClient, IRequest request, SortedDictionary<string, object> parameters, string contentType)
        {
            if (apiClient.requestBodyFormat == RequestBodyFormat.Json)
            {
                if (parameters.Count == 1 && parameters.Keys.First() == BodyParameterKey)
                {
                    // Write the parameters as json in the body
                    var stringData = JsonConvert.SerializeObject(parameters[BodyParameterKey]);
                    request.SetContent(stringData, contentType);
                }
                else
                {
                    // Write the parameters as json in the body
                    var stringData = JsonConvert.SerializeObject(parameters);
                    request.SetContent(stringData, contentType);
                }
            }
            else if (apiClient.requestBodyFormat == RequestBodyFormat.FormData)
            {
                // Write the parameters as form data in the body
                var stringData = parameters.ToFormData();
                request.SetContent(stringData, contentType);
            }
        }

        protected override Error ParseErrorResponse(JToken error)
        {
            return OkxParseErrorResponse(error);
        }

        internal virtual Error OkxParseErrorResponse(JToken error)
        {
            if (error["code"] == null || error["msg"] == null)
                return new ServerError(error.ToString());

            return new ServerError((int)error["code"]!, (string)error["msg"]!);
        }

        internal async Task<WebCallResult> ExecuteAsync(RestApiClient apiClient, Uri uri, HttpMethod method, CancellationToken ct, Dictionary<string, object> parameters = null, bool signed = false, HttpMethodParameterPosition? parameterPosition = null)
        {
            var result = await SendRequestAsync<object>(apiClient, uri, method, ct, parameters, signed, parameterPosition).ConfigureAwait(false);
            if (!result) return result.AsDatalessError(result.Error!);

            return result.AsDataless();
        }

        internal async Task<WebCallResult<T>> ExecuteAsync<T>(RestApiClient apiClient, Uri uri, HttpMethod method, CancellationToken ct, Dictionary<string, object> parameters = null, bool signed = false, int weight = 1, bool ignoreRatelimit = false, HttpMethodParameterPosition? parameterPosition = null) where T : class
        {
            var result = await SendRequestAsync<T>(apiClient, uri, method, ct, parameters, signed, parameterPosition, requestWeight: weight, ignoreRatelimit: ignoreRatelimit).ConfigureAwait(false);
            if (!result) return result.AsError<T>(result.Error!);

            return result.As(result.Data);
        }
        #endregion

    }

    public class OkxClientUnifiedApi : RestApiClient
    {
        #region Internal Fields
        internal readonly Log _log;
        internal readonly OkxClient _baseClient;
        internal readonly OkxClientOptions _options;
        internal static TimeSyncState TimeSyncState = new TimeSyncState("Unified Api");
        #endregion

        internal OkxClientUnifiedApi(Log log, OkxClient baseClient, OkxClientOptions options) : base(options, options.UnifiedApiOptions)
        {
            _baseClient = baseClient;
            _options = options;
            _log = log;
        }

        protected override AuthenticationProvider CreateAuthenticationProvider(ApiCredentials credentials)
            => new OkxAuthenticationProvider((OkxApiCredentials)credentials);

        internal Task<WebCallResult> ExecuteAsync(Uri uri, HttpMethod method, CancellationToken ct, Dictionary<string, object> parameters = null, bool signed = false, HttpMethodParameterPosition? parameterPosition = null)
         => _baseClient.ExecuteAsync(this, uri, method, ct, parameters, signed, parameterPosition: parameterPosition);

        internal Task<WebCallResult<T>> ExecuteAsync<T>(Uri uri, HttpMethod method, CancellationToken ct, Dictionary<string, object> parameters = null, bool signed = false, int weight = 1, bool ignoreRatelimit = false, HttpMethodParameterPosition? parameterPosition = null) where T : class
         => _baseClient.ExecuteAsync<T>(this, uri, method, ct, parameters, signed, weight, ignoreRatelimit: ignoreRatelimit, parameterPosition: parameterPosition);

        internal Uri GetUri(string endpoint, string param = "")
        {
            var x = endpoint.IndexOf('<');
            var y = endpoint.IndexOf('>');
            if (x > -1 && y > -1) endpoint = endpoint.Replace(endpoint.Substring(x, y - x + 1), param);

            return new Uri($"{BaseAddress.TrimEnd('/')}/{endpoint}");
        }

        protected override Task<WebCallResult<DateTimeOffset>> GetServerTimestampAsync()
             => _baseClient.GetSystemTimeAsync();

        public override TimeSyncInfo GetTimeSyncInfo()
            => new TimeSyncInfo(_log, _options.UnifiedApiOptions.AutoTimestamp, _options.UnifiedApiOptions.TimestampRecalculationInterval, TimeSyncState);

        public override TimeSpan GetTimeOffset()
            => TimeSyncState.TimeOffset;

    }

}