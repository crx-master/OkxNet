using Newtonsoft.Json;
using OkxNet.Converters;
using System;

namespace OkxNet.Objects.Funding
{
    public class OkxLightningDeposit
    {
        [JsonProperty("cTime"), JsonConverter(typeof(OkxTimestampConverter))]
        public DateTime Time { get; set; }

        [JsonProperty("invoice")]
        public string Invoice { get; set; }
    }
}