using Newtonsoft.Json;
using OkxNet.Converters;
using System;

namespace OkxNet.Objects.Funding
{
    public class OkxLightningDeposit
    {
        [JsonProperty("cTime"), JsonConverter(typeof(OkxTimestampConverter))]
        public DateTimeOffset Time { get; set; }

        [JsonProperty("invoice")]
        public string Invoice { get; set; }
    }
}