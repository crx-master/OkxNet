using Newtonsoft.Json;

namespace OkxNet.Objects.SubAccount
{
    public class OkxSubAccountTransfer
    {
        [JsonProperty("transId")]
        public long? TransferId { get; set; }
    }
}
