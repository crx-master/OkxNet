using Newtonsoft.Json;
using SharpCryptoExchange.Okx.Converters;
using SharpCryptoExchange.Okx.Enums;
using System;

namespace SharpCryptoExchange.Okx.Objects.Funding
{
    public class OkxWithdrawalHistory
    {
        [JsonProperty("ts"), JsonConverter(typeof(OkxTimestampConverter))]
        public DateTimeOffset Time { get; set; }

        [JsonProperty("wdId")]
        public long WithdrawalId { get; set; }

        [JsonProperty("state"), JsonConverter(typeof(WithdrawalStateConverter))]
        public OkxWithdrawalState State { get; set; }

        [JsonProperty("ccy")]
        public string Currency { get; set; }

        [JsonProperty("chain")]
        public string Chain { get; set; }

        [JsonProperty("txId")]
        public string TransactionId { get; set; }

        [JsonProperty("from")]
        public string From { get; set; }

        [JsonProperty("to")]
        public string To { get; set; }

        [JsonProperty("fee")]
        public decimal Fee { get; set; }

        [JsonProperty("amt")]
        public decimal Amount { get; set; }
    }
}