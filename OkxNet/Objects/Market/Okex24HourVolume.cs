using Newtonsoft.Json;
using OkxNet.Converters;
using System;

namespace OkxNet.Objects.Market
{
    public class Okx24HourVolume
    {
        [JsonProperty("volUsd")]
        public decimal VolumeUsd { get; set; }

        [JsonProperty("volCny")]
        public decimal VolumeCny { get; set; }

        [JsonProperty("ts"), JsonConverter(typeof(OkxTimestampConverter))]
        public DateTimeOffset Time { get; set; }
    }
}
