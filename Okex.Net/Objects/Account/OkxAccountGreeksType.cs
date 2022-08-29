using Newtonsoft.Json;
using OkxNet.Converters;
using OkxNet.Enums;

namespace OkxNet.Objects.Account
{
    public class OkxAccountGreeksType
    {
        [JsonProperty("greeksType"), JsonConverter(typeof(GreeksTypeConverter))]
        public OkxGreeksType GreeksType { get; set; }
    }
}
