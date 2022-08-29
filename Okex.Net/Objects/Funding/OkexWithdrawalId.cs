using Newtonsoft.Json;

namespace OkxNet.Objects.Funding
{
    public class OkxWithdrawalId
    {
        [JsonProperty("wdId")]
        public string WithdrawalId { get; set; }
    }
}