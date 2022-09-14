using SharpCryptoExchange.Objects;
using SharpCryptoExchange.Sockets;
using SharpCryptoExchange.Okx.Enums;
using SharpCryptoExchange.Okx.Objects.Account;
using SharpCryptoExchange.Okx.Objects.Core;
using SharpCryptoExchange.Okx.Objects.Trade;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SharpCryptoExchange.Okx
{
    public partial class OkxSocketClient
    {
        #region Private Signed Feeds
        /// <summary>
        /// Retrieve account information. Data will be pushed when triggered by events such as placing/canceling order, and will also be pushed in regular interval according to subscription granularity.
        /// </summary>
        /// <param name="onData">On Data Handler</param>
        /// <returns></returns>
        public virtual CallResult<UpdateSubscription> SubscribeToAccountUpdates(
            Action<OkxAccountBalance> onData)
            => SubscribeToAccountUpdatesAsync(onData).Result;
        /// <summary>
        /// Retrieve account information. Data will be pushed when triggered by events such as placing/canceling order, and will also be pushed in regular interval according to subscription granularity.
        /// </summary>
        /// <param name="onData">On Data Handler</param>
        /// <returns></returns>
        public virtual async Task<CallResult<UpdateSubscription>> SubscribeToAccountUpdatesAsync(
            Action<OkxAccountBalance> onData,
            CancellationToken ct = default)
        {
            var internalHandler = new Action<DataEvent<OkxSocketUpdateResponse<IEnumerable<OkxAccountBalance>>>>(data =>
            {
                foreach (var d in data.Data.Data)
                    onData(d);
            });

            var request = new OkxSocketRequest(OkxSocketOperation.Subscribe, "account");
            return await UnifiedSubscribeAsync(request, null, true, internalHandler, ct).ConfigureAwait(false);
        }

        /// <summary>
        /// Retrieve position information. Initial snapshot will be pushed according to subscription granularity. Data will be pushed when triggered by events such as placing/canceling order, and will also be pushed in regular interval according to subscription granularity.
        /// </summary>
        /// <param name="instrumentType">Instrument Type</param>
        /// <param name="instrumentId">Instrument ID</param>
        /// <param name="underlying">Underlying</param>
        /// <param name="onData">On Data Handler</param>
        /// <returns></returns>
        public virtual CallResult<UpdateSubscription> SubscribeToPositionUpdates(
            OkxInstrumentType instrumentType,
            string instrumentId,
            string underlying,
            Action<OkxPosition> onData)
            => SubscribeToPositionUpdatesAsync(instrumentType, instrumentId, underlying, onData).Result;
        /// <summary>
        /// Retrieve position information. Initial snapshot will be pushed according to subscription granularity. Data will be pushed when triggered by events such as placing/canceling order, and will also be pushed in regular interval according to subscription granularity.
        /// </summary>
        /// <param name="instrumentType">Instrument Type</param>
        /// <param name="instrumentId">Instrument ID</param>
        /// <param name="underlying">Underlying</param>
        /// <param name="onData">On Data Handler</param>
        /// <returns></returns>
        public virtual async Task<CallResult<UpdateSubscription>> SubscribeToPositionUpdatesAsync(
            OkxInstrumentType instrumentType,
            string instrumentId,
            string underlying,
            Action<OkxPosition> onData,
            CancellationToken ct = default)
        {
            var internalHandler = new Action<DataEvent<OkxSocketUpdateResponse<IEnumerable<OkxPosition>>>>(data =>
            {
                foreach (var d in data.Data.Data)
                    onData(d);
            });

            var request = new OkxSocketRequest(OkxSocketOperation.Subscribe, new OkxSocketRequestArgument
            {
                Channel = "positions",
                InstrumentId = instrumentId,
                InstrumentType = instrumentType,
                Underlying = underlying,
            });
            return await UnifiedSubscribeAsync(request, null, true, internalHandler, ct).ConfigureAwait(false);
        }

        /// <summary>
        /// Retrieve account balance and position information. Data will be pushed when triggered by events such as filled order, funding transfer.
        /// </summary>
        /// <param name="onData">On Data Handler</param>
        /// <returns></returns>
        public virtual CallResult<UpdateSubscription> SubscribeToBalanceAndPositionUpdates(
            Action<OkxPositionRisk> onData)
            => SubscribeToBalanceAndPositionUpdatesAsync(onData).Result;
        /// <summary>
        /// Retrieve account balance and position information. Data will be pushed when triggered by events such as filled order, funding transfer.
        /// </summary>
        /// <param name="onData">On Data Handler</param>
        /// <returns></returns>
        public virtual async Task<CallResult<UpdateSubscription>> SubscribeToBalanceAndPositionUpdatesAsync(
            Action<OkxPositionRisk> onData,
            CancellationToken ct = default)
        {
            var internalHandler = new Action<DataEvent<OkxSocketUpdateResponse<IEnumerable<OkxPositionRisk>>>>(data =>
            {
                foreach (var d in data.Data.Data)
                    onData(d);
            });

            var request = new OkxSocketRequest(OkxSocketOperation.Subscribe, "balance_and_position");
            return await UnifiedSubscribeAsync(request, null, true, internalHandler, ct).ConfigureAwait(false);
        }

        /// <summary>
        /// Retrieve order information. Data will not be pushed when first subscribed. Data will only be pushed when triggered by events such as placing/canceling order.
        /// </summary>
        /// <param name="instrumentType">Instrument Type</param>
        /// <param name="instrumentId">Instrument ID</param>
        /// <param name="underlying">Underlying</param>
        /// <param name="onData">On Data Handler</param>
        /// <returns></returns>
        public virtual CallResult<UpdateSubscription> SubscribeToOrderUpdates(
            OkxInstrumentType instrumentType,
            string instrumentId,
            string underlying,
            Action<OkxOrder> onData)
            => SubscribeToOrderUpdatesAsync(instrumentType, instrumentId, underlying, onData).Result;
        /// <summary>
        /// Retrieve order information. Data will not be pushed when first subscribed. Data will only be pushed when triggered by events such as placing/canceling order.
        /// </summary>
        /// <param name="instrumentType">Instrument Type</param>
        /// <param name="instrumentId">Instrument ID</param>
        /// <param name="underlying">Underlying</param>
        /// <param name="onData">On Data Handler</param>
        /// <returns></returns>
        public virtual async Task<CallResult<UpdateSubscription>> SubscribeToOrderUpdatesAsync(
            OkxInstrumentType instrumentType,
            string instrumentId,
            string underlying,
            Action<OkxOrder> onData,
            CancellationToken ct = default)
        {
            var internalHandler = new Action<DataEvent<OkxSocketUpdateResponse<IEnumerable<OkxOrder>>>>(data =>
            {
                foreach (var d in data.Data.Data)
                    onData(d);
            });


            var request = new OkxSocketRequest(OkxSocketOperation.Subscribe, new OkxSocketRequestArgument
            {
                Channel = "orders",
                InstrumentId = instrumentId,
                InstrumentType = instrumentType,
                Underlying = underlying,
            });
            return await UnifiedSubscribeAsync(request, null, true, internalHandler, ct).ConfigureAwait(false);
        }

        /// <summary>
        /// Retrieve algo orders (includes trigger order, oco order, conditional order). Data will not be pushed when first subscribed. Data will only be pushed when triggered by events such as placing/canceling order.
        /// </summary>
        /// <param name="instrumentType">Instrument Type</param>
        /// <param name="instrumentId">Instrument ID</param>
        /// <param name="underlying">Underlying</param>
        /// <param name="onData">On Data Handler</param>
        /// <returns></returns>
        public virtual CallResult<UpdateSubscription> SubscribeToAlgoOrderUpdates(
            OkxInstrumentType instrumentType,
            string instrumentId,
            string underlying,
            Action<OkxAlgoOrder> onData)
            => SubscribeToAlgoOrderUpdatesAsync(instrumentType, instrumentId, underlying, onData).Result;
        /// <summary>
        /// Retrieve algo orders (includes trigger order, oco order, conditional order). Data will not be pushed when first subscribed. Data will only be pushed when triggered by events such as placing/canceling order.
        /// </summary>
        /// <param name="instrumentType">Instrument Type</param>
        /// <param name="instrumentId">Instrument ID</param>
        /// <param name="underlying">Underlying</param>
        /// <param name="onData">On Data Handler</param>
        /// <returns></returns>
        public virtual async Task<CallResult<UpdateSubscription>> SubscribeToAlgoOrderUpdatesAsync(
            OkxInstrumentType instrumentType,
            string instrumentId,
            string underlying,
            Action<OkxAlgoOrder> onData,
            CancellationToken ct = default)
        {
            var internalHandler = new Action<DataEvent<OkxSocketUpdateResponse<IEnumerable<OkxAlgoOrder>>>>(data =>
            {
                foreach (var d in data.Data.Data)
                    onData(d);
            });


            var request = new OkxSocketRequest(OkxSocketOperation.Subscribe, new OkxSocketRequestArgument
            {
                Channel = "orders-algo",
                InstrumentId = instrumentId,
                InstrumentType = instrumentType,
                Underlying = underlying,
            });
            return await UnifiedSubscribeAsync(request, null, true, internalHandler, ct).ConfigureAwait(false);
        }

        /// <summary>
        /// Retrieve advance algo orders (includes iceberg order and twap order). Data will be pushed when first subscribed. Data will be pushed when triggered by events such as placing/canceling order.
        /// </summary>
        /// <param name="instrumentType">Instrument Type</param>
        /// <param name="instrumentId">Instrument ID</param>
        /// <param name="underlying">Underlying</param>
        /// <param name="onData">On Data Handler</param>
        /// <returns></returns>
        public virtual CallResult<UpdateSubscription> SubscribeToAdvanceAlgoOrderUpdates(
            OkxInstrumentType instrumentType,
            string instrumentId,
            string underlying,
            Action<OkxAlgoOrder> onData)
            => SubscribeToAdvanceAlgoOrderUpdatesAsync(instrumentType, instrumentId, underlying, onData).Result;
        /// <summary>
        /// Retrieve advance algo orders (includes iceberg order and twap order). Data will be pushed when first subscribed. Data will be pushed when triggered by events such as placing/canceling order.
        /// </summary>
        /// <param name="instrumentType">Instrument Type</param>
        /// <param name="instrumentId">Instrument ID</param>
        /// <param name="underlying">Underlying</param>
        /// <param name="onData">On Data Handler</param>
        /// <returns></returns>
        public virtual async Task<CallResult<UpdateSubscription>> SubscribeToAdvanceAlgoOrderUpdatesAsync(
            OkxInstrumentType instrumentType,
            string instrumentId,
            string underlying,
            Action<OkxAlgoOrder> onData,
            CancellationToken ct = default)
        {
            var internalHandler = new Action<DataEvent<OkxSocketUpdateResponse<IEnumerable<OkxAlgoOrder>>>>(data =>
            {
                foreach (var d in data.Data.Data)
                    onData(d);
            });


            var request = new OkxSocketRequest(OkxSocketOperation.Subscribe, new OkxSocketRequestArgument
            {
                Channel = "algo-advance",
                InstrumentId = instrumentId,
                InstrumentType = instrumentType,
                Underlying = underlying,
            });
            return await UnifiedSubscribeAsync(request, null, true, internalHandler, ct).ConfigureAwait(false);
        }

        // TODO: Position risk warning
        // TODO: Account greeks channel
        // TODO: Rfqs channel
        // TODO: Quotes channel
        // TODO: Structure brick trades channel
        // TODO: Spot grid algo orders channel
        // TODO: Contract grid algo orders channel
        // TODO: Grid positions channel
        // TODO: Grid sub orders channel

        #endregion
    }
}