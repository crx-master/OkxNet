using Newtonsoft.Json;
using System.Collections.Generic;

namespace SharpCryptoExchange.Okx.Models.Trading
{
    public class OkxSupportCoins
    {
        [JsonProperty("contract")]
        public IEnumerable<string> Contract { get; set; }

        [JsonProperty("option")]
        public IEnumerable<string> Option { get; set; }

        [JsonProperty("spot")]
        public IEnumerable<string> Spot { get; set; }
    }
}
