﻿using Newtonsoft.Json;

namespace OkxNet.Objects.Trade
{
    public class OkxAlgoOrderResponse
    {
        [JsonProperty("algoId")]
        public long? AlgoOrderId { get; set; }

        [JsonProperty("sCode")]
        public string Code { get; set; }

        [JsonProperty("sMsg")]
        public string Message { get; set; }
    }
}
