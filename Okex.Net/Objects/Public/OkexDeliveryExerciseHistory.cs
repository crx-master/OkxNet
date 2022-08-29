using Newtonsoft.Json;
using OkxNet.Converters;
using OkxNet.Enums;
using System;
using System.Collections.Generic;

namespace OkxNet.Objects.Public
{
    public class OkxDeliveryExerciseHistory
    {
        [JsonProperty("ts"), JsonConverter(typeof(OkxTimestampConverter))]
        public DateTime Time { get; set; }

        [JsonProperty("details")]
        public IEnumerable<OkxPublicDeliveryExerciseHistoryDetail> Details { get; set; }
    }

    public class OkxPublicDeliveryExerciseHistoryDetail
    {
        [JsonProperty("type"), JsonConverter(typeof(DeliveryExerciseHistoryTypeConverter))]
        public OkxDeliveryExerciseHistoryType Type { get; set; }

        [JsonProperty("insId")]
        public string Instrument { get; set; }

        [JsonProperty("px")]
        public decimal Price { get; set; }
    }
}
