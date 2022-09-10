using Newtonsoft.Json;
using OkxNet.Converters;
using OkxNet.Enums;
using System;

namespace OkxNet.Objects.Funding
{
    public class OkxFundingBill
    {
        [JsonProperty("billId")]
        public long? BillId { get; set; }

        [JsonProperty("ccy")]
        public string Currency { get; set; }

        [JsonProperty("bal")]
        public decimal? Balance { get; set; }

        [JsonProperty("balChg")]
        public decimal? BalanceChange { get; set; }

        [JsonProperty("type"), JsonConverter(typeof(FundingBillTypeConverter))]
        public OkxFundingBillType Type { get; set; }

        [JsonProperty("ts"), JsonConverter(typeof(OkxTimestampConverter))]
        public DateTimeOffset Time { get; set; }
    }
}