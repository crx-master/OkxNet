using Newtonsoft.Json;

namespace OkxNet.Objects.SubAccount
{
    public class OkxSubAccountName
    {
        [JsonProperty("subAcct")]
        public string SubAccountName { get; set; }
    }
}
