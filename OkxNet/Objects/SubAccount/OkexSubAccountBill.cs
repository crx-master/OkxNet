using Newtonsoft.Json;
using OkxNet.Converters;
using OkxNet.Enums;
using System;

namespace OkxNet.Objects.SubAccount
{
    public class OkxSubAccountBill
    {
        [JsonProperty("billId")]
        public long BillId { get; set; }

        [JsonProperty("ts"), JsonConverter(typeof(OkxTimestampConverter))]
        public DateTime Time { get; set; }

        [JsonProperty("type"), JsonConverter(typeof(SubAccountTransferTypeConverter))]
        public OkxSubAccountTransferType Type { get; set; }

        [JsonProperty("ccy")]
        public string Currency { get; set; }

        [JsonProperty("subAcct")]
        public string SubAccountName { get; set; }

        [JsonProperty("amt")]
        public decimal? Amount { get; set; }
    }
}
