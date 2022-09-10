using Newtonsoft.Json;
using OkxNet.Converters;
using OkxNet.Enums;
using System;
using System.Diagnostics.Contracts;

namespace OkxNet.Objects.Market
{
    public class OkxTicker
    {
        /// <summary>
        /// Instrument type: SPOT, MARGIN, SWAP, FUTURES, OPTION, ANY
        /// </summary>
        [JsonProperty("instType"), JsonConverter(typeof(InstrumentTypeConverter))]
        public OkxInstrumentType InstrumentType { get; set; }

        /// <summary>
        /// Instrument ID string
        /// </summary>
        [JsonProperty("instId")]
        public string Instrument { get; set; }

        /// <summary>
        /// Last traded price
        /// </summary>
        [JsonProperty("last")]
        public decimal LastTradedPrice { get; set; }

        /// <summary>
        /// Last traded size
        /// </summary>
        [JsonProperty("lastSz")]
        public decimal LastTradedSize { get; set; }

        /// <summary>
        /// Best ask price
        /// </summary>
        [JsonProperty("askPx")]
        public decimal BestAskPrice { get; set; }

        /// <summary>
        /// Best ask size
        /// </summary>
        [JsonProperty("askSz")]
        public decimal BestAskSize { get; set; }

        /// <summary>
        /// Best bid price
        /// </summary>
        [JsonProperty("bidPx")]
        public decimal BestBidPrice { get; set; }

        /// <summary>
        /// Best bid size
        /// </summary>
        [JsonProperty("bidSz")]
        public decimal BestBidSize { get; set; }

        /// <summary>
        /// Open price in the past 24 hours
        /// </summary>
        [JsonProperty("open24h")]
        public decimal Open { get; set; }

        /// <summary>
        /// Highest price in the past 24 hours
        /// </summary>
        [JsonProperty("high24h")]
        public decimal High { get; set; }

        /// <summary>
        /// Lowest price in the past 24 hours
        /// </summary>
        [JsonProperty("low24h")]
        public decimal Low { get; set; }

        /// <summary>
        /// 24h trading volume, with a unit of currency.
        /// If it is a derivatives contract, the value is the number of base currency.
        /// If it is SPOT/MARGIN, the value is the quantity in quote currency.
        /// </summary>
        [JsonProperty("volCcy24h")]
        public decimal VolumeCurrency { get; set; }

        /// <summary>
        /// 24h trading volume, with a unit of contract.
        /// If it is a derivatives contract, the value is the number of contracts. 
        /// If it is SPOT/MARGIN, the value is the quantity in base currency.
        /// </summary>
        [JsonProperty("vol24h")]
        public decimal Volume { get; set; }

        /// <summary>
        /// Open price in the UTC 0
        /// </summary>
        [JsonProperty("sodUtc0")]
        public decimal OpenPriceUtc0 { get; set; }

        /// <summary>
        /// Open price in the UTC 8
        /// </summary>
        [JsonProperty("sodUtc8")]
        public decimal OpenPriceUtc8 { get; set; }

        /// <summary>
        /// Ticker data generation time, Unix timestamp format in milliseconds, e.g. 1597026383085
        /// </summary>
        [JsonProperty("ts"), JsonConverter(typeof(OkxTimestampConverter))]
        public DateTimeOffset Time { get; set; }
    }
}
