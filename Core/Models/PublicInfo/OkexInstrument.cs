using Newtonsoft.Json;
using SharpCryptoExchange.Okx.Converters;
using SharpCryptoExchange.Okx.Enums;
using System;

namespace SharpCryptoExchange.Okx.Models.PublicInfo
{
    public class OkxInstrument
    {
        /// <summary>
        /// Instrument type
        /// </summary>
        [JsonProperty("instType"), JsonConverter(typeof(InstrumentTypeConverter))]
        public OkxInstrumentType InstrumentType { get; set; }

        /// <summary>
        /// Instrument ID, e.g. BTC-USD-SWAP
        /// </summary>
        [JsonProperty("instId")]
        public string Instrument { get; set; }

        /// <summary>
        /// Underlying, e.g. BTC-USD. Only applicable to FUTURES/SWAP/OPTION
        /// </summary>
        [JsonProperty("uly")]
        public string Underlying { get; set; }

        /// <summary>
        /// Fee Schedule
        /// </summary>
        [JsonProperty("category")]
        public string Category { get; set; }

        /// <summary>
        /// Base currency, e.g. BTC in BTC-USDT
        /// Only applicable to SPOT/MARGIN
        /// </summary>
        [JsonProperty("baseCcy")]
        public string BaseCurrency { get; set; }

        /// <summary>
        /// Quote currency, e.g. USDT in BTC-USDT
        /// Only applicable to SPOT/MARGIN
        /// </summary>
        [JsonProperty("quoteCcy")]
        public string QuoteCurrency { get; set; }

        /// <summary>
        /// Settlement and margin currency, e.g. BTC
        /// Only applicable to FUTURES/SWAP/OPTION
        /// </summary>
        [JsonProperty("settleCcy")]
        public string SettlementCurrency { get; set; }

        /// <summary>
        /// Contract value
        /// </summary>
        [JsonProperty("ctVal")]
        public decimal? ContractValue { get; set; }

        /// <summary>
        /// Contract multiplier
        /// </summary>
        [JsonProperty("ctMult")]
        public decimal? ContractMultiplier { get; set; }

        /// <summary>
        /// Contract value currency
        /// </summary>
        [JsonProperty("ctValCcy")]
        public string ContractValueCurrency { get; set; }

        /// <summary>
        /// Option type, C: Call P: Put
        /// Only applicable to OPTION
        /// </summary>
        [JsonProperty("optType"), JsonConverter(typeof(OptionTypeConverter))]
        public OkxOptionType? OptionType { get; set; }

        /// <summary>
        /// Strike price
        /// Only applicable to OPTION
        /// </summary>
        [JsonProperty("stk")]
        public decimal? StrikePrice { get; set; }

        /// <summary>
        /// Listing time
        /// Only applicable to FUTURES/SWAP/OPTION
        /// </summary>
        [JsonProperty("listTime"), JsonConverter(typeof(OkxTimestampConverter))]
        public DateTimeOffset? ListingTime { get; set; }

        /// <summary>
        /// Expiry time
        /// Only applicable to FUTURES/OPTION
        /// </summary>
        [JsonProperty("expTime"), JsonConverter(typeof(OkxTimestampConverter))]
        public DateTimeOffset? ExpiryTime { get; set; }

        /// <summary>
        /// Max Leverage
        /// Not applicable to SPOT OPTION, used to distinguish between MARGIN and SPOT.
        /// </summary>
        [JsonProperty("lever")]
        public int? MaxLeverage { get; set; }

        /// <summary>
        /// Tick size, e.g. 0.0001
        /// </summary>
        [JsonProperty("tickSz")]
        public decimal TickSize { get; set; }

        /// <summary>
        /// Lot size,e.g. BTC-USDT-SWAP: 1
        /// </summary>
        [JsonProperty("lotSz")]
        public decimal LotSize { get; set; }

        /// <summary>
        /// Minimum order size
        /// </summary>
        [JsonProperty("minSz")]
        public decimal MinimumOrderSize { get; set; }

        /// <summary>
        /// Contract type, linear: linear contract inverse: inverse contract
        /// Only applicable to FUTURES/SWAP
        /// </summary>
        [JsonProperty("ctType"), JsonConverter(typeof(ContractTypeConverter))]
        public OkxContractType? ContractType { get; set; }

        /// <summary>
        /// Alias: this_week, next_week, quarter, next_quarter
        /// Only applicable to FUTURES
        /// </summary>
        [JsonProperty("alias"), JsonConverter(typeof(InstrumentAliasConverter))]
        public OkxInstrumentAlias? Alias { get; set; }

        /// <summary>
        /// Instrument status: live, suspend, expired, preopen
        /// </summary>
        [JsonProperty("state"), JsonConverter(typeof(InstrumentStateConverter))]
        public OkxInstrumentState State { get; set; }
    }
}
