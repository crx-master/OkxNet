using Newtonsoft.Json;
using OkxNet.Converters;
using System;

namespace OkxNet.Objects.Public
{
    public class OkxTime
    {
        /// <summary>
        /// System time, Unix timestamp format in milliseconds, e.g. 1597026383085
        /// </summary>
        [JsonProperty("ts"), JsonConverter(typeof(OkxTimestampConverter))]
        public DateTime Time { get; set; }
    }
}
