using Newtonsoft.Json;
using SharpCryptoExchange.Okx.Converters;
using SharpCryptoExchange.Okx.Enums;
using System;
using System.Collections.Generic;

namespace SharpCryptoExchange.Okx.Objects.Public
{
    public class OkxInsuranceFund
    {
        [JsonProperty("total")]
        public decimal Total { get; set; }

        [JsonProperty("details")]
        public IEnumerable< OkxInsuranceFundDetail> Details { get; set; }
    }

    public class OkxInsuranceFundDetail
    {
        [JsonProperty("amt")]
        public decimal Amount { get; set; }

        [JsonProperty("balance")]
        public decimal Balance { get; set; }
        
        [JsonProperty("ccy")]
        public string Currency { get; set; }

        [JsonProperty("type"), JsonConverter(typeof(InsuranceTypeConverter))]
        public OkxInsuranceType Type { get; set; }

        [JsonProperty("ts"), JsonConverter(typeof(OkxTimestampConverter))]
        public DateTimeOffset Timestamp { get; set; }
    }
}
