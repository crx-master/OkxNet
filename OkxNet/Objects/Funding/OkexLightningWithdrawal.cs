using Newtonsoft.Json;
using OkxNet.Converters;
using System;

namespace OkxNet.Objects.Funding
{
    public class OkxLightningWithdrawal
    {
        [JsonProperty("cTime"), JsonConverter(typeof(OkxTimestampConverter))]
        public DateTimeOffset Time { get; set; }

        [JsonProperty("wdId")]
        public string WithdrawalId { get; set; }
    }
}