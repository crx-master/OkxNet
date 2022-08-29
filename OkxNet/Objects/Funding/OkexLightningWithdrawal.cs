using Newtonsoft.Json;
using OkxNet.Converters;
using System;

namespace OkxNet.Objects.Funding
{
    public class OkxLightningWithdrawal
    {
        [JsonProperty("cTime"), JsonConverter(typeof(OkxTimestampConverter))]
        public DateTime Time { get; set; }

        [JsonProperty("wdId")]
        public string WithdrawalId { get; set; }
    }
}