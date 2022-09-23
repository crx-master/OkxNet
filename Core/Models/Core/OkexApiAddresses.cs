namespace SharpCryptoExchange.Okx.Models.Core
{
    public class OkxApiAddresses
    {
        public string UnifiedAddress { get; set; }

        public readonly static OkxApiAddresses Default = new OkxApiAddresses
        {
            UnifiedAddress = "https://www.okx.com",
        };
    }
}
