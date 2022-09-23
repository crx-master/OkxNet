using Newtonsoft.Json;
using SharpCryptoExchange.CommonObjects;
using SharpCryptoExchange.Objects;
using SharpCryptoExchange.Okx.Converters;
using SharpCryptoExchange.Okx.Enums;
using SharpCryptoExchange.Okx.Helpers;
using SharpCryptoExchange.Okx.Models.Core;
using SharpCryptoExchange.Okx.Models.Trade;
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
        #region Trade API Endpoints
        /// <summary>
        /// You can place an order only if you have sufficient funds.
        /// </summary>
        /// <param name="instrumentId">Instrument ID</param>
        /// <param name="tradeMode">Trade Mode</param>
        /// <param name="orderSide">Order Side</param>
        /// <param name="positionSide">Position Side</param>
        /// <param name="orderType">Order Type</param>
        /// <param name="size">Size</param>
        /// <param name="price">Price</param>
        /// <param name="currency">Currency</param>
        /// <param name="clientOrderId">Client Order ID</param>
        /// <param name="tag">Tag</param>
        /// <param name="reduceOnly">Whether to reduce position only or not, true false, the default is false.</param>
        /// <param name="quantityType">Quantity Type</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<OkxOrderPlaceResponse> PlaceOrder(
            string instrumentId,
            OkxTradeMode tradeMode,
            OkxOrderSide orderSide,
            OkxPositionSide positionSide,
            OkxOrderType orderType,
            decimal size,
            decimal? price = null,
            string currency = null,
            string clientOrderId = null,
            string tag = null,
            bool? reduceOnly = null,
            OkxQuantityType? quantityType = null,
            CancellationToken ct = default)
            => PlaceOrderAsync(
            instrumentId,
            tradeMode,
            orderSide,
            positionSide,
            orderType,
            size,
            price,
            currency,
            clientOrderId,
            tag,
            reduceOnly,
            quantityType,
            ct).Result;
        /// <summary>
        /// You can place an order only if you have sufficient funds.
        /// </summary>
        /// <param name="instrumentId">Instrument ID</param>
        /// <param name="tradeMode">Trade Mode</param>
        /// <param name="orderSide">Order Side</param>
        /// <param name="positionSide">Position Side</param>
        /// <param name="orderType">Order Type</param>
        /// <param name="size">Size</param>
        /// <param name="price">Price</param>
        /// <param name="currency">Currency</param>
        /// <param name="clientOrderId">Client Order ID</param>
        /// <param name="tag">Tag</param>
        /// <param name="reduceOnly">Whether to reduce position only or not, true false, the default is false.</param>
        /// <param name="quantityType">Quantity Type</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<OkxOrderPlaceResponse>> PlaceOrderAsync(
            string instrumentId,
            OkxTradeMode tradeMode,
            OkxOrderSide orderSide,
            OkxPositionSide positionSide,
            OkxOrderType orderType,
            decimal size,
            decimal? price = null,
            string currency = null,
            string clientOrderId = null,
            string tag = null,
            bool? reduceOnly = null,
            OkxQuantityType? quantityType = null,
            CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object> {
                {"instId", instrumentId },
                {"tdMode", JsonConvert.SerializeObject(tradeMode, new TradeModeConverter(false)) },
                {"side", JsonConvert.SerializeObject(orderSide, new OrderSideConverter(false)) },
                {"posSide", JsonConvert.SerializeObject(positionSide, new PositionSideConverter(false)) },
                {"ordType", JsonConvert.SerializeObject(orderType, new OrderTypeConverter(false)) },
                {"sz", size.ToString(OkxGlobals.OkxCultureInfo) },
            };
            parameters.AddOptionalParameter("px", price?.ToString(OkxGlobals.OkxCultureInfo));
            parameters.AddOptionalParameter("ccy", currency);
            parameters.AddOptionalParameter("clOrdId", clientOrderId);
            parameters.AddOptionalParameter("tag", tag);
            parameters.AddOptionalParameter("reduceOnly", reduceOnly);
            if (quantityType.HasValue)
                parameters.AddOptionalParameter("tgtCcy", JsonConvert.SerializeObject(quantityType, new QuantityTypeConverter(false)));

            var result = await UnifiedApi.ExecuteAsync<OkxRestApiResponse<IEnumerable<OkxOrderPlaceResponse>>>(UnifiedApi.GetUri(Endpoints.V5.Trade.Order), HttpMethod.Post, ct, parameters, true).ConfigureAwait(false);
            if (!result.Success) return result.AsError<OkxOrderPlaceResponse>(new OkxRestApiError(result.Error.Code, result.Error.Message, result.Error.Data));
            if (result.Data.ErrorCode > 0) return result.AsError<OkxOrderPlaceResponse>(new OkxRestApiError(result.Data.ErrorCode, result.Data.ErrorMessage, null));

            return result.As(result.Data.Data.FirstOrDefault());
        }

        /// <summary>
        /// Place orders in batches. Maximum 20 orders can be placed at a time. Request parameters should be passed in the form of an array.
        /// </summary>
        /// <param name="orders">Orders</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<IEnumerable<OkxOrderPlaceResponse>> PlaceMultipleOrders(IEnumerable<OkxOrderPlaceRequest> orders, CancellationToken ct = default)
            => PlaceMultipleOrdersAsync(orders, ct).Result;
        /// <summary>
        /// Place orders in batches. Maximum 20 orders can be placed at a time. Request parameters should be passed in the form of an array.
        /// </summary>
        /// <param name="orders">Orders</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<IEnumerable<OkxOrderPlaceResponse>>> PlaceMultipleOrdersAsync(IEnumerable<OkxOrderPlaceRequest> orders, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>
            {
                { BodyParameterKey, orders },
            };

            var result = await UnifiedApi.ExecuteAsync<OkxRestApiResponse<IEnumerable<OkxOrderPlaceResponse>>>(UnifiedApi.GetUri(Endpoints.V5.Trade.BatchOrders), HttpMethod.Post, ct, parameters, true).ConfigureAwait(false);
            if (!result.Success) return result.AsError<IEnumerable<OkxOrderPlaceResponse>>(new OkxRestApiError(result.Error.Code, result.Error.Message, result.Error.Data));
            if (result.Data.ErrorCode > 0) return result.AsError<IEnumerable<OkxOrderPlaceResponse>>(new OkxRestApiError(result.Data.ErrorCode, result.Data.ErrorMessage, null));

            return result.As(result.Data.Data);
        }

        /// <summary>
        /// Cancel an incomplete order.
        /// </summary>
        /// <param name="instrumentId">Instrument ID</param>
        /// <param name="orderId">Order ID</param>
        /// <param name="clientOrderId">Client Order ID</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<OkxOrderCancelResponse> CancelOrder(string instrumentId, long? orderId = null, string clientOrderId = null, CancellationToken ct = default)
            => CancelOrderAsync(instrumentId, orderId, clientOrderId, ct).Result;
        /// <summary>
        /// Cancel an incomplete order.
        /// </summary>
        /// <param name="instrumentId">Instrument ID</param>
        /// <param name="orderId">Order ID</param>
        /// <param name="clientOrderId">Client Order ID</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<OkxOrderCancelResponse>> CancelOrderAsync(string instrumentId, long? orderId = null, string clientOrderId = null, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object> {
                {"instId", instrumentId },
            };
            parameters.AddOptionalParameter("ordId", orderId?.ToString(OkxGlobals.OkxCultureInfo));
            parameters.AddOptionalParameter("clOrdId", clientOrderId);

            var result = await UnifiedApi.ExecuteAsync<OkxRestApiResponse<IEnumerable<OkxOrderCancelResponse>>>(UnifiedApi.GetUri(Endpoints.V5.Trade.CancelOrder), HttpMethod.Post, ct, parameters, true).ConfigureAwait(false);
            if (!result.Success) return result.AsError<OkxOrderCancelResponse>(new OkxRestApiError(result.Error.Code, result.Error.Message, result.Error.Data));
            if (result.Data.ErrorCode > 0) return result.AsError<OkxOrderCancelResponse>(new OkxRestApiError(result.Data.ErrorCode, result.Data.ErrorMessage, null));

            return result.As(result.Data.Data.FirstOrDefault());
        }

        /// <summary>
        /// Cancel incomplete orders in batches. Maximum 20 orders can be canceled at a time. Request parameters should be passed in the form of an array.
        /// </summary>
        /// <param name="orders">Orders</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<IEnumerable<OkxOrderCancelResponse>> CancelMultipleOrders(IEnumerable<OkxOrderCancelRequest> orders, CancellationToken ct = default)
            => CancelMultipleOrdersAsync(orders, ct).Result;
        /// <summary>
        /// Cancel incomplete orders in batches. Maximum 20 orders can be canceled at a time. Request parameters should be passed in the form of an array.
        /// </summary>
        /// <param name="orders">Orders</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<IEnumerable<OkxOrderCancelResponse>>> CancelMultipleOrdersAsync(IEnumerable<OkxOrderCancelRequest> orders, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object> {
                { BodyParameterKey, orders },
            };

            var result = await UnifiedApi.ExecuteAsync<OkxRestApiResponse<IEnumerable<OkxOrderCancelResponse>>>(UnifiedApi.GetUri(Endpoints.V5.Trade.CancelBatchOrders), HttpMethod.Post, ct, parameters, true).ConfigureAwait(false);
            if (!result.Success) return result.AsError<IEnumerable<OkxOrderCancelResponse>>(new OkxRestApiError(result.Error.Code, result.Error.Message, result.Error.Data));
            if (result.Data.ErrorCode > 0) return result.AsError<IEnumerable<OkxOrderCancelResponse>>(new OkxRestApiError(result.Data.ErrorCode, result.Data.ErrorMessage, null));

            return result.As(result.Data.Data);
        }

        /// <summary>
        /// Amend an incomplete order.
        /// </summary>
        /// <param name="instrumentId">Instrument ID</param>
        /// <param name="orderId">Order ID</param>
        /// <param name="clientOrderId">Client Order ID</param>
        /// <param name="requestId">Request ID</param>
        /// <param name="cancelOnFail">Cancel On Fail</param>
        /// <param name="newQuantity">New Quantity</param>
        /// <param name="newPrice">New Price</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<OkxOrderAmendResponse> AmendOrder(
            string instrumentId,
            long? orderId = null,
            string clientOrderId = null,
            string requestId = null,
            bool? cancelOnFail = null,
            decimal? newQuantity = null,
            decimal? newPrice = null,
            CancellationToken ct = default)
            => AmendOrderAsync(instrumentId, orderId, clientOrderId, requestId, cancelOnFail, newQuantity, newPrice, ct).Result;
        /// <summary>
        /// Amend an incomplete order.
        /// </summary>
        /// <param name="instrumentId">Instrument ID</param>
        /// <param name="orderId">Order ID</param>
        /// <param name="clientOrderId">Client Order ID</param>
        /// <param name="requestId">Request ID</param>
        /// <param name="cancelOnFail">Cancel On Fail</param>
        /// <param name="newQuantity">New Quantity</param>
        /// <param name="newPrice">New Price</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<OkxOrderAmendResponse>> AmendOrderAsync(
            string instrumentId,
            long? orderId = null,
            string clientOrderId = null,
            string requestId = null,
            bool? cancelOnFail = null,
            decimal? newQuantity = null,
            decimal? newPrice = null,
            CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object>
            {
                { "instId", instrumentId },
            };
            parameters.AddOptionalParameter("ordId", orderId?.ToString(OkxGlobals.OkxCultureInfo));
            parameters.AddOptionalParameter("clOrdId", clientOrderId);
            parameters.AddOptionalParameter("cxlOnFail", cancelOnFail);
            parameters.AddOptionalParameter("reqId", requestId);
            parameters.AddOptionalParameter("newSz", newQuantity?.ToString(OkxGlobals.OkxCultureInfo));
            parameters.AddOptionalParameter("newPx", newPrice?.ToString(OkxGlobals.OkxCultureInfo));

            var result = await UnifiedApi.ExecuteAsync<OkxRestApiResponse<IEnumerable<OkxOrderAmendResponse>>>(UnifiedApi.GetUri(Endpoints.V5.Trade.AmendOrder), HttpMethod.Post, ct, parameters, true).ConfigureAwait(false);
            if (!result.Success) return result.AsError<OkxOrderAmendResponse>(new OkxRestApiError(result.Error.Code, result.Error.Message, result.Error.Data));
            if (result.Data.ErrorCode > 0) return result.AsError<OkxOrderAmendResponse>(new OkxRestApiError(result.Data.ErrorCode, result.Data.ErrorMessage, null));

            return result.As(result.Data.Data.FirstOrDefault());
        }

        /// <summary>
        /// Amend incomplete orders in batches. Maximum 20 orders can be amended at a time. Request parameters should be passed in the form of an array.
        /// </summary>
        /// <param name="orders">Orders</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<IEnumerable<OkxOrderAmendResponse>> AmendMultipleOrders(IEnumerable<OkxOrderAmendRequest> orders, CancellationToken ct = default)
            => AmendMultipleOrdersAsync(orders, ct).Result;
        /// <summary>
        /// Amend incomplete orders in batches. Maximum 20 orders can be amended at a time. Request parameters should be passed in the form of an array.
        /// </summary>
        /// <param name="orders">Orders</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<IEnumerable<OkxOrderAmendResponse>>> AmendMultipleOrdersAsync(IEnumerable<OkxOrderAmendRequest> orders, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object> {
                { BodyParameterKey, orders },
            };

            var result = await UnifiedApi.ExecuteAsync<OkxRestApiResponse<IEnumerable<OkxOrderAmendResponse>>>(UnifiedApi.GetUri(Endpoints.V5.Trade.AmendBatchOrders), HttpMethod.Post, ct, parameters, true).ConfigureAwait(false);
            if (!result.Success) return result.AsError<IEnumerable<OkxOrderAmendResponse>>(new OkxRestApiError(result.Error.Code, result.Error.Message, result.Error.Data));
            if (result.Data.ErrorCode > 0) return result.AsError<IEnumerable<OkxOrderAmendResponse>>(new OkxRestApiError(result.Data.ErrorCode, result.Data.ErrorMessage, null));

            return result.As(result.Data.Data);
        }

        /// <summary>
        /// Close all positions of an instrument via a market order.
        /// </summary>
        /// <param name="instrumentId">Instrument ID</param>
        /// <param name="marginMode">Margin Mode</param>
        /// <param name="positionSide">Position Side</param>
        /// <param name="currency">Currency</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<OkxClosePositionResponse> ClosePosition(
            string instrumentId,
            OkxMarginMode marginMode,
            OkxPositionSide? positionSide = null,
            string currency = null,
            CancellationToken ct = default)
            => ClosePositionAsync(instrumentId, marginMode, positionSide, currency, ct).Result;
        /// <summary>
        /// Close all positions of an instrument via a market order.
        /// </summary>
        /// <param name="instrumentId">Instrument ID</param>
        /// <param name="marginMode">Margin Mode</param>
        /// <param name="positionSide">Position Side</param>
        /// <param name="currency">Currency</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<OkxClosePositionResponse>> ClosePositionAsync(
            string instrumentId,
            OkxMarginMode marginMode,
            OkxPositionSide? positionSide = null,
            string currency = null,
            CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object> {
                {"instId", instrumentId },
                {"mgnMode", JsonConvert.SerializeObject(marginMode, new MarginModeConverter(false)) },
            };
            if (positionSide.HasValue)
                parameters.AddOptionalParameter("posSide", JsonConvert.SerializeObject(positionSide, new PositionSideConverter(false)));
            parameters.AddOptionalParameter("ccy", currency);

            var result = await UnifiedApi.ExecuteAsync<OkxRestApiResponse<IEnumerable<OkxClosePositionResponse>>>(UnifiedApi.GetUri(Endpoints.V5.Trade.ClosePosition), HttpMethod.Post, ct, parameters, true).ConfigureAwait(false);
            if (!result.Success) return result.AsError<OkxClosePositionResponse>(new OkxRestApiError(result.Error.Code, result.Error.Message, result.Error.Data));
            if (result.Data.ErrorCode > 0) return result.AsError<OkxClosePositionResponse>(new OkxRestApiError(result.Data.ErrorCode, result.Data.ErrorMessage, null));

            return result.As(result.Data.Data.FirstOrDefault());
        }

        /// <summary>
        /// Retrieve order details.
        /// </summary>
        /// <param name="instrumentId">Instrument ID</param>
        /// <param name="orderId">Order ID</param>
        /// <param name="clientOrderId">Client Order ID</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<OkxOrder> GetOrderDetails(
            string instrumentId,
            long? orderId = null,
            string clientOrderId = null,
            CancellationToken ct = default)
            => GetOrderDetailsAsync(instrumentId, orderId, clientOrderId, ct).Result;
        /// <summary>
        /// Retrieve order details.
        /// </summary>
        /// <param name="instrumentId">Instrument ID</param>
        /// <param name="orderId">Order ID</param>
        /// <param name="clientOrderId">Client Order ID</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<OkxOrder>> GetOrderDetailsAsync(
            string instrumentId,
            long? orderId = null,
            string clientOrderId = null,
            CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object> {
                {"instId", instrumentId },
            };
            parameters.AddOptionalParameter("ordId", $"{orderId}");
            parameters.AddOptionalParameter("clOrdId", clientOrderId);

            var result = await UnifiedApi.ExecuteAsync<OkxRestApiResponse<IEnumerable<OkxOrder>>>(UnifiedApi.GetUri(Endpoints.V5.Trade.Order), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
            if (!result.Success) return result.AsError<OkxOrder>(new OkxRestApiError(result.Error.Code, result.Error.Message, result.Error.Data));
            if (result.Data.ErrorCode > 0) return result.AsError<OkxOrder>(new OkxRestApiError(result.Data.ErrorCode, result.Data.ErrorMessage, null));

            return result.As(result.Data.Data.FirstOrDefault());
        }

        /// <summary>
        /// Retrieve all incomplete orders under the current account.
        /// </summary>
        /// <param name="instrumentType">Instrument Type</param>
        /// <param name="instrumentId">Instrument ID</param>
        /// <param name="underlying">Underlying</param>
        /// <param name="orderType">Order Type</param>
        /// <param name="state">State</param>
        /// <param name="after">Pagination of data to return records earlier than the requested ts, Unix timestamp format in milliseconds, e.g. 1597026383085</param>
        /// <param name="before">Pagination of data to return records newer than the requested ts, Unix timestamp format in milliseconds, e.g. 1597026383085</param>
        /// <param name="limit">Number of results per request. The maximum is 100; the default is 100.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<IEnumerable<OkxOrder>> GetOrderList(
            OkxInstrumentType? instrumentType = null,
            string instrumentId = null,
            string underlying = null,
            OkxOrderType? orderType = null,
            OkxOrderState? state = null,
            long? after = null,
            long? before = null,
            int limit = 100,
            CancellationToken ct = default)
            => GetOrderListAsync(
            instrumentType,
            instrumentId,
            underlying,
            orderType,
            state,
            after,
            before,
            limit,
            ct).Result;
        /// <summary>
        /// Retrieve all incomplete orders under the current account.
        /// </summary>
        /// <param name="instrumentType">Instrument Type</param>
        /// <param name="instrumentId">Instrument ID</param>
        /// <param name="underlying">Underlying</param>
        /// <param name="orderType">Order Type</param>
        /// <param name="state">State</param>
        /// <param name="after">Pagination of data to return records earlier than the requested ts, Unix timestamp format in milliseconds, e.g. 1597026383085</param>
        /// <param name="before">Pagination of data to return records newer than the requested ts, Unix timestamp format in milliseconds, e.g. 1597026383085</param>
        /// <param name="limit">Number of results per request. The maximum is 100; the default is 100.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<IEnumerable<OkxOrder>>> GetOrderListAsync(
            OkxInstrumentType? instrumentType = null,
            string instrumentId = null,
            string underlying = null,
            OkxOrderType? orderType = null,
            OkxOrderState? state = null,
            long? after = null,
            long? before = null,
            int limit = 100,
            CancellationToken ct = default)
        {
            if (limit < 1 || limit > 100)
                throw new ArgumentException("Limit can be between 1-100.");

            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("instId", instrumentId);
            parameters.AddOptionalParameter("uly", underlying);
            parameters.AddOptionalParameter("after", $"{after}");
            parameters.AddOptionalParameter("before", $"{before}");
            parameters.AddOptionalParameter("limit", $"{limit}");

            if (instrumentType.HasValue)
                parameters.AddOptionalParameter("instType", JsonConvert.SerializeObject(instrumentType, new InstrumentTypeConverter(false)));

            if (orderType.HasValue)
                parameters.AddOptionalParameter("ordType", JsonConvert.SerializeObject(orderType, new OrderTypeConverter(false)));

            if (state.HasValue)
                parameters.AddOptionalParameter("state", JsonConvert.SerializeObject(state, new OrderStateConverter(false)));

            var result = await UnifiedApi.ExecuteAsync<OkxRestApiResponse<IEnumerable<OkxOrder>>>(UnifiedApi.GetUri(Endpoints.V5.Trade.OrdersPending), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
            if (!result.Success) return result.AsError<IEnumerable<OkxOrder>>(new OkxRestApiError(result.Error.Code, result.Error.Message, result.Error.Data));
            if (result.Data.ErrorCode > 0) return result.AsError<IEnumerable<OkxOrder>>(new OkxRestApiError(result.Data.ErrorCode, result.Data.ErrorMessage, null));

            return result.As(result.Data.Data);
        }

        /// <summary>
        /// Retrieve the completed order data for the last 7 days, and the incomplete orders that have been cancelled are only reserved for 2 hours.
        /// </summary>
        /// <param name="instrumentType">Instrument Type</param>
        /// <param name="instrumentId">Instrument ID</param>
        /// <param name="underlying">Underlying</param>
        /// <param name="orderType">Order Type</param>
        /// <param name="state">State</param>
        /// <param name="category">Category</param>
        /// <param name="after">Pagination of data to return records earlier than the requested ts, Unix timestamp format in milliseconds, e.g. 1597026383085</param>
        /// <param name="before">Pagination of data to return records newer than the requested ts, Unix timestamp format in milliseconds, e.g. 1597026383085</param>
        /// <param name="limit">Number of results per request. The maximum is 100; the default is 100.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<IEnumerable<OkxOrder>> GetOrderHistory(
            OkxInstrumentType instrumentType,
            string instrumentId = null,
            string underlying = null,
            OkxOrderType? orderType = null,
            OkxOrderState? state = null,
            OkxOrderCategory? category = null,
            long? after = null,
            long? before = null,
            int limit = 100,
            CancellationToken ct = default)
            => GetOrderHistoryAsync(
            instrumentType,
            instrumentId,
            underlying,
            orderType,
            state,
            category,
            after,
            before,
            limit,
            ct).Result;
        /// <summary>
        /// Retrieve the completed order data for the last 7 days, and the incomplete orders that have been cancelled are only reserved for 2 hours.
        /// </summary>
        /// <param name="instrumentType">Instrument Type</param>
        /// <param name="instrumentId">Instrument ID</param>
        /// <param name="underlying">Underlying</param>
        /// <param name="orderType">Order Type</param>
        /// <param name="state">State</param>
        /// <param name="category">Category</param>
        /// <param name="after">Pagination of data to return records earlier than the requested ts, Unix timestamp format in milliseconds, e.g. 1597026383085</param>
        /// <param name="before">Pagination of data to return records newer than the requested ts, Unix timestamp format in milliseconds, e.g. 1597026383085</param>
        /// <param name="limit">Number of results per request. The maximum is 100; the default is 100.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<IEnumerable<OkxOrder>>> GetOrderHistoryAsync(
            OkxInstrumentType instrumentType,
            string instrumentId = null,
            string underlying = null,
            OkxOrderType? orderType = null,
            OkxOrderState? state = null,
            OkxOrderCategory? category = null,
            long? after = null,
            long? before = null,
            int limit = 100,
            CancellationToken ct = default)
        {
            if (limit < 1 || limit > 100)
                throw new ArgumentException("Limit can be between 1-100.");

            var parameters = new Dictionary<string, object>
            {
                {"instType", JsonConvert.SerializeObject(instrumentType, new InstrumentTypeConverter(false))}
            };
            parameters.AddOptionalParameter("instId", instrumentId);
            parameters.AddOptionalParameter("uly", underlying);
            parameters.AddOptionalParameter("after", $"{after}");
            parameters.AddOptionalParameter("before", $"{before}");
            parameters.AddOptionalParameter("limit", $"{limit}");

            if (orderType.HasValue)
                parameters.AddOptionalParameter("ordType", JsonConvert.SerializeObject(orderType, new OrderTypeConverter(false)));

            if (state.HasValue)
                parameters.AddOptionalParameter("state", JsonConvert.SerializeObject(state, new OrderStateConverter(false)));

            if (category.HasValue)
                parameters.AddOptionalParameter("category", JsonConvert.SerializeObject(category, new OrderCategoryConverter(false)));

            var result = await UnifiedApi.ExecuteAsync<OkxRestApiResponse<IEnumerable<OkxOrder>>>(UnifiedApi.GetUri(Endpoints.V5.Trade.OrdersHistory), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
            if (!result.Success) return result.AsError<IEnumerable<OkxOrder>>(new OkxRestApiError(result.Error.Code, result.Error.Message, result.Error.Data));
            if (result.Data.ErrorCode > 0) return result.AsError<IEnumerable<OkxOrder>>(new OkxRestApiError(result.Data.ErrorCode, result.Data.ErrorMessage, null));

            return result.As(result.Data.Data);
        }

        /// <summary>
        /// Retrieve the completed order data of the last 3 months, and the incomplete orders that have been canceled are only reserved for 2 hours.
        /// </summary>
        /// <param name="instrumentType">Instrument Type</param>
        /// <param name="instrumentId">Instrument ID</param>
        /// <param name="underlying">Underlying</param>
        /// <param name="orderType">Order Type</param>
        /// <param name="state">State</param>
        /// <param name="category">Category</param>
        /// <param name="after">Pagination of data to return records earlier than the requested ts, Unix timestamp format in milliseconds, e.g. 1597026383085</param>
        /// <param name="before">Pagination of data to return records newer than the requested ts, Unix timestamp format in milliseconds, e.g. 1597026383085</param>
        /// <param name="limit">Number of results per request. The maximum is 100; the default is 100.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<IEnumerable<OkxOrder>> GetOrderArchive(
            OkxInstrumentType instrumentType,
            string instrumentId = null,
            string underlying = null,
            OkxOrderType? orderType = null,
            OkxOrderState? state = null,
            OkxOrderCategory? category = null,
            long? after = null,
            long? before = null,
            int limit = 100,
            CancellationToken ct = default)
            => GetOrderArchiveAsync(
            instrumentType,
            instrumentId,
            underlying,
            orderType,
            state,
            category,
            after,
            before,
            limit,
            ct).Result;
        /// <summary>
        /// Retrieve the completed order data of the last 3 months, and the incomplete orders that have been canceled are only reserved for 2 hours.
        /// </summary>
        /// <param name="instrumentType">Instrument Type</param>
        /// <param name="instrumentId">Instrument ID</param>
        /// <param name="underlying">Underlying</param>
        /// <param name="orderType">Order Type</param>
        /// <param name="state">State</param>
        /// <param name="category">Category</param>
        /// <param name="after">Pagination of data to return records earlier than the requested ts, Unix timestamp format in milliseconds, e.g. 1597026383085</param>
        /// <param name="before">Pagination of data to return records newer than the requested ts, Unix timestamp format in milliseconds, e.g. 1597026383085</param>
        /// <param name="limit">Number of results per request. The maximum is 100; the default is 100.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<IEnumerable<OkxOrder>>> GetOrderArchiveAsync(
            OkxInstrumentType instrumentType,
            string instrumentId = null,
            string underlying = null,
            OkxOrderType? orderType = null,
            OkxOrderState? state = null,
            OkxOrderCategory? category = null,
            long? after = null,
            long? before = null,
            int limit = 100,
            CancellationToken ct = default)
        {
            if (limit < 1 || limit > 100)
                throw new ArgumentException("Limit can be between 1-100.");

            var parameters = new Dictionary<string, object>
            {
                {"instType", JsonConvert.SerializeObject(instrumentType, new InstrumentTypeConverter(false))}
            };
            parameters.AddOptionalParameter("instId", instrumentId);
            parameters.AddOptionalParameter("uly", underlying);
            parameters.AddOptionalParameter("after", $"{after}");
            parameters.AddOptionalParameter("before", $"{before}");
            parameters.AddOptionalParameter("limit", $"{limit}");

            if (orderType.HasValue)
                parameters.AddOptionalParameter("ordType", JsonConvert.SerializeObject(orderType, new OrderTypeConverter(false)));

            if (state.HasValue)
                parameters.AddOptionalParameter("state", JsonConvert.SerializeObject(state, new OrderStateConverter(false)));

            if (category.HasValue)
                parameters.AddOptionalParameter("category", JsonConvert.SerializeObject(category, new OrderCategoryConverter(false)));

            var result = await UnifiedApi.ExecuteAsync<OkxRestApiResponse<IEnumerable<OkxOrder>>>(UnifiedApi.GetUri(Endpoints.V5.Trade.OrdersHistoryArchive), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
            if (!result.Success) return result.AsError<IEnumerable<OkxOrder>>(new OkxRestApiError(result.Error.Code, result.Error.Message, result.Error.Data));
            if (result.Data.ErrorCode > 0) return result.AsError<IEnumerable<OkxOrder>>(new OkxRestApiError(result.Data.ErrorCode, result.Data.ErrorMessage, null));

            return result.As(result.Data.Data);
        }

        /// <summary>
        /// Retrieve recently-filled transaction details in the last 3 day.
        /// </summary>
        /// <param name="instrumentType">Instrument Type</param>
        /// <param name="instrumentId">Instrument ID</param>
        /// <param name="underlying">Underlying</param>
        /// <param name="orderId">Order ID</param>
        /// <param name="after">Pagination of data to return records earlier than the requested ts, Unix timestamp format in milliseconds, e.g. 1597026383085</param>
        /// <param name="before">Pagination of data to return records newer than the requested ts, Unix timestamp format in milliseconds, e.g. 1597026383085</param>
        /// <param name="limit">Number of results per request. The maximum is 100; the default is 100.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<IEnumerable<OkxTransaction>> GetTransactionHistory(
            OkxInstrumentType? instrumentType = null,
            string instrumentId = null,
            string underlying = null,
            long? orderId = null,
            long? after = null,
            long? before = null,
            int limit = 100,
            CancellationToken ct = default)
            => GetTransactionHistoryAsync(
            instrumentType,
            instrumentId,
            underlying,
            orderId,
            after,
            before,
            limit,
            ct).Result;
        /// <summary>
        /// Retrieve recently-filled transaction details in the last 3 day.
        /// </summary>
        /// <param name="instrumentType">Instrument Type</param>
        /// <param name="instrumentId">Instrument ID</param>
        /// <param name="underlying">Underlying</param>
        /// <param name="orderId">Order ID</param>
        /// <param name="after">Pagination of data to return records earlier than the requested ts, Unix timestamp format in milliseconds, e.g. 1597026383085</param>
        /// <param name="before">Pagination of data to return records newer than the requested ts, Unix timestamp format in milliseconds, e.g. 1597026383085</param>
        /// <param name="limit">Number of results per request. The maximum is 100; the default is 100.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<IEnumerable<OkxTransaction>>> GetTransactionHistoryAsync(
            OkxInstrumentType? instrumentType = null,
            string instrumentId = null,
            string underlying = null,
            long? orderId = null,
            long? after = null,
            long? before = null,
            int limit = 100,
            CancellationToken ct = default)
        {
            if (limit < 1 || limit > 100)
                throw new ArgumentException("Limit can be between 1-100.");

            var parameters = new Dictionary<string, object>();
            if (instrumentType.HasValue)
                parameters.AddOptionalParameter("instType", JsonConvert.SerializeObject(instrumentType, new InstrumentTypeConverter(false)));
            parameters.AddOptionalParameter("instId", instrumentId);
            parameters.AddOptionalParameter("uly", underlying);
            parameters.AddOptionalParameter("ordId", $"{orderId}");
            parameters.AddOptionalParameter("after", $"{after}");
            parameters.AddOptionalParameter("before", $"{before}");
            parameters.AddOptionalParameter("limit", $"{limit}");

            var result = await UnifiedApi.ExecuteAsync<OkxRestApiResponse<IEnumerable<OkxTransaction>>>(UnifiedApi.GetUri(Endpoints.V5.Trade.Fills), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
            if (!result.Success) return result.AsError<IEnumerable<OkxTransaction>>(new OkxRestApiError(result.Error.Code, result.Error.Message, result.Error.Data));
            if (result.Data.ErrorCode > 0) return result.AsError<IEnumerable<OkxTransaction>>(new OkxRestApiError(result.Data.ErrorCode, result.Data.ErrorMessage, null));

            return result.As(result.Data.Data);
        }

        /// <summary>
        /// Retrieve recently-filled transaction details in the last 3 months.
        /// </summary>
        /// <param name="instrumentType">Instrument Type</param>
        /// <param name="instrumentId">Instrument ID</param>
        /// <param name="underlying">Underlying</param>
        /// <param name="orderId">Order ID</param>
        /// <param name="after">Pagination of data to return records earlier than the requested ts, Unix timestamp format in milliseconds, e.g. 1597026383085</param>
        /// <param name="before">Pagination of data to return records newer than the requested ts, Unix timestamp format in milliseconds, e.g. 1597026383085</param>
        /// <param name="limit">Number of results per request. The maximum is 100; the default is 100.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<IEnumerable<OkxTransaction>> GetTransactionArchive(
            OkxInstrumentType instrumentType,
            string instrumentId = null,
            string underlying = null,
            long? orderId = null,
            long? after = null,
            long? before = null,
            int limit = 100,
            CancellationToken ct = default)
            => GetTransactionArchiveAsync(
            instrumentType,
            instrumentId,
            underlying,
            orderId,
            after,
            before,
            limit,
            ct).Result;
        /// <summary>
        /// Retrieve recently-filled transaction details in the last 3 months.
        /// </summary>
        /// <param name="instrumentType">Instrument Type</param>
        /// <param name="instrumentId">Instrument ID</param>
        /// <param name="underlying">Underlying</param>
        /// <param name="orderId">Order ID</param>
        /// <param name="after">Pagination of data to return records earlier than the requested ts, Unix timestamp format in milliseconds, e.g. 1597026383085</param>
        /// <param name="before">Pagination of data to return records newer than the requested ts, Unix timestamp format in milliseconds, e.g. 1597026383085</param>
        /// <param name="limit">Number of results per request. The maximum is 100; the default is 100.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<IEnumerable<OkxTransaction>>> GetTransactionArchiveAsync(
            OkxInstrumentType instrumentType,
            string instrumentId = null,
            string underlying = null,
            long? orderId = null,
            long? after = null,
            long? before = null,
            int limit = 100,
            CancellationToken ct = default)
        {
            if (limit < 1 || limit > 100)
                throw new ArgumentException("Limit can be between 1-100.");

            var parameters = new Dictionary<string, object>();
            parameters.AddOptionalParameter("instType", JsonConvert.SerializeObject(instrumentType, new InstrumentTypeConverter(false)));
            parameters.AddOptionalParameter("instId", instrumentId);
            parameters.AddOptionalParameter("uly", underlying);
            parameters.AddOptionalParameter("ordId", $"{orderId}");
            parameters.AddOptionalParameter("after", $"{after}");
            parameters.AddOptionalParameter("before", $"{before}");
            parameters.AddOptionalParameter("limit", $"{limit}");

            var result = await UnifiedApi.ExecuteAsync<OkxRestApiResponse<IEnumerable<OkxTransaction>>>(UnifiedApi.GetUri(Endpoints.V5.Trade.FillsHistory), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
            if (!result.Success) return result.AsError<IEnumerable<OkxTransaction>>(new OkxRestApiError(result.Error.Code, result.Error.Message, result.Error.Data));
            if (result.Data.ErrorCode > 0) return result.AsError<IEnumerable<OkxTransaction>>(new OkxRestApiError(result.Data.ErrorCode, result.Data.ErrorMessage, null));

            return result.As(result.Data.Data);
        }

        /// <summary>
        /// The algo order includes trigger order, oco order, conditional order,iceberg order and twap order.
        /// </summary>
        /// <param name="instrumentId">Instrument ID</param>
        /// <param name="tradeMode">Trade Mode</param>
        /// <param name="orderSide">Order Side</param>
        /// <param name="algoOrderType">Algo Order Type</param>
        /// <param name="size">Size</param>
        /// <param name="currency">Currency</param>
        /// <param name="reduceOnly">Reduce Only</param>
        /// <param name="positionSide">Position Side</param>
        /// <param name="quantityType">Quantity Type</param>
        /// <param name="tpTriggerPrice">Take Profit Trigger Price</param>
        /// <param name="tpOrderPrice">Take Profit Order Price</param>
        /// <param name="slTriggerPrice">Stop Loss Trigger Price</param>
        /// <param name="slOrderPrice">Stop Loss Order Price</param>
        /// <param name="triggerPrice">Trigger Price</param>
        /// <param name="orderPrice">Order Price</param>
        /// <param name="pxVar">Price Variance</param>
        /// <param name="priceRatio">Price Ratio</param>
        /// <param name="sizeLimit">Size Limit</param>
        /// <param name="priceLimit">Price Limit</param>
        /// <param name="timeInterval">Time Interval</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<OkxAlgoOrderResponse> PlaceAlgoOrder(
            /* Common */
            string instrumentId,
            OkxTradeMode tradeMode,
            OkxOrderSide orderSide,
            OkxAlgoOrderType algoOrderType,
            decimal size,
            string currency = null,
            bool? reduceOnly = null,
            OkxPositionSide? positionSide = null,
            OkxQuantityType? quantityType = null,

            /* Stop Order */
            decimal? tpTriggerPrice = null,
            decimal? tpOrderPrice = null,
            decimal? slTriggerPrice = null,
            decimal? slOrderPrice = null,

            /* Trigger Order */
            decimal? triggerPrice = null,
            decimal? orderPrice = null,

            /* Iceberg Order */
            /* TWAP Order */
            OkxPriceVariance? pxVar = null,
            decimal? priceRatio = null,
            decimal? sizeLimit = null,
            decimal? priceLimit = null,

            /* TWAP Order */
            long? timeInterval = null,

            /* Cancellation Token */
            CancellationToken ct = default)
            => PlaceAlgoOrderAsync(
            /* Common */
            instrumentId,
            tradeMode,
            orderSide,
            algoOrderType,
            size,
            currency,
            reduceOnly,
            positionSide,
            quantityType,

            /* Stop Order */
            tpTriggerPrice,
            tpOrderPrice,
            slTriggerPrice,
            slOrderPrice,

            /* Trigger Order */
            triggerPrice,
            orderPrice,

            /* Iceberg Order */
            /* TWAP Order */
            pxVar,
            priceRatio,
            sizeLimit,
            priceLimit,

            /* TWAP Order */
            timeInterval,

            /* Cancellation Token */
            ct).Result;
        /// <summary>
        /// The algo order includes trigger order, oco order, conditional order,iceberg order and twap order.
        /// </summary>
        /// <param name="instrumentId">Instrument ID</param>
        /// <param name="tradeMode">Trade Mode</param>
        /// <param name="orderSide">Order Side</param>
        /// <param name="algoOrderType">Algo Order Type</param>
        /// <param name="size">Size</param>
        /// <param name="currency">Currency</param>
        /// <param name="reduceOnly">Reduce Only</param>
        /// <param name="positionSide">Position Side</param>
        /// <param name="quantityType">Quantity Type</param>
        /// <param name="tpTriggerPrice">Take Profit Trigger Price</param>
        /// <param name="tpOrderPrice">Take Profit Order Price</param>
        /// <param name="slTriggerPrice">Stop Loss Trigger Price</param>
        /// <param name="slOrderPrice">Stop Loss Order Price</param>
        /// <param name="triggerPrice">Trigger Price</param>
        /// <param name="orderPrice">Order Price</param>
        /// <param name="pxVar">Price Variance</param>
        /// <param name="priceRatio">Price Ratio</param>
        /// <param name="sizeLimit">Size Limit</param>
        /// <param name="priceLimit">Price Limit</param>
        /// <param name="timeInterval">Time Interval</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<OkxAlgoOrderResponse>> PlaceAlgoOrderAsync(
            //* Common */
            string instrumentId,
            OkxTradeMode tradeMode,
            OkxOrderSide orderSide,
            OkxAlgoOrderType algoOrderType,
            decimal size,
            string currency = null,
            bool? reduceOnly = null,
            OkxPositionSide? positionSide = null,
            OkxQuantityType? quantityType = null,

            /* Stop Order */
            decimal? tpTriggerPrice = null,
            decimal? tpOrderPrice = null,
            decimal? slTriggerPrice = null,
            decimal? slOrderPrice = null,

            /* Trigger Order */
            decimal? triggerPrice = null,
            decimal? orderPrice = null,

            /* Iceberg Order */
            /* TWAP Order */
            OkxPriceVariance? pxVar = null,
            decimal? priceRatio = null,
            decimal? sizeLimit = null,
            decimal? priceLimit = null,

            /* TWAP Order */
            long? timeInterval = null,

            /* Cancellation Token */
            CancellationToken ct = default)
        {
            /* Common */
            var parameters = new Dictionary<string, object> {
                {"instId", instrumentId },
                {"tdMode", JsonConvert.SerializeObject(tradeMode, new TradeModeConverter(false)) },
                {"side", JsonConvert.SerializeObject(orderSide, new OrderSideConverter(false)) },
                {"ordType", JsonConvert.SerializeObject(algoOrderType, new AlgoOrderTypeConverter(false)) },
                {"sz", size.ToString(OkxGlobals.OkxCultureInfo) },
            };
            parameters.AddOptionalParameter("ccy", currency);
            parameters.AddOptionalParameter("reduceOnly", reduceOnly);

            if (positionSide.HasValue)
                parameters.AddOptionalParameter("posSide", JsonConvert.SerializeObject(positionSide, new PositionSideConverter(false)));
            if (quantityType.HasValue)
                parameters.AddOptionalParameter("tgtCcy", JsonConvert.SerializeObject(quantityType, new QuantityTypeConverter(false)));

            /* Stop Order */
            parameters.AddOptionalParameter("tpTriggerPx", tpTriggerPrice?.ToString(OkxGlobals.OkxCultureInfo));
            parameters.AddOptionalParameter("tpOrdPx", tpOrderPrice?.ToString(OkxGlobals.OkxCultureInfo));
            parameters.AddOptionalParameter("slTriggerPx", slTriggerPrice?.ToString(OkxGlobals.OkxCultureInfo));
            parameters.AddOptionalParameter("slOrdPx", slOrderPrice?.ToString(OkxGlobals.OkxCultureInfo));

            /* Trigger Order */
            parameters.AddOptionalParameter("triggerPx", triggerPrice?.ToString(OkxGlobals.OkxCultureInfo));
            parameters.AddOptionalParameter("orderPx", orderPrice?.ToString(OkxGlobals.OkxCultureInfo));

            /* Iceberg Order */
            /* TWAP Order */
            if (pxVar.HasValue)
                parameters.AddOptionalParameter("pxVar", JsonConvert.SerializeObject(pxVar, new PriceVarianceConverter(false)));
            parameters.AddOptionalParameter("pxSpread", priceRatio?.ToString(OkxGlobals.OkxCultureInfo));
            parameters.AddOptionalParameter("szLimit", sizeLimit?.ToString(OkxGlobals.OkxCultureInfo));
            parameters.AddOptionalParameter("pxLimit", priceLimit?.ToString(OkxGlobals.OkxCultureInfo));

            /* TWAP Order */
            parameters.AddOptionalParameter("timeInterval", timeInterval?.ToString(OkxGlobals.OkxCultureInfo));

            var result = await UnifiedApi.ExecuteAsync<OkxRestApiResponse<IEnumerable<OkxAlgoOrderResponse>>>(UnifiedApi.GetUri(Endpoints.V5.Trade.OrderAlgo), HttpMethod.Post, ct, parameters, true).ConfigureAwait(false);
            if (!result.Success) return result.AsError<OkxAlgoOrderResponse>(new OkxRestApiError(result.Error.Code, result.Error.Message, result.Error.Data));
            if (result.Data.ErrorCode > 0) return result.AsError<OkxAlgoOrderResponse>(new OkxRestApiError(result.Data.ErrorCode, result.Data.ErrorMessage, null));

            return result.As(result.Data.Data.FirstOrDefault());
        }

        /// <summary>
        /// Cancel unfilled algo orders(trigger order, oco order, conditional order). A maximum of 10 orders can be canceled at a time. Request parameters should be passed in the form of an array.
        /// </summary>
        /// <param name="orders">Orders</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<OkxAlgoOrderResponse> CancelAlgoOrder(IEnumerable<OkxAlgoOrderRequest> orders, CancellationToken ct = default)
            => CancelAlgoOrderAsync(orders, ct).Result;
        /// <summary>
        /// Cancel unfilled algo orders(trigger order, oco order, conditional order). A maximum of 10 orders can be canceled at a time. Request parameters should be passed in the form of an array.
        /// </summary>
        /// <param name="orders">Orders</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<OkxAlgoOrderResponse>> CancelAlgoOrderAsync(IEnumerable<OkxAlgoOrderRequest> orders, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object> {
                {BodyParameterKey, orders },
            };

            var result = await UnifiedApi.ExecuteAsync<OkxRestApiResponse<IEnumerable<OkxAlgoOrderResponse>>>(UnifiedApi.GetUri(Endpoints.V5.Trade.CancelAlgos), HttpMethod.Post, ct, parameters, true).ConfigureAwait(false);
            if (!result.Success) return result.AsError<OkxAlgoOrderResponse>(new OkxRestApiError(result.Error.Code, result.Error.Message, result.Error.Data));
            if (result.Data.ErrorCode > 0) return result.AsError<OkxAlgoOrderResponse>(new OkxRestApiError(result.Data.ErrorCode, result.Data.ErrorMessage, null));

            return result.As(result.Data.Data.FirstOrDefault());
        }

        /// <summary>
        /// Cancel unfilled algo orders(iceberg order and twap order). A maximum of 10 orders can be canceled at a time. Request parameters should be passed in the form of an array.
        /// </summary>
        /// <param name="orders">Orders</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<OkxAlgoOrderResponse> CancelAdvanceAlgoOrder(IEnumerable<OkxAlgoOrderRequest> orders, CancellationToken ct = default)
            => CancelAdvanceAlgoOrderAsync(orders, ct).Result;
        /// <summary>
        /// Cancel unfilled algo orders(iceberg order and twap order). A maximum of 10 orders can be canceled at a time. Request parameters should be passed in the form of an array.
        /// </summary>
        /// <param name="orders">Orders</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<OkxAlgoOrderResponse>> CancelAdvanceAlgoOrderAsync(IEnumerable<OkxAlgoOrderRequest> orders, CancellationToken ct = default)
        {
            var parameters = new Dictionary<string, object> {
                {BodyParameterKey, orders },
            };

            var result = await UnifiedApi.ExecuteAsync<OkxRestApiResponse<IEnumerable<OkxAlgoOrderResponse>>>(UnifiedApi.GetUri(Endpoints.V5.Trade.CancelAdvanceAlgos), HttpMethod.Post, ct, parameters, true).ConfigureAwait(false);
            if (!result.Success) return result.AsError<OkxAlgoOrderResponse>(new OkxRestApiError(result.Error.Code, result.Error.Message, result.Error.Data));
            if (result.Data.ErrorCode > 0) return result.AsError<OkxAlgoOrderResponse>(new OkxRestApiError(result.Data.ErrorCode, result.Data.ErrorMessage, null));

            return result.As(result.Data.Data.FirstOrDefault());
        }

        /// <summary>
        /// Cancel unfilled algo orders(iceberg order and twap order). A maximum of 10 orders can be canceled at a time. Request parameters should be passed in the form of an array.
        /// </summary>
        /// <param name="algoOrderType">Algo Order Type</param>
        /// <param name="algoId">Algo ID</param>
        /// <param name="instrumentType">Instrument Type</param>
        /// <param name="instrumentId">Instrument ID</param>
        /// <param name="after">Pagination of data to return records earlier than the requested ts, Unix timestamp format in milliseconds, e.g. 1597026383085</param>
        /// <param name="before">Pagination of data to return records newer than the requested ts, Unix timestamp format in milliseconds, e.g. 1597026383085</param>
        /// <param name="limit">Number of results per request. The maximum is 100; the default is 100.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<IEnumerable<OkxAlgoOrder>> GetAlgoOrderList(
            OkxAlgoOrderType algoOrderType,
            long? algoId = null,
            OkxInstrumentType? instrumentType = null,
            string instrumentId = null,
            long? after = null,
            long? before = null,
            int limit = 100,
            CancellationToken ct = default)
            => GetAlgoOrderListAsync(
            algoOrderType,
            algoId,
            instrumentType,
            instrumentId,
            after,
            before,
            limit,
            ct).Result;
        /// <summary>
        /// Cancel unfilled algo orders(iceberg order and twap order). A maximum of 10 orders can be canceled at a time. Request parameters should be passed in the form of an array.
        /// </summary>
        /// <param name="algoOrderType">Algo Order Type</param>
        /// <param name="algoId">Algo ID</param>
        /// <param name="instrumentType">Instrument Type</param>
        /// <param name="instrumentId">Instrument ID</param>
        /// <param name="after">Pagination of data to return records earlier than the requested ts, Unix timestamp format in milliseconds, e.g. 1597026383085</param>
        /// <param name="before">Pagination of data to return records newer than the requested ts, Unix timestamp format in milliseconds, e.g. 1597026383085</param>
        /// <param name="limit">Number of results per request. The maximum is 100; the default is 100.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<IEnumerable<OkxAlgoOrder>>> GetAlgoOrderListAsync(
            OkxAlgoOrderType algoOrderType,
            long? algoId = null,
            OkxInstrumentType? instrumentType = null,
            string instrumentId = null,
            long? after = null,
            long? before = null,
            int limit = 100,
            CancellationToken ct = default)
        {
            if (limit < 1 || limit > 100)
                throw new ArgumentException("Limit can be between 1-100.");

            var parameters = new Dictionary<string, object>
            {
                {"ordType",   JsonConvert.SerializeObject(algoOrderType, new AlgoOrderTypeConverter(false))}
            };
            parameters.AddOptionalParameter("algoId", algoId?.ToString(OkxGlobals.OkxCultureInfo));
            parameters.AddOptionalParameter("instId", instrumentId);

            parameters.AddOptionalParameter("after", $"{after}");
            parameters.AddOptionalParameter("before", $"{before}");
            parameters.AddOptionalParameter("limit", $"{limit}");

            if (instrumentType.HasValue)
                parameters.AddOptionalParameter("instType", JsonConvert.SerializeObject(instrumentType, new InstrumentTypeConverter(false)));

            var result = await UnifiedApi.ExecuteAsync<OkxRestApiResponse<IEnumerable<OkxAlgoOrder>>>(UnifiedApi.GetUri(Endpoints.V5.Trade.OrdersAlgoPending), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
            if (!result.Success) return result.AsError<IEnumerable<OkxAlgoOrder>>(new OkxRestApiError(result.Error.Code, result.Error.Message, result.Error.Data));
            if (result.Data.ErrorCode > 0) return result.AsError<IEnumerable<OkxAlgoOrder>>(new OkxRestApiError(result.Data.ErrorCode, result.Data.ErrorMessage, null));

            return result.As(result.Data.Data);
        }

        /// <summary>
        /// Retrieve a list of untriggered Algo orders under the current account.
        /// </summary>
        /// <param name="algoOrderType">Algo Order Type</param>
        /// <param name="algoOrderState">Algo Order State</param>
        /// <param name="algoId">Algo ID</param>
        /// <param name="instrumentType">Instrument Type</param>
        /// <param name="instrumentId">Instrument ID</param>
        /// <param name="after">Pagination of data to return records earlier than the requested ts, Unix timestamp format in milliseconds, e.g. 1597026383085</param>
        /// <param name="before">Pagination of data to return records newer than the requested ts, Unix timestamp format in milliseconds, e.g. 1597026383085</param>
        /// <param name="limit">Number of results per request. The maximum is 100; the default is 100.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual WebCallResult<IEnumerable<OkxAlgoOrder>> GetAlgoOrderHistory(
            OkxAlgoOrderType algoOrderType,
            OkxAlgoOrderState? algoOrderState = null,
            long? algoId = null,
            OkxInstrumentType? instrumentType = null,
            string instrumentId = null,
            long? after = null,
            long? before = null,
            int limit = 100,
            CancellationToken ct = default)
            => GetAlgoOrderHistoryAsync(
            algoOrderType,
            algoOrderState,
            algoId,
            instrumentType,
            instrumentId,
            after,
            before,
            limit,
            ct).Result;
        /// <summary>
        /// Retrieve a list of untriggered Algo orders under the current account.
        /// </summary>
        /// <param name="algoOrderType">Algo Order Type</param>
        /// <param name="algoOrderState">Algo Order State</param>
        /// <param name="algoId">Algo ID</param>
        /// <param name="instrumentType">Instrument Type</param>
        /// <param name="instrumentId">Instrument ID</param>
        /// <param name="after">Pagination of data to return records earlier than the requested ts, Unix timestamp format in milliseconds, e.g. 1597026383085</param>
        /// <param name="before">Pagination of data to return records newer than the requested ts, Unix timestamp format in milliseconds, e.g. 1597026383085</param>
        /// <param name="limit">Number of results per request. The maximum is 100; the default is 100.</param>
        /// <param name="ct">Cancellation Token</param>
        /// <returns></returns>
        public virtual async Task<WebCallResult<IEnumerable<OkxAlgoOrder>>> GetAlgoOrderHistoryAsync(
            OkxAlgoOrderType algoOrderType,
            OkxAlgoOrderState? algoOrderState = null,
            long? algoId = null,
            OkxInstrumentType? instrumentType = null,
            string instrumentId = null,
            long? after = null,
            long? before = null,
            int limit = 100,
            CancellationToken ct = default)
        {
            if (limit < 1 || limit > 100)
                throw new ArgumentException("Limit can be between 1-100.");

            var parameters = new Dictionary<string, object>
            {
                {"ordType",   JsonConvert.SerializeObject(algoOrderType, new AlgoOrderTypeConverter(false))}
            };
            parameters.AddOptionalParameter("algoId", algoId?.ToString(OkxGlobals.OkxCultureInfo));
            parameters.AddOptionalParameter("instId", instrumentId);
            parameters.AddOptionalParameter("after", $"{after}");
            parameters.AddOptionalParameter("before", $"{before}");
            parameters.AddOptionalParameter("limit", $"{limit}");

            if (algoOrderState.HasValue)
                parameters.AddOptionalParameter("state", JsonConvert.SerializeObject(algoOrderState, new AlgoOrderStateConverter(false)));

            if (instrumentType.HasValue)
                parameters.AddOptionalParameter("instType", JsonConvert.SerializeObject(instrumentType, new InstrumentTypeConverter(false)));

            var result = await UnifiedApi.ExecuteAsync<OkxRestApiResponse<IEnumerable<OkxAlgoOrder>>>(UnifiedApi.GetUri(Endpoints.V5.Trade.OrdersAlgoHistory), HttpMethod.Get, ct, parameters, true).ConfigureAwait(false);
            if (!result.Success) return result.AsError<IEnumerable<OkxAlgoOrder>>(new OkxRestApiError(result.Error.Code, result.Error.Message, result.Error.Data));
            if (result.Data.ErrorCode > 0) return result.AsError<IEnumerable<OkxAlgoOrder>>(new OkxRestApiError(result.Data.ErrorCode, result.Data.ErrorMessage, null));

            return result.As(result.Data.Data);
        }

        #endregion
    }
}