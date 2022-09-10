﻿using Newtonsoft.Json;
using OkxNet.Converters;
using OkxNet.Enums;
using System;

namespace OkxNet.Objects.Trade
{
    public class OkxTransaction
    {
        [JsonProperty("instType"), JsonConverter(typeof(InstrumentTypeConverter))]
        public OkxInstrumentType InstrumentType { get; set; }

        [JsonProperty("instId")]
        public string Instrument { get; set; }

        [JsonProperty("tradeId")]
        public long? TradeId { get; set; }

        [JsonProperty("ordId")]
        public long? OrderId { get; set; }

        [JsonProperty("clOrdId")]
        public string ClientOrderId { get; set; }

        [JsonProperty("billId")]
        public long? BillId { get; set; }

        [JsonProperty("tag")]
        public string Tag { get; set; }

        [JsonProperty("fillPx")]
        public decimal? FillPrice { get; set; }

        [JsonProperty("fillSz")]
        public decimal? FillQuantity { get; set; }

        [JsonProperty("side"), JsonConverter(typeof(OrderSideConverter))]
        public OkxOrderSide OrderSide { get; set; }

        [JsonProperty("posSide"), JsonConverter(typeof(PositionSideConverter))]
        public OkxPositionSide PositionSide { get; set; }

        [JsonProperty("execType"), JsonConverter(typeof(OrderFlowTypeConverter))]
        public OkxOrderFlowType OrderFlowType { get; set; }

        [JsonProperty("feeCcy")]
        public string FeeCurrency { get; set; }

        [JsonProperty("fee")]
        public decimal? Fee { get; set; }

        [JsonProperty("ts"), JsonConverter(typeof(OkxTimestampConverter))]
        public DateTimeOffset Time { get; set; }
    }
}